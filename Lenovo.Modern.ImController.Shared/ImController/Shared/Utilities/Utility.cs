using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000036 RID: 54
	public class Utility
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00008044 File Offset: 0x00006244
		public static bool SanitizePath(ref string sourcePath)
		{
			if (string.IsNullOrEmpty(sourcePath) || sourcePath.Length > Utility.MAXFILEPATHLENGTH)
			{
				return false;
			}
			if (sourcePath.Contains("\\.") || sourcePath.Contains("\\.."))
			{
				return false;
			}
			string str = new string(Path.GetInvalidPathChars());
			return !new Regex("[" + Regex.Escape(str) + "]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant).IsMatch(sourcePath);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000080BC File Offset: 0x000062BC
		public static string SanitizePath1(string sourcePath)
		{
			string result = "";
			if (string.IsNullOrEmpty(sourcePath) || sourcePath.Length > Utility.MAXFILEPATHLENGTH)
			{
				return result;
			}
			if (sourcePath.Contains("\\.") || sourcePath.Contains("\\.."))
			{
				return result;
			}
			string str = new string(Path.GetInvalidPathChars());
			return (!new Regex("[" + Regex.Escape(str) + "]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant).IsMatch(sourcePath)) ? "" : sourcePath;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008140 File Offset: 0x00006340
		public static bool SanitizeFileName(ref string fileName)
		{
			if (string.IsNullOrEmpty(fileName) || fileName.Length > Utility.MAXARGSLENGTH)
			{
				return false;
			}
			string str = new string(Path.GetInvalidFileNameChars());
			return !new Regex("[" + Regex.Escape(str) + "]", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant).IsMatch(fileName);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000819A File Offset: 0x0000639A
		public static bool SanitizeArg(ref string argument)
		{
			return !string.IsNullOrEmpty(argument) && argument.Length <= Utility.MAXARGSLENGTH && new Regex("^(-|/)?[\\w\\d]+[^ \\s\\t]$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant).IsMatch(argument);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000081CB File Offset: 0x000063CB
		public static bool SanitizeString(ref string inputString)
		{
			if (string.IsNullOrEmpty(inputString) || inputString.Length > Utility.MAXARGSLENGTH)
			{
				return false;
			}
			Regex regex = new Regex("^[\\w\\d]+[^ \\s\\t]$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
			regex.Match(inputString);
			return regex.IsMatch(inputString);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008208 File Offset: 0x00006408
		public static bool SanitizeVersion(ref string inputVersion)
		{
			bool result = false;
			if (string.IsNullOrEmpty(inputVersion) || inputVersion.Length > Utility.MAXARGSLENGTH)
			{
				return false;
			}
			Version v = new Version();
			if (Version.TryParse(inputVersion, out v))
			{
				result = null != v;
			}
			return result;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000824A File Offset: 0x0000644A
		public static bool Sanitize(ref string inputString)
		{
			return !string.IsNullOrEmpty(inputString) && inputString.Length <= Utility.MAXARGSLENGTH;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008266 File Offset: 0x00006466
		public static string Sanitize1(string inputString)
		{
			if (string.IsNullOrEmpty(inputString) || inputString.Length > Utility.MAXARGSLENGTH)
			{
				return "";
			}
			return inputString;
		}

		// Token: 0x040000E2 RID: 226
		private static readonly int MAXFILEPATHLENGTH = 2048;

		// Token: 0x040000E3 RID: 227
		private static readonly int MAXARGSLENGTH = 252;
	}
}
