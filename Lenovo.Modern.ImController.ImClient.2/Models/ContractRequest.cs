using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000026 RID: 38
	[XmlRoot(ElementName = "ContractRequest", Namespace = null)]
	public sealed class ContractRequest
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00002344 File Offset: 0x00000544
		public ContractRequest()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000049F6 File Offset: 0x00002BF6
		public ContractRequest(string contractName, string commandName)
			: this(contractName, commandName, string.Empty)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004A05 File Offset: 0x00002C05
		public ContractRequest(string contractName, string commandName, string commandParameter)
		{
			if (string.IsNullOrWhiteSpace(contractName))
			{
				throw new ArgumentNullException("Must provide a non-empty contract name");
			}
			this.Name = contractName;
			this.Command = new ContractCommandRequest(commandName, commandParameter);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004A34 File Offset: 0x00002C34
		public ContractRequest(string contractName, string pluginName, string commandName, string commandParameter)
		{
			if (string.IsNullOrWhiteSpace(contractName))
			{
				throw new ArgumentNullException("Must provide a non-empty contract name");
			}
			this.Name = contractName;
			this.Command = new ContractCommandRequest(commandName, commandParameter);
			this.PluginName = pluginName;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004A6B File Offset: 0x00002C6B
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004A73 File Offset: 0x00002C73
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004A7C File Offset: 0x00002C7C
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004A84 File Offset: 0x00002C84
		[XmlElement(ElementName = "Command")]
		public ContractCommandRequest Command { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004A8D File Offset: 0x00002C8D
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004A95 File Offset: 0x00002C95
		[XmlAttribute(AttributeName = "pluginName")]
		public string PluginName { get; set; }
	}
}
