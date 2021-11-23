using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Lenovo.ImController.EventLogging.Model
{
	// Token: 0x02000016 RID: 22
	[DataContract(Name = "event", Namespace = "")]
	internal sealed class StorableEvent
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002C67 File Offset: 0x00000E67
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002C6F File Offset: 0x00000E6F
		[DataMember(Name = "name")]
		public string Name { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002C78 File Offset: 0x00000E78
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002C80 File Offset: 0x00000E80
		[IgnoreDataMember]
		public DataClassification Classification { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002C89 File Offset: 0x00000E89
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002C91 File Offset: 0x00000E91
		[DataMember(Name = "occuredDate")]
		public string TimeStampOfOccurance { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002C9A File Offset: 0x00000E9A
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002CA2 File Offset: 0x00000EA2
		[DataMember(Name = "collectedDate")]
		public string TimeStampOfCollection { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002CAB File Offset: 0x00000EAB
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002CB3 File Offset: 0x00000EB3
		[DataMember(Name = "variables")]
		internal List<StorableEventVariable> Variables { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002CBC File Offset: 0x00000EBC
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002CC4 File Offset: 0x00000EC4
		[DataMember(Name = "productVersion")]
		public string ProductVersion { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002CCD File Offset: 0x00000ECD
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002CD5 File Offset: 0x00000ED5
		[DataMember(Name = "productName")]
		public string ProductName { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002CDE File Offset: 0x00000EDE
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002CE6 File Offset: 0x00000EE6
		[DataMember(Name = "sid")]
		public string UserSID { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002CF0 File Offset: 0x00000EF0
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002D14 File Offset: 0x00000F14
		[DataMember(Name = "classification")]
		public string XmlClassificationDoNotUse
		{
			get
			{
				return this.Classification.ToString();
			}
			set
			{
				DataClassification classification = DataClassification.NotSpecified;
				Enum.TryParse<DataClassification>(value, out classification);
				this.Classification = classification;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002D33 File Offset: 0x00000F33
		public StorableEvent()
		{
			this.Variables = new List<StorableEventVariable>();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002D48 File Offset: 0x00000F48
		public static StorableEvent FromUserEvent(UserEvent userEvent)
		{
			StorableEvent storableEvent = new StorableEvent();
			storableEvent.ProductVersion = userEvent.ProductVersion;
			storableEvent.ProductName = userEvent.ProductName;
			storableEvent.Name = userEvent.Name;
			storableEvent.Classification = userEvent.Classification;
			storableEvent.UserSID = userEvent.UserSID;
			storableEvent.TimeStampOfCollection = userEvent.TimeStampOfCollection.ToString("yyyy-MM-ddTHH:mm:ss");
			storableEvent.TimeStampOfOccurance = userEvent.TimeStampOfOccurance.ToString("yyyy-MM-ddTHH:mm:ss");
			storableEvent.Variables = (from s in userEvent.Variables
				select StorableEventVariable.FromEventVariable(s)).ToList<StorableEventVariable>();
			return storableEvent;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002E00 File Offset: 0x00001000
		public static UserEvent ToUserEvent(StorableEvent userEvent)
		{
			UserEvent userEvent2 = new UserEvent();
			userEvent2.ProductName = userEvent.ProductName;
			userEvent2.ProductVersion = userEvent.ProductVersion;
			userEvent2.Name = userEvent.Name;
			userEvent2.Classification = userEvent.Classification;
			userEvent2.UserSID = userEvent.UserSID;
			userEvent2.TimeStampOfCollection = DateTime.ParseExact(userEvent.TimeStampOfCollection, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
			userEvent2.TimeStampOfOccurance = DateTime.ParseExact(userEvent.TimeStampOfOccurance, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
			userEvent2.Variables = (from u in userEvent.Variables
				select StorableEventVariable.ToEventVariable(u)).ToList<UserEventVariable>();
			return userEvent2;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002EB9 File Offset: 0x000010B9
		public override string ToString()
		{
			return this.ProductName + ":" + this.Name;
		}
	}
}
