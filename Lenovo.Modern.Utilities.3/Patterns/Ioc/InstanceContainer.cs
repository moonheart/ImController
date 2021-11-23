using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Lenovo.Modern.Utilities.Patterns.Ioc
{
	// Token: 0x0200003A RID: 58
	public class InstanceContainer : IDisposable
	{
		// Token: 0x0600017A RID: 378 RVA: 0x000074EC File Offset: 0x000056EC
		public static InstanceContainer GetInstance()
		{
			InstanceContainer result;
			if ((result = InstanceContainer._instance) == null)
			{
				result = (InstanceContainer._instance = new InstanceContainer());
			}
			return result;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007502 File Offset: 0x00005702
		public InstanceContainer()
		{
			this._instanceDataDictionary = new Dictionary<Type, Dictionary<string, List<InstanceContainer.InstanceData>>>();
			this._ownedObjects = new List<IDisposable>();
			this._containerLock = new object();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000752B File Offset: 0x0000572B
		public InstanceContainer CreateChildContainer()
		{
			return new InstanceContainer(this);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007533 File Offset: 0x00005733
		public IInstanceBuilder Register<TClass>() where TClass : class
		{
			return this.Register<TClass, TClass>(string.Empty);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007540 File Offset: 0x00005740
		public IInstanceBuilder Register<TClass>(string key) where TClass : class
		{
			return this.Register<TClass, TClass>(key);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007549 File Offset: 0x00005749
		public IInstanceBuilder Register<TInterface, TClass>() where TInterface : class where TClass : class
		{
			return this.Register<TInterface, TClass>(string.Empty);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007556 File Offset: 0x00005756
		public IInstanceBuilder Register<TInterface, TClass>(string key) where TInterface : class where TClass : class
		{
			return this.RegisterHelper(typeof(TInterface), typeof(TClass), key);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007573 File Offset: 0x00005773
		public IInstanceBuilder RegisterAll<TInterface>()
		{
			return this.RegisterAll<TInterface>(string.Empty);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007580 File Offset: 0x00005780
		public IInstanceBuilder RegisterAll<TInterface>(string key)
		{
			return new MultiInstanceBuilder(from a in AppDomain.CurrentDomain.GetAssemblies()
				select this.RegisterAll<TInterface>(a, key));
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000075C1 File Offset: 0x000057C1
		public IInstanceBuilder RegisterAll<TInterface>(Assembly assembly)
		{
			return this.RegisterAll<TInterface>(assembly, string.Empty);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000075D0 File Offset: 0x000057D0
		public IInstanceBuilder RegisterAll<TInterface>(Assembly assembly, string key)
		{
			return new MultiInstanceBuilder(from t in assembly.GetTypes()
				where t.IsPublic && t.IsClass && typeof(TInterface).IsAssignableFrom(t)
				select this.RegisterHelper(typeof(TInterface), t, key));
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007634 File Offset: 0x00005834
		public void RegisterInstance<TClass>(TClass instance) where TClass : class
		{
			this.Register<TClass, TClass>().WithFactoryFunction(() => instance).CreateImmediately();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000766C File Offset: 0x0000586C
		public void RegisterInstance<TClass>(TClass instance, string key) where TClass : class
		{
			this.Register<TClass, TClass>(key).WithFactoryFunction(() => instance).CreateImmediately();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000076A3 File Offset: 0x000058A3
		public TInterface Resolve<TInterface>() where TInterface : class
		{
			return this.Resolve<TInterface>(string.Empty, null);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000076B1 File Offset: 0x000058B1
		public TInterface Resolve<TInterface>(string key) where TInterface : class
		{
			return this.Resolve<TInterface>(key, null);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000076BB File Offset: 0x000058BB
		public TInterface Resolve<TInterface>(IDictionary<string, object> constructorArguments) where TInterface : class
		{
			return this.Resolve<TInterface>(string.Empty, constructorArguments);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000076CC File Offset: 0x000058CC
		public TInterface Resolve<TInterface>(string key, IDictionary<string, object> constructorArguments) where TInterface : class
		{
			List<Lazy<object>> list = this.ResolveAllLazy(this, typeof(TInterface), key, constructorArguments);
			if (list.Count != 1)
			{
				throw new ArgumentException(string.Format("no unique instance is registered for type '{0}', key {1}", typeof(TInterface).Name, key));
			}
			return (TInterface)((object)list.First<Lazy<object>>().Value);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007724 File Offset: 0x00005924
		public IEnumerable<TInterface> ResolveAll<TInterface>() where TInterface : class
		{
			return this.ResolveAll<TInterface>(string.Empty, null);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007732 File Offset: 0x00005932
		public IEnumerable<TInterface> ResolveAll<TInterface>(string key) where TInterface : class
		{
			return this.ResolveAll<TInterface>(key, null);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000773C File Offset: 0x0000593C
		public IEnumerable<TInterface> ResolveAll<TInterface>(IDictionary<string, object> constructorArguments) where TInterface : class
		{
			return this.ResolveAll<TInterface>(string.Empty, constructorArguments);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000774C File Offset: 0x0000594C
		public IEnumerable<TInterface> ResolveAll<TInterface>(string key, IDictionary<string, object> constructorArguments) where TInterface : class
		{
			List<Lazy<object>> list = this.ResolveAllLazy(this, typeof(TInterface), key, constructorArguments);
			if (list.Count == 0)
			{
				throw new ArgumentException(string.Format("no instance available for type '{0}', key '{1}'", typeof(TInterface).Name, key));
			}
			return (from lazy in list
				select (TInterface)((object)lazy.Value)).ToList<TInterface>();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000077BD File Offset: 0x000059BD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000077CC File Offset: 0x000059CC
		private InstanceContainer(InstanceContainer parent)
			: this()
		{
			this._parent = parent;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000077DC File Offset: 0x000059DC
		private IInstanceBuilder RegisterHelper(Type interfaceType, Type classType, string key)
		{
			key = key ?? string.Empty;
			object containerLock = this._containerLock;
			bool flag = false;
			IInstanceBuilder result;
			try
			{
				Monitor.Enter(containerLock, ref flag);
				Dictionary<string, List<InstanceContainer.InstanceData>> dictionary = null;
				if (!this._instanceDataDictionary.TryGetValue(interfaceType, out dictionary))
				{
					dictionary = (this._instanceDataDictionary[interfaceType] = new Dictionary<string, List<InstanceContainer.InstanceData>>());
				}
				if (!dictionary.ContainsKey(key))
				{
					dictionary[key] = new List<InstanceContainer.InstanceData>();
				}
				InstanceContainer.InstanceData instanceData = new InstanceContainer.InstanceData();
				instanceData.Lifecycle = InstanceLifecycle.Singleton;
				instanceData.FactoryFunction = (IDictionary<string, object> args) => this.DefaultFactoryFunction(classType, instanceData, args);
				dictionary[key].Add(instanceData);
				result = new InstanceBuilder(this, instanceData);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(containerLock);
				}
			}
			return result;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000078D8 File Offset: 0x00005AD8
		private List<Lazy<object>> ResolveAllLazy(InstanceContainer owningContainer, Type interfaceType, string key, IDictionary<string, object> constructorArguments)
		{
			List<Lazy<object>> list = new List<Lazy<object>>();
			object containerLock = this._containerLock;
			lock (containerLock)
			{
				Dictionary<string, List<InstanceContainer.InstanceData>> dictionary = null;
				if (this._instanceDataDictionary.TryGetValue(interfaceType, out dictionary))
				{
					List<InstanceContainer.InstanceData> source = null;
					if (dictionary.TryGetValue(key, out source))
					{
						list.AddRange(from id in source
							select this.GetInstanceLazy(owningContainer, id, constructorArguments));
					}
				}
			}
			if (this._parent != null)
			{
				list.AddRange(this._parent.ResolveAllLazy(owningContainer, interfaceType, key, constructorArguments));
			}
			return list;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007998 File Offset: 0x00005B98
		internal Lazy<object> GetInstanceLazy(InstanceContainer owningContainer, InstanceContainer.InstanceData instanceData, IDictionary<string, object> constructorArguments)
		{
			Dictionary<string, object> mergedConstructorArguments = new Dictionary<string, object>();
			if (constructorArguments != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in constructorArguments)
				{
					mergedConstructorArguments[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (instanceData.ConstructorArguments != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair2 in instanceData.ConstructorArguments)
				{
					if (!mergedConstructorArguments.ContainsKey(keyValuePair2.Key))
					{
						mergedConstructorArguments[keyValuePair2.Key] = keyValuePair2.Value;
					}
				}
			}
			switch (instanceData.Lifecycle)
			{
			case InstanceLifecycle.Singleton:
				return new Lazy<object>(delegate()
				{
					object containerLock = this._containerLock;
					object singletonInstance;
					lock (containerLock)
					{
						if (instanceData.SingletonInstance == null)
						{
							instanceData.SingletonInstance = instanceData.FactoryFunction(mergedConstructorArguments);
							if (instanceData.SingletonInstance == null)
							{
								throw new InvalidOperationException(string.Format("instance factory did not return an instance", new object[0]));
							}
							if (instanceData.SingletonInstance is IDisposable)
							{
								owningContainer._ownedObjects.Add((IDisposable)instanceData.SingletonInstance);
							}
						}
						singletonInstance = instanceData.SingletonInstance;
					}
					return singletonInstance;
				});
			case InstanceLifecycle.Transient:
				return new Lazy<object>(() => instanceData.FactoryFunction(mergedConstructorArguments));
			case InstanceLifecycle.ThreadLocal:
				return new Lazy<object>(delegate()
				{
					if (!instanceData.ThreadLocalInstance.IsValueCreated)
					{
						instanceData.ThreadLocalInstance.Value = instanceData.FactoryFunction(mergedConstructorArguments);
						if (!instanceData.ThreadLocalInstance.IsValueCreated)
						{
							throw new InvalidOperationException(string.Format("instance factory did not return an instance", new object[0]));
						}
						if (instanceData.ThreadLocalInstance.Value is IDisposable)
						{
							object containerLock = this._containerLock;
							lock (containerLock)
							{
								if (instanceData.ThreadLocalInstance.Values.Count == 1)
								{
									owningContainer._ownedObjects.Add(instanceData.ThreadLocalInstance);
								}
							}
						}
					}
					return instanceData.ThreadLocalInstance.Value;
				});
			default:
				throw new InvalidOperationException("invalid instance lifecycle");
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007AF4 File Offset: 0x00005CF4
		private object DefaultFactoryFunction(Type classType, InstanceContainer.InstanceData instanceData, IDictionary<string, object> constructorArguments)
		{
			if (instanceData.Lifecycle == InstanceLifecycle.Singleton)
			{
				MethodInfo methodInfo = classType.GetTypeInfo().DeclaredMethods.FirstOrDefault((MethodInfo m) => m.IsPublic && m.IsStatic && m.Name.Equals("GetInstance") && m.GetParameters().Count<ParameterInfo>() == 0 && m.ReturnType.Equals(classType));
				if (methodInfo != null)
				{
					return methodInfo.Invoke(null, null);
				}
			}
			List<ConstructorInfo> source = (from c in classType.GetTypeInfo().DeclaredConstructors
				where c.IsPublic
				select c).ToList<ConstructorInfo>();
			if (source.Count<ConstructorInfo>() != 1)
			{
				throw new InvalidOperationException("there must be exactly one constructor");
			}
			ConstructorInfo constructorInfo = source.First<ConstructorInfo>();
			object[] parameters = constructorInfo.GetParameters().Select(delegate(ParameterInfo p)
			{
				if (constructorArguments.ContainsKey(p.Name))
				{
					return constructorArguments[p.Name];
				}
				return this.GetType().GetTypeInfo().DeclaredMethods.First((MethodInfo m) => m.Name.Equals("Resolve") && m.GetParameters().Length == 0).MakeGenericMethod(new Type[] { p.ParameterType }).Invoke(this, null);
			}).ToArray<object>();
			return constructorInfo.Invoke(parameters);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007BD0 File Offset: 0x00005DD0
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				object containerLock = this._containerLock;
				lock (containerLock)
				{
					foreach (IDisposable disposable in this._ownedObjects)
					{
						disposable.Dispose();
					}
					this._instanceDataDictionary.Clear();
					this._ownedObjects.Clear();
				}
			}
		}

		// Token: 0x04000075 RID: 117
		private Dictionary<Type, Dictionary<string, List<InstanceContainer.InstanceData>>> _instanceDataDictionary;

		// Token: 0x04000076 RID: 118
		private List<IDisposable> _ownedObjects;

		// Token: 0x04000077 RID: 119
		private object _containerLock;

		// Token: 0x04000078 RID: 120
		private InstanceContainer _parent;

		// Token: 0x04000079 RID: 121
		private static InstanceContainer _instance;

		// Token: 0x02000093 RID: 147
		internal class InstanceData
		{
			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000213 RID: 531 RVA: 0x00009F66 File Offset: 0x00008166
			// (set) Token: 0x06000214 RID: 532 RVA: 0x00009F6E File Offset: 0x0000816E
			public IDictionary<string, object> ConstructorArguments { get; set; }

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x06000215 RID: 533 RVA: 0x00009F77 File Offset: 0x00008177
			// (set) Token: 0x06000216 RID: 534 RVA: 0x00009F7F File Offset: 0x0000817F
			public Func<IDictionary<string, object>, object> FactoryFunction { get; set; }

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x06000217 RID: 535 RVA: 0x00009F88 File Offset: 0x00008188
			// (set) Token: 0x06000218 RID: 536 RVA: 0x00009F90 File Offset: 0x00008190
			public object SingletonInstance { get; set; }

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x06000219 RID: 537 RVA: 0x00009F99 File Offset: 0x00008199
			// (set) Token: 0x0600021A RID: 538 RVA: 0x00009FA1 File Offset: 0x000081A1
			public ThreadLocal<object> ThreadLocalInstance { get; set; }

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x0600021B RID: 539 RVA: 0x00009FAA File Offset: 0x000081AA
			// (set) Token: 0x0600021C RID: 540 RVA: 0x00009FB2 File Offset: 0x000081B2
			public InstanceLifecycle Lifecycle { get; set; }
		}
	}
}
