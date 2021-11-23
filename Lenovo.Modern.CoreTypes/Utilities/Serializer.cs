using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Utilities
{
	// Token: 0x02000003 RID: 3
	public static class Serializer
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public static T Deserialize<T>(string xml)
		{
			return XmlStringSerializer.Deserialize<T>(xml);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000206E File Offset: 0x0000026E
		public static string Serialize<T>(T instance)
		{
			return XmlStringSerializer.Serialize<T>(instance);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
		public static string GetXmlEnumAttributeValueFromEnum<TEnum>(TEnum value) where TEnum : struct, IConvertible
		{
			Type typeFromHandle = typeof(TEnum);
			if (!typeFromHandle.IsEnum)
			{
				return null;
			}
			MemberInfo memberInfo = typeFromHandle.GetMember(value.ToString()).FirstOrDefault<MemberInfo>();
			if (memberInfo == null)
			{
				return null;
			}
			XmlEnumAttribute xmlEnumAttribute = memberInfo.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault<XmlEnumAttribute>();
			if (xmlEnumAttribute == null)
			{
				return null;
			}
			return xmlEnumAttribute.Name;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020DC File Offset: 0x000002DC
		public static bool GetXmlEnumValueFromString<TEnum>(string value, ref TEnum result) where TEnum : struct, IConvertible
		{
			bool result2 = false;
			Type typeFromHandle = typeof(TEnum);
			if (!typeFromHandle.IsEnum)
			{
				return result2;
			}
			FieldInfo fieldInfo = typeFromHandle.GetFields().FirstOrDefault(delegate(FieldInfo field)
			{
				XmlEnumAttribute xmlEnumAttribute = field.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault<XmlEnumAttribute>();
				return xmlEnumAttribute != null && !string.IsNullOrWhiteSpace(xmlEnumAttribute.Name) && xmlEnumAttribute.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase);
			});
			if (fieldInfo == null || string.IsNullOrWhiteSpace(fieldInfo.Name))
			{
				return result2;
			}
			return Enum.TryParse<TEnum>(value, true, out result);
		}
	}
}
