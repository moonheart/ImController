using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C6 RID: 198
	[XmlRoot(ElementName = "UniversalAppLaunchDetails", Namespace = "")]
	public sealed class UniversalAppLaunchDetails
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00008518 File Offset: 0x00006718
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00008520 File Offset: 0x00006720
		[XmlAttribute(AttributeName = "packageFamilyName")]
		public string PackageFamilyName { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00008529 File Offset: 0x00006729
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00008531 File Offset: 0x00006731
		[XmlAttribute(AttributeName = "arguments")]
		public string Arguments { get; set; }
	}
}
