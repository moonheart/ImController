using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.ImController.EventLogging.Utilities
{
	// Token: 0x02000007 RID: 7
	internal static class XmlStringSerializer
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
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
					result = (T)((object)new XmlSerializer(typeof(T)).Deserialize(xmlReader));
				}
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000226C File Offset: 0x0000046C
		internal static string Serialize<T>(T instance)
		{
			string result = null;
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				OmitXmlDeclaration = false,
				NewLineHandling = NewLineHandling.Entitize,
				Encoding = new UTF8Encoding(false, false)
			};
			using (StringWriter stringWriter = new StringWriter())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
				{
					xmlSerializer.Serialize(xmlWriter, instance);
				}
				result = stringWriter.ToString();
			}
			return result;
		}
	}
}
