using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lenovo.Modern.CoreTypes.Contracts.Messaging;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.AppTagList;

namespace Lenovo.Modern.CoreTypes.Services
{
	// Token: 0x02000007 RID: 7
	public static class MessageDataExtractor
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000030AC File Offset: 0x000012AC
		public static Uri ExtractApplicableAction(LenovoMessage message, MachineInformation machineInformation, AppAndTagCollection appsAndTags)
		{
			Uri result = null;
			if (message != null && message.MessageActionList != null && message.MessageActionList.Any<MessageAction>())
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				if (machineInformation.PreloadTagList != null)
				{
					list.AddRange(from t in machineInformation.PreloadTagList
						select new KeyValuePair<string, string>(t, string.Empty));
				}
				if (appsAndTags != null)
				{
					list.AddRange(AppAndTagCollection.ExtractCollecitonOfKeys(appsAndTags));
				}
				IBasicEligabilityRequirements mostApplicableBasicEligibilityItem = EligibilityFilter.GetMostApplicableBasicEligibilityItem(message.MessageActionList, machineInformation, null, list);
				MessageAction item = mostApplicableBasicEligibilityItem as MessageAction;
				if (item != null && !string.IsNullOrWhiteSpace(item.Command))
				{
					if (item.Type == MessageActionType.InstalledItem)
					{
						if (appsAndTags == null || appsAndTags.InstalledApps == null)
						{
							throw new Exception("Action type is InstalledItem but no appsAndTags/installedItems are available");
						}
						InstalledApp installedApp = appsAndTags.InstalledApps.FirstOrDefault((InstalledApp installedItem) => installedItem.Key != null && installedItem.Key.Equals(item.Command));
						if (installedApp == null)
						{
							throw new Exception(string.Format("Locating applicable action. Unable to find expected InstalledITem {0}", item.Command));
						}
						result = new Uri(installedApp.Protocol);
					}
					else if (Uri.IsWellFormedUriString(item.Command, UriKind.Absolute))
					{
						result = new Uri(item.Command);
					}
				}
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000031FC File Offset: 0x000013FC
		public static ApplicableBindings ExtractApplicableBindingValues(string contentXml, string locale)
		{
			ApplicableBindings applicableBindings = null;
			if (!string.IsNullOrWhiteSpace(contentXml))
			{
				XDocument xdocument = XDocument.Parse(contentXml);
				if (xdocument != null)
				{
					Func<XElement, string, bool> findCorrectBinding = delegate(XElement binding, string requiredLocal)
					{
						XAttribute xattribute = binding.Attribute("lang");
						bool result;
						if (string.IsNullOrWhiteSpace(requiredLocal))
						{
							result = xattribute == null || xattribute.Value == string.Empty;
						}
						else
						{
							result = xattribute != null && xattribute.Value == requiredLocal;
						}
						return result;
					};
					IEnumerable<XElement> source = from b in xdocument.Descendants("binding")
						where findCorrectBinding(b, locale)
						select b;
					if (!source.Any<XElement>() && !string.IsNullOrWhiteSpace(locale))
					{
						source = from b in xdocument.Descendants("binding")
							where findCorrectBinding(b, null)
							select b;
						if (!source.Any<XElement>() && !string.IsNullOrWhiteSpace(locale))
						{
							source = from b in xdocument.Descendants("binding")
								where findCorrectBinding(b, "en")
								select b;
						}
					}
					if (source.Any<XElement>())
					{
						List<Uri> imageValues = (from i in source.Descendants("image").Select(delegate(XElement e)
							{
								Uri result = null;
								if (e.HasAttributes && e.Attribute("src") != null)
								{
									result = new Uri(e.Attribute("src").Value);
								}
								return result;
							})
							where i != null
							select i).ToList<Uri>();
						List<string> textValues = (from e in source.Descendants("text")
							select e.Value into i
							where i != null
							select i).ToList<string>();
						applicableBindings = new ApplicableBindings();
						applicableBindings.ImageValues = imageValues;
						applicableBindings.TextValues = textValues;
					}
				}
			}
			return applicableBindings;
		}
	}
}
