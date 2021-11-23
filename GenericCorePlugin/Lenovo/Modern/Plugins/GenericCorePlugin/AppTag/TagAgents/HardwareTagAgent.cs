using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Windows.Devices.Enumeration;
using Windows.Devices.Power;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000020 RID: 32
	internal class HardwareTagAgent : ITagAgent
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x00008108 File Offset: 0x00006308
		public async Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			TaskAwaiter<bool> taskAwaiter = this.IsCameraInstalled().GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				list.Add(new Tag
				{
					Key = "System.Camera.Installed"
				});
			}
			taskAwaiter = this.IsBatteryInstalled().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				list.Add(new Tag
				{
					Key = "System.Battery.Installed"
				});
			}
			taskAwaiter = this.IsMicrophoneInstalled().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				list.Add(new Tag
				{
					Key = "System.Microphone.Installed"
				});
			}
			return list;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008150 File Offset: 0x00006350
		private async Task<bool> IsCameraInstalled()
		{
			bool isCameraInstalled = false;
			try
			{
				DeviceInformationCollection deviceInformationCollection = await DeviceInformation.FindAllAsync(4);
				if (deviceInformationCollection != null && deviceInformationCollection.Count >= 1)
				{
					isCameraInstalled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "HardwareTagAgent: Error while checking the camera");
			}
			return isCameraInstalled;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00008190 File Offset: 0x00006390
		private async Task<bool> IsBatteryInstalled()
		{
			bool isBatteryInstalled = false;
			try
			{
				DeviceInformationCollection deviceInformationCollection = await DeviceInformation.FindAllAsync(Battery.GetDeviceSelector());
				if (deviceInformationCollection != null && deviceInformationCollection.Count >= 1)
				{
					isBatteryInstalled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "HardwareTagAgent: Error while checking the battery");
			}
			return isBatteryInstalled;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000081D0 File Offset: 0x000063D0
		private async Task<bool> IsMicrophoneInstalled()
		{
			bool isMicrophoneInstalled = false;
			try
			{
				DeviceInformationCollection deviceInformationCollection = await DeviceInformation.FindAllAsync(1);
				if (deviceInformationCollection != null && deviceInformationCollection.Count >= 1)
				{
					isMicrophoneInstalled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "HardwareTagAgent: Error while checking the microphone");
			}
			return isMicrophoneInstalled;
		}
	}
}
