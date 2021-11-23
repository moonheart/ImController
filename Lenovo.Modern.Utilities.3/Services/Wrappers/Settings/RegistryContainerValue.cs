using System;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Settings
{
	// Token: 0x02000017 RID: 23
	public class RegistryContainerValue : IContainerValue
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000327F File Offset: 0x0000147F
		internal RegistryContainerValue(string name, string value)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				this._name = name;
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				this._value = value;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000032A5 File Offset: 0x000014A5
		public string GetName()
		{
			return this._name;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000032AD File Offset: 0x000014AD
		public string GetValueAsString()
		{
			return this._value;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000032B8 File Offset: 0x000014B8
		public int? GetValueAsInt()
		{
			int? result = null;
			int value;
			if (int.TryParse(this._value.Trim(), out value))
			{
				result = new int?(value);
			}
			return result;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000032EC File Offset: 0x000014EC
		public bool? GetValueAsBool()
		{
			string text = this._value.Trim();
			bool? result = null;
			bool value;
			if (bool.TryParse(text, out value))
			{
				result = new bool?(value);
			}
			else if (text == "0")
			{
				result = new bool?(false);
			}
			else if (text == "1")
			{
				result = new bool?(true);
			}
			return result;
		}

		// Token: 0x04000017 RID: 23
		private readonly string _name;

		// Token: 0x04000018 RID: 24
		private readonly string _value;
	}
}
