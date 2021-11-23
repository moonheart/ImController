using System;
using System.Runtime.Serialization;

namespace Lenovo.ImController.EventLogging.Model
{
	// Token: 0x02000017 RID: 23
	[DataContract(Name = "var", Namespace = "")]
	internal sealed class StorableEventVariable
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00002500 File Offset: 0x00000700
		public StorableEventVariable()
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002ED1 File Offset: 0x000010D1
		public StorableEventVariable(string name, string value)
		{
			this.Value = value;
			this.Name = name;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002EE7 File Offset: 0x000010E7
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002EEF File Offset: 0x000010EF
		[DataMember(Name = "name")]
		public string Name { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002EF8 File Offset: 0x000010F8
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002F00 File Offset: 0x00001100
		[DataMember(Name = "value")]
		public string Value { get; set; }

		// Token: 0x06000068 RID: 104 RVA: 0x00002F09 File Offset: 0x00001109
		public static StorableEventVariable FromEventVariable(UserEventVariable eventVariable)
		{
			return new StorableEventVariable(eventVariable.Name, eventVariable.Value);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002F1C File Offset: 0x0000111C
		public static UserEventVariable ToEventVariable(StorableEventVariable eventVariable)
		{
			return new UserEventVariable(eventVariable.Name, eventVariable.Value);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002F2F File Offset: 0x0000112F
		public override string ToString()
		{
			return this.Name + "::" + this.Value;
		}
	}
}
