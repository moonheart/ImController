using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Lenovo.ImController.EventLogging.Utilities
{
	// Token: 0x02000006 RID: 6
	public static class DataContractStringSerializer
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002090 File Offset: 0x00000290
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

		// Token: 0x06000005 RID: 5 RVA: 0x00002124 File Offset: 0x00000324
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
