using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenovo.Modern.Utilities.Patterns.Ioc
{
	// Token: 0x02000039 RID: 57
	internal class MultiInstanceBuilder : IInstanceBuilder
	{
		// Token: 0x06000172 RID: 370 RVA: 0x0000732A File Offset: 0x0000552A
		public IInstanceBuilder WithFactoryFunction(Func<object> factoryFunction)
		{
			throw new NotSupportedException("factory function can not be specified for a multi-instance registration");
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000732A File Offset: 0x0000552A
		public IInstanceBuilder WithFactoryFunction(Func<IDictionary<string, object>, object> factoryFunction)
		{
			throw new NotSupportedException("factory function can not be specified for a multi-instance registration");
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007338 File Offset: 0x00005538
		public IInstanceBuilder WithTransientLifecycle()
		{
			foreach (IInstanceBuilder instanceBuilder in this._builders)
			{
				instanceBuilder.WithTransientLifecycle();
			}
			return this;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000738C File Offset: 0x0000558C
		public IInstanceBuilder WithSingletonLifecycle()
		{
			foreach (IInstanceBuilder instanceBuilder in this._builders)
			{
				instanceBuilder.WithSingletonLifecycle();
			}
			return this;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000073E0 File Offset: 0x000055E0
		public IInstanceBuilder WithThreadLocalLifecycle()
		{
			foreach (IInstanceBuilder instanceBuilder in this._builders)
			{
				instanceBuilder.WithThreadLocalLifecycle();
			}
			return this;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007434 File Offset: 0x00005634
		public IInstanceBuilder WithArguments(IDictionary<string, object> arguments)
		{
			foreach (IInstanceBuilder instanceBuilder in this._builders)
			{
				instanceBuilder.WithArguments(arguments);
			}
			return this;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007488 File Offset: 0x00005688
		public void CreateImmediately()
		{
			foreach (IInstanceBuilder instanceBuilder in this._builders)
			{
				instanceBuilder.CreateImmediately();
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000074D8 File Offset: 0x000056D8
		internal MultiInstanceBuilder(IEnumerable<IInstanceBuilder> builders)
		{
			this._builders = builders.ToList<IInstanceBuilder>();
		}

		// Token: 0x04000074 RID: 116
		private List<IInstanceBuilder> _builders;
	}
}
