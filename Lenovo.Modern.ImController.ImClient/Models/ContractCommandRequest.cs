using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000027 RID: 39
	public sealed class ContractCommandRequest
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00002344 File Offset: 0x00000544
		public ContractCommandRequest()
		{
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004A9E File Offset: 0x00002C9E
		public ContractCommandRequest(string commandName, string commandParameter)
		{
			if (string.IsNullOrWhiteSpace(commandName))
			{
				throw new ArgumentNullException("Must provide a non-empty command name");
			}
			this.Name = commandName;
			this.Parameter = commandParameter;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004AC7 File Offset: 0x00002CC7
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004ACF File Offset: 0x00002CCF
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004AD8 File Offset: 0x00002CD8
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004AE0 File Offset: 0x00002CE0
		[XmlAttribute(AttributeName = "requestType")]
		public string RequestType { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004AE9 File Offset: 0x00002CE9
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004AF1 File Offset: 0x00002CF1
		[XmlIgnore]
		public string Parameter { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004AFA File Offset: 0x00002CFA
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004B0C File Offset: 0x00002D0C
		[XmlElement(ElementName = "Parameter")]
		public XmlCDataSection ParameterCCDATA
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Parameter);
			}
			set
			{
				this.Parameter = value.Value;
			}
		}
	}
}
