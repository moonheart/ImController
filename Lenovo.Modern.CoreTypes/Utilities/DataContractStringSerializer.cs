using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Lenovo.Modern.CoreTypes.Utilities
{
	// Token: 0x02000004 RID: 4
	public static class DataContractStringSerializer
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000214C File Offset: 0x0000034C
		public static T Deserialize<T>(string xml)
		{
			T result = default(T);
			XmlReaderSettings settings = new XmlReaderSettings
			{
				DtdProcessing = DtdProcessing.Ignore,
				XmlResolver = null,
				CheckCharacters = false,
				IgnoreWhitespace = true
			};
			using (StringReader stringReader = new StringReader(xml))
			{
				using (XmlReader xmlReader = XmlReader.Create(stringReader, settings))
				{
					result = (T)((object)new DataContractSerializer(typeof(T)).ReadObject(xmlReader));
				}
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021E0 File Offset: 0x000003E0
		public static string Serialize<T>(T instance)
		{
			string result = null;
			DataContractSerializerSettings settings = new DataContractSerializerSettings
			{
				PreserveObjectReferences = false
			};
			XmlWriterSettings settings2 = new XmlWriterSettings
			{
				Indent = true,
				OmitXmlDeclaration = false,
				NewLineHandling = NewLineHandling.Entitize,
				Encoding = new UTF8Encoding(false, false)
			};
			using (StringWriter stringWriter = new StringWriter())
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T), settings);
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings2))
				{
					dataContractSerializer.WriteObject(xmlWriter, instance);
				}
				result = stringWriter.ToString();
			}
			return result;
		}
	}
}
