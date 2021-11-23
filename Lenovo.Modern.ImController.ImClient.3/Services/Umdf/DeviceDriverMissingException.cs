using System;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x0200001C RID: 28
	public class DeviceDriverMissingException : Exception
	{
		// Token: 0x06000085 RID: 133 RVA: 0x0000341A File Offset: 0x0000161A
		public DeviceDriverMissingException(string message)
			: base(message)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003B13 File Offset: 0x00001D13
		public DeviceDriverMissingException()
			: base("An error occured with the Device Driver)")
		{
		}
	}
}
