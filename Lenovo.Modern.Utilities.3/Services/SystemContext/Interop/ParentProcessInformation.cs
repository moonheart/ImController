using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Interop
{
	// Token: 0x02000032 RID: 50
	public static class ParentProcessInformation
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000693C File Offset: 0x00004B3C
		public static string GetProcessExecutablePath(int processID)
		{
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT ExecutablePath, ProcessID FROM Win32_Process"))
				{
					if (managementObjectSearcher != null)
					{
						using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
						{
							foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
							{
								ManagementObject managementObject = (ManagementObject)managementBaseObject;
								object obj = managementObject["ProcessID"];
								object obj2 = managementObject["ExecutablePath"];
								if (obj2 != null && obj.ToString() == processID.ToString())
								{
									return obj2.ToString();
								}
							}
						}
					}
				}
			}
			catch
			{
				return string.Empty;
			}
			return string.Empty;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006A24 File Offset: 0x00004C24
		public static int GetParentProcessId(int Id)
		{
			try
			{
				ParentProcessInformation.PROCESSENTRY32 processentry = default(ParentProcessInformation.PROCESSENTRY32);
				processentry.dwSize = (uint)Marshal.SizeOf(typeof(ParentProcessInformation.PROCESSENTRY32));
				using (ParentProcessInformation.SafeSnapshotHandle safeSnapshotHandle = ParentProcessInformation.CreateToolhelp32Snapshot(ParentProcessInformation.SnapshotFlags.Process, (uint)Id))
				{
					if (safeSnapshotHandle.IsInvalid)
					{
						return -1;
					}
					if (!ParentProcessInformation.Process32First(safeSnapshotHandle, ref processentry) && Marshal.GetLastWin32Error() == 18)
					{
						return -1;
					}
					while (processentry.th32ProcessID != (uint)Id)
					{
						if (!ParentProcessInformation.Process32Next(safeSnapshotHandle, ref processentry))
						{
							goto IL_73;
						}
					}
					return (int)processentry.th32ParentProcessID;
				}
				IL_73:;
			}
			catch
			{
				return -1;
			}
			return -1;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006ACC File Offset: 0x00004CCC
		public static bool KillProcessAndChildren(int pid)
		{
			bool result = true;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid))
			{
				if (managementObjectSearcher != null)
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							ParentProcessInformation.KillProcessAndChildren(Convert.ToInt32(((ManagementObject)managementBaseObject)["ProcessID"]));
						}
						try
						{
							Process.GetProcessById(pid).Kill();
						}
						catch (Exception)
						{
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000142 RID: 322
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern ParentProcessInformation.SafeSnapshotHandle CreateToolhelp32Snapshot(ParentProcessInformation.SnapshotFlags flags, uint id);

		// Token: 0x06000143 RID: 323
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool Process32First(ParentProcessInformation.SafeSnapshotHandle hSnapshot, ref ParentProcessInformation.PROCESSENTRY32 lppe);

		// Token: 0x06000144 RID: 324
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool Process32Next(ParentProcessInformation.SafeSnapshotHandle hSnapshot, ref ParentProcessInformation.PROCESSENTRY32 lppe);

		// Token: 0x0400005C RID: 92
		private const int ERROR_NO_MORE_FILES = 18;

		// Token: 0x0200008B RID: 139
		[Flags]
		private enum SnapshotFlags : uint
		{
			// Token: 0x04000213 RID: 531
			HeapList = 1U,
			// Token: 0x04000214 RID: 532
			Process = 2U,
			// Token: 0x04000215 RID: 533
			Thread = 4U,
			// Token: 0x04000216 RID: 534
			Module = 8U,
			// Token: 0x04000217 RID: 535
			Module32 = 16U,
			// Token: 0x04000218 RID: 536
			All = 15U,
			// Token: 0x04000219 RID: 537
			Inherit = 2147483648U,
			// Token: 0x0400021A RID: 538
			NoHeaps = 1073741824U
		}

		// Token: 0x0200008C RID: 140
		private struct PROCESSENTRY32
		{
			// Token: 0x0400021B RID: 539
			public uint dwSize;

			// Token: 0x0400021C RID: 540
			public uint cntUsage;

			// Token: 0x0400021D RID: 541
			public uint th32ProcessID;

			// Token: 0x0400021E RID: 542
			public IntPtr th32DefaultHeapID;

			// Token: 0x0400021F RID: 543
			public uint th32ModuleID;

			// Token: 0x04000220 RID: 544
			public uint cntThreads;

			// Token: 0x04000221 RID: 545
			public uint th32ParentProcessID;

			// Token: 0x04000222 RID: 546
			public int pcPriClassBase;

			// Token: 0x04000223 RID: 547
			public uint dwFlags;

			// Token: 0x04000224 RID: 548
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szExeFile;
		}

		// Token: 0x0200008D RID: 141
		[SuppressUnmanagedCodeSecurity]
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		internal sealed class SafeSnapshotHandle : SafeHandleMinusOneIsInvalid
		{
			// Token: 0x06000204 RID: 516 RVA: 0x00009EEF File Offset: 0x000080EF
			internal SafeSnapshotHandle()
				: base(true)
			{
			}

			// Token: 0x06000205 RID: 517 RVA: 0x00009EF8 File Offset: 0x000080F8
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			internal SafeSnapshotHandle(IntPtr handle)
				: base(true)
			{
				base.SetHandle(handle);
			}

			// Token: 0x06000206 RID: 518 RVA: 0x00009F08 File Offset: 0x00008108
			protected override bool ReleaseHandle()
			{
				return ParentProcessInformation.SafeSnapshotHandle.CloseHandle(this.handle);
			}

			// Token: 0x06000207 RID: 519
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			private static extern bool CloseHandle(IntPtr handle);
		}
	}
}
