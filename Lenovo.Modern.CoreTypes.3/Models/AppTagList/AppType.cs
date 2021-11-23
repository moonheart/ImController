using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x02000019 RID: 25
	[DataContract(Namespace = "")]
	public enum AppType
	{
		// Token: 0x04000065 RID: 101
		[EnumMember(Value = "WindowsStore")]
		WindowsStore,
		// Token: 0x04000066 RID: 102
		[EnumMember(Value = "WindowsLegacy")]
		WindowsLegacy
	}
}
