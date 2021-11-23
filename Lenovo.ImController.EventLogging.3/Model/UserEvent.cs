using System;
using System.Collections.Generic;

namespace Lenovo.ImController.EventLogging.Model
{
	// Token: 0x02000014 RID: 20
	public sealed class UserEvent
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002A4C File Offset: 0x00000C4C
		public UserEvent(string eventName, DataClassification classification, DateTime timeOfEventOccurance)
		{
			this.Name = eventName;
			this.TimeStampOfCollection = DateTime.Now;
			this.TimeStampOfOccurance = timeOfEventOccurance;
			this.Variables = new List<UserEventVariable>();
			this.Classification = classification;
			this.ProductName = "NotSet";
			this.ProductVersion = "NotSet";
			this.UserSID = "NotSet";
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AAB File Offset: 0x00000CAB
		public UserEvent(string eventName, DataClassification classification)
			: this(eventName, classification, DateTime.Now)
		{
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002ABA File Offset: 0x00000CBA
		public UserEvent()
		{
			this.Classification = DataClassification.NotSpecified;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002AC9 File Offset: 0x00000CC9
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public string Name { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002ADA File Offset: 0x00000CDA
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002AE2 File Offset: 0x00000CE2
		public DataClassification Classification { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002AEB File Offset: 0x00000CEB
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002AF3 File Offset: 0x00000CF3
		public DateTime TimeStampOfOccurance { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002AFC File Offset: 0x00000CFC
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002B04 File Offset: 0x00000D04
		public DateTime TimeStampOfCollection { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002B0D File Offset: 0x00000D0D
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002B15 File Offset: 0x00000D15
		public List<UserEventVariable> Variables { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002B1E File Offset: 0x00000D1E
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002B26 File Offset: 0x00000D26
		public string UserSID { get; internal set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002B2F File Offset: 0x00000D2F
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002B37 File Offset: 0x00000D37
		public string ProductVersion { get; internal set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002B40 File Offset: 0x00000D40
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002B48 File Offset: 0x00000D48
		public string ProductName { get; internal set; }

		// Token: 0x06000040 RID: 64 RVA: 0x00002B54 File Offset: 0x00000D54
		public static void AssertValidity(UserEvent userEvent)
		{
			if (userEvent == null)
			{
				throw new ArgumentNullException("Event cannot be null");
			}
			if (string.IsNullOrWhiteSpace(userEvent.Name))
			{
				throw new InvalidOperationException("Event Id is required");
			}
			DateTime timeStampOfCollection = userEvent.TimeStampOfCollection;
			DateTime timeStampOfCollection2 = userEvent.TimeStampOfCollection;
			if (string.IsNullOrWhiteSpace(userEvent.ProductName))
			{
				throw new InvalidOperationException("Event Product Name is required");
			}
			if (userEvent.Variables == null)
			{
				throw new InvalidOperationException("Event variable list cannot be null");
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public override string ToString()
		{
			return this.ProductName + ":" + this.Name;
		}

		// Token: 0x02000023 RID: 35
		private static class UserDataConstants
		{
			// Token: 0x04000067 RID: 103
			public const string GuidEmpty = "00000000-0000-0000-0000-000000000000";
		}
	}
}
