using System;
using System.Collections.Generic;
using System.Text;

namespace Lenovo.Modern.ImController.PluginHost.Utilities
{
	// Token: 0x02000004 RID: 4
	public class InputArguments
	{
		// Token: 0x17000001 RID: 1
		public string this[string key]
		{
			get
			{
				return this.GetValue(key);
			}
			set
			{
				if (key != null)
				{
					this._parsedArguments[key] = value;
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000023C3 File Offset: 0x000005C3
		public string KeyLeadingPattern
		{
			get
			{
				return this._keyLeadingPattern;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023CB File Offset: 0x000005CB
		public InputArguments(string[] args, string keyLeadingPattern)
		{
			this._keyLeadingPattern = ((!string.IsNullOrEmpty(keyLeadingPattern)) ? keyLeadingPattern : "-");
			if (args != null && args.Length != 0)
			{
				this.Parse(args);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002407 File Offset: 0x00000607
		public InputArguments(string[] args)
			: this(args, null)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002414 File Offset: 0x00000614
		public InputArguments(string cl)
		{
			string[] array = cl.Split(new char[] { ' ' });
			foreach (string text in new string[] { "name", "pluginName", "pluginVersion", "runas" })
			{
				int num = 0;
				string text2 = "-" + text;
				while (num < array.Length && !text2.Equals(array[num]))
				{
					num++;
				}
				if (array.Length >= num + 2)
				{
					this._parsedArguments[text] = array[num + 1];
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000024CC File Offset: 0x000006CC
		public bool Contains(string key)
		{
			string text;
			return this.ContainsKey(key, out text);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024E2 File Offset: 0x000006E2
		public int Count()
		{
			if (this._parsedArguments != null)
			{
				return this._parsedArguments.Count;
			}
			return 0;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual string GetPeeledKey(string key)
		{
			if (!this.IsKey(key))
			{
				return key;
			}
			return key.Substring(this._keyLeadingPattern.Length);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002517 File Offset: 0x00000717
		public virtual string GetDecoratedKey(string key)
		{
			if (this.IsKey(key))
			{
				return key;
			}
			return this._keyLeadingPattern + key;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002530 File Offset: 0x00000730
		public virtual bool IsKey(string str)
		{
			return str.StartsWith(this._keyLeadingPattern);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002540 File Offset: 0x00000740
		public string Flatten()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._parsedArguments != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this._parsedArguments)
				{
					stringBuilder.AppendFormat("{0} {1} ", keyValuePair.Key, keyValuePair.Value);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025BC File Offset: 0x000007BC
		protected virtual void Parse(string[] args)
		{
			bool flag = false;
			for (int i = 0; i < args.Length; i++)
			{
				if (flag)
				{
					flag = false;
				}
				else if (args[i] != null)
				{
					string text = null;
					string text2 = null;
					if (this.IsKey(args[i]))
					{
						text = args[i];
						if (i + 1 < args.Length && !this.IsKey(args[i + 1]))
						{
							text2 = args[i + 1];
							flag = true;
						}
					}
					else
					{
						text2 = args[i];
					}
					if (text == null)
					{
						text = text2;
						text2 = null;
					}
					this._parsedArguments[text] = text2;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002630 File Offset: 0x00000830
		protected virtual string GetValue(string key)
		{
			string key2;
			if (this.ContainsKey(key, out key2))
			{
				return this._parsedArguments[key2];
			}
			return null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002658 File Offset: 0x00000858
		protected virtual bool ContainsKey(string key, out string adjustedKey)
		{
			adjustedKey = key;
			if (this._parsedArguments.ContainsKey(key))
			{
				return true;
			}
			if (this.IsKey(key))
			{
				string peeledKey = this.GetPeeledKey(key);
				if (this._parsedArguments.ContainsKey(peeledKey))
				{
					adjustedKey = peeledKey;
					return true;
				}
				return false;
			}
			else
			{
				string decoratedKey = this.GetDecoratedKey(key);
				if (this._parsedArguments.ContainsKey(decoratedKey))
				{
					adjustedKey = decoratedKey;
					return true;
				}
				return false;
			}
		}

		// Token: 0x04000003 RID: 3
		public const string DEFAULT_KEY_LEADING_PATTERN = "-";

		// Token: 0x04000004 RID: 4
		protected Dictionary<string, string> _parsedArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000005 RID: 5
		protected readonly string _keyLeadingPattern;
	}
}
