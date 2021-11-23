using System;
using System.Collections.Generic;
using System.Threading;

namespace Lenovo.Modern.Utilities.Patterns.Ioc
{
	// Token: 0x02000038 RID: 56
	internal class InstanceBuilder : IInstanceBuilder
	{
		// Token: 0x06000169 RID: 361 RVA: 0x000071D4 File Offset: 0x000053D4
		public IInstanceBuilder WithFactoryFunction(Func<object> factoryFunction)
		{
			return this.WithFactoryFunction((IDictionary<string, object> ps) => factoryFunction());
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007200 File Offset: 0x00005400
		public IInstanceBuilder WithFactoryFunction(Func<IDictionary<string, object>, object> factoryFunction)
		{
			if (this._factoryFunctionSet)
			{
				throw new InvalidOperationException("factory function already set for this registration");
			}
			this._instanceData.FactoryFunction = factoryFunction;
			this._factoryFunctionSet = true;
			return this;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007229 File Offset: 0x00005429
		public IInstanceBuilder WithTransientLifecycle()
		{
			return this.WithLifecycle(InstanceLifecycle.Transient);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007232 File Offset: 0x00005432
		public IInstanceBuilder WithSingletonLifecycle()
		{
			return this.WithLifecycle(InstanceLifecycle.Singleton);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000723B File Offset: 0x0000543B
		public IInstanceBuilder WithThreadLocalLifecycle()
		{
			this.WithLifecycle(InstanceLifecycle.ThreadLocal);
			this._instanceData.ThreadLocalInstance = new ThreadLocal<object>(true);
			return this;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007257 File Offset: 0x00005457
		public IInstanceBuilder WithArguments(IDictionary<string, object> arguments)
		{
			if (this._instanceData.ConstructorArguments != null)
			{
				throw new InvalidOperationException("constructor parameters have already been set for this registration");
			}
			this._instanceData.ConstructorArguments = arguments;
			return this;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007280 File Offset: 0x00005480
		public void CreateImmediately()
		{
			if (this._instanceData.Lifecycle == InstanceLifecycle.Transient)
			{
				throw new InvalidOperationException("cannot create a transient object immediately");
			}
			if (this._instanceData.Lifecycle == InstanceLifecycle.ThreadLocal)
			{
				throw new InvalidOperationException("cannot create a threadlocal object immediately");
			}
			object value = this._container.GetInstanceLazy(this._container, this._instanceData, null).Value;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000072DD File Offset: 0x000054DD
		internal InstanceBuilder(InstanceContainer container, InstanceContainer.InstanceData instanceData)
		{
			this._container = container;
			this._instanceData = instanceData;
			this._lifecycleSet = false;
			this._factoryFunctionSet = false;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007301 File Offset: 0x00005501
		private InstanceBuilder WithLifecycle(InstanceLifecycle lifecycle)
		{
			if (this._lifecycleSet)
			{
				throw new InvalidOperationException("instance lifecycle already set for this registration");
			}
			this._instanceData.Lifecycle = lifecycle;
			this._lifecycleSet = true;
			return this;
		}

		// Token: 0x04000070 RID: 112
		private bool _lifecycleSet;

		// Token: 0x04000071 RID: 113
		private bool _factoryFunctionSet;

		// Token: 0x04000072 RID: 114
		private InstanceContainer _container;

		// Token: 0x04000073 RID: 115
		private InstanceContainer.InstanceData _instanceData;
	}
}
