using System;
using System.Collections.Generic;

namespace Lenovo.Modern.Utilities.Patterns.Ioc
{
	// Token: 0x02000037 RID: 55
	public interface IInstanceBuilder
	{
		// Token: 0x06000162 RID: 354
		IInstanceBuilder WithFactoryFunction(Func<object> factoryFunction);

		// Token: 0x06000163 RID: 355
		IInstanceBuilder WithFactoryFunction(Func<IDictionary<string, object>, object> factoryFunction);

		// Token: 0x06000164 RID: 356
		IInstanceBuilder WithTransientLifecycle();

		// Token: 0x06000165 RID: 357
		IInstanceBuilder WithSingletonLifecycle();

		// Token: 0x06000166 RID: 358
		IInstanceBuilder WithThreadLocalLifecycle();

		// Token: 0x06000167 RID: 359
		IInstanceBuilder WithArguments(IDictionary<string, object> arguments);

		// Token: 0x06000168 RID: 360
		void CreateImmediately();
	}
}
