using System;
using System.Collections.Generic;
using System.Linq;
using Lenovo.Modern.CoreTypes.Contracts.Messaging;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.AppTagList;

namespace Lenovo.Modern.CoreTypes.Services
{
	// Token: 0x02000006 RID: 6
	public static class EligibilityFilter
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000023CC File Offset: 0x000005CC
		public static IBasicEligabilityRequirements GetFirstApplicableMatch(IEnumerable<IBasicEligabilityRequirements> filterList, MachineInformation machineInformation, AppAndTagCollection appTagList, string productVersion)
		{
			string currentVersion = productVersion ?? "0.0.0.0";
			if (filterList == null)
			{
				throw new ArgumentNullException("filterList");
			}
			if (machineInformation == null)
			{
				throw new ArgumentNullException("machineInformation");
			}
			List<KeyValuePair<string, string>> mergedTagList = new List<KeyValuePair<string, string>>();
			if (machineInformation.PreloadTagList != null)
			{
				mergedTagList.AddRange(from t in machineInformation.PreloadTagList
					select new KeyValuePair<string, string>(t, string.Empty));
			}
			if (appTagList != null)
			{
				mergedTagList.AddRange(AppAndTagCollection.ExtractCollecitonOfKeys(appTagList));
			}
			return filterList.FirstOrDefault((IBasicEligabilityRequirements x) => EligibilityFilter.IsApplicableBasic(x, machineInformation, currentVersion, mergedTagList));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002490 File Offset: 0x00000690
		public static bool IsApplicableAdvanced(IAdvancedApplicabilityFilter filter, MachineInformation machineInformation, AppAndTagCollection appTagList)
		{
			if (filter != null)
			{
				bool flag = true;
				if (flag && filter.BrandList != null && filter.BrandList.Any<NegatableBrand>())
				{
					filter.BrandList.ToList<NegatableBrand>().RemoveAll((NegatableBrand x) => x.Value.Equals(BrandType.None));
					flag = EligibilityFilter.FilterTargetWithNegatableList((from source in filter.BrandList
						select new NegatableString(source.Exclude, source.Value.ToString())).AsEnumerable<NegatableString>(), machineInformation.Brand.ToString(), EligibilityFilter.ComparrisonMethods.CaseInsensitiveWithWildcard);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to brand filter");
					}
				}
				if (flag && filter.SubBrandList != null && filter.SubBrandList.Any<NegatableString>())
				{
					flag = EligibilityFilter.FilterTargetWithNegatableList(filter.SubBrandList.AsEnumerable<NegatableString>(), machineInformation.SubBrand, EligibilityFilter.ComparrisonMethods.CaseInsensitiveWithWildcard);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to sub-brand filter");
					}
				}
				if (flag && filter.CountryList != null && filter.CountryList.Any<NegatableString>())
				{
					flag = EligibilityFilter.FilterTargetWithNegatableList(filter.CountryList.AsEnumerable<NegatableString>(), machineInformation.CountryCode, EligibilityFilter.ComparrisonMethods.CaseInsensitive);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to country filter");
					}
				}
				if (flag && filter.FamilyList != null && filter.FamilyList.Any<NegatableString>())
				{
					flag = EligibilityFilter.FilterTargetWithNegatableList(filter.FamilyList.AsEnumerable<NegatableString>(), machineInformation.Family, EligibilityFilter.ComparrisonMethods.CaseInsensitiveWithWildcard);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to Family filter");
					}
				}
				if (flag && filter.LangList != null && filter.LangList.Any<NegatableString>())
				{
					flag = EligibilityFilter.FilterTargetWithNegatableList(filter.LangList.AsEnumerable<NegatableString>(), machineInformation.Locale, EligibilityFilter.ComparrisonMethods.CaseInsensitive);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to Language filter");
					}
				}
				if (flag && filter.MtmList != null && filter.MtmList.Any<NegatableString>())
				{
					flag = EligibilityFilter.FilterTargetWithNegatableList(filter.MtmList.AsEnumerable<NegatableString>(), machineInformation.MTM, EligibilityFilter.ComparrisonMethods.CaseInsensitiveWithWildcard);
					if (!flag)
					{
						throw new NotSupportedException("Item not eligibile due to MTM filter");
					}
				}
				if (flag && filter.TagList != null && filter.TagList.Any<NegatableKeyValuePair>())
				{
					List<KeyValuePair<string, string>> mergedTagList = new List<KeyValuePair<string, string>>();
					if (machineInformation.PreloadTagList != null)
					{
						mergedTagList.AddRange(from t in machineInformation.PreloadTagList
							select new KeyValuePair<string, string>(t, string.Empty));
					}
					if (appTagList != null)
					{
						mergedTagList.AddRange(AppAndTagCollection.ExtractCollecitonOfKeys(appTagList));
					}
					IEnumerable<NegatableKeyValuePair> enumerable = from f in filter.TagList
						where f.Exclude.Equals(false)
						select f;
					if ((enumerable != null && enumerable.Any<NegatableKeyValuePair>() && mergedTagList == null) || mergedTagList.Count == 0)
					{
						flag = false;
						if (!flag)
						{
							throw new NotSupportedException("Item not eligibile due to empty (but required) tag filter");
						}
					}
					else
					{
						IEnumerable<NegatableKeyValuePair> matchingFilterKeys = (from requiredFilterItem in filter.TagList
							where mergedTagList.Any(delegate(KeyValuePair<string, string> actualItem)
							{
								bool flag2 = actualItem.Key != null && actualItem.Key.Equals(requiredFilterItem.Key, StringComparison.OrdinalIgnoreCase);
								if (flag2 && !string.IsNullOrWhiteSpace(requiredFilterItem.Value))
								{
									flag2 = EligibilityFilter.DoesTextMatchWithWildcard(requiredFilterItem.Value, actualItem.Value);
								}
								return flag2;
							})
							select requiredFilterItem).ToList<NegatableKeyValuePair>();
						IEnumerable<NegatableKeyValuePair> enumerable2 = from kvp in matchingFilterKeys
							where kvp.Exclude
							select kvp;
						List<NegatableKeyValuePair> list = filter.TagList.Where(delegate(NegatableKeyValuePair requiredKvp)
						{
							bool result = false;
							if (!requiredKvp.Exclude && !matchingFilterKeys.Any((NegatableKeyValuePair kvp) => kvp.Key != null && kvp.Key.Equals(requiredKvp.Key, StringComparison.OrdinalIgnoreCase) && !kvp.Exclude))
							{
								result = true;
							}
							return result;
						}).ToList<NegatableKeyValuePair>();
						if ((enumerable2 != null && enumerable2.Any<NegatableKeyValuePair>()) || (list != null && list.Any<NegatableKeyValuePair>()))
						{
							throw new NotSupportedException(string.Format("Item not eligibile due to tag filter mismatch.  matchingBlockingItems={0}, unmatchedRequirements={1}", string.Join("', '", from ns in list
								select ns.Key), string.Join("', '", from ns in list
								select ns.Key)));
						}
						flag = true;
					}
				}
				return flag;
			}
			throw new NotSupportedException("Item not eligible because filter is null");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002894 File Offset: 0x00000A94
		public static bool IsApplicableBasic(IBasicEligabilityRequirements requirements, MachineInformation machineInformation, string appVersion, IList<KeyValuePair<string, string>> tagList)
		{
			bool result = false;
			if (requirements != null && machineInformation != null)
			{
				result = EligibilityFilter.GetRequirementScore(requirements, machineInformation, appVersion, tagList).Item1;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028BC File Offset: 0x00000ABC
		public static IBasicEligabilityRequirements GetMostApplicableBasicEligibilityItem(IEnumerable<IBasicEligabilityRequirements> listOfItems, MachineInformation machineInformation, string appVersion, IList<KeyValuePair<string, string>> tagList)
		{
			IBasicEligabilityRequirements result = null;
			if (listOfItems != null && listOfItems.Any<IBasicEligabilityRequirements>() && machineInformation != null)
			{
				result = (from item in listOfItems
					where EligibilityFilter.GetRequirementScore(item, machineInformation, appVersion, tagList).Item1
					orderby EligibilityFilter.GetRequirementScore(item, machineInformation, appVersion, tagList).Item2 descending
					select item).FirstOrDefault<IBasicEligabilityRequirements>();
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002924 File Offset: 0x00000B24
		internal static Tuple<bool, int> GetRequirementScore(IBasicEligabilityRequirements requirements, MachineInformation info, string appVersion, IList<KeyValuePair<string, string>> tagList)
		{
			int num = 0;
			bool flag = true;
			if (!string.IsNullOrWhiteSpace(requirements.Manufacturer))
			{
				if (requirements.Manufacturer.Equals(info.Manufacturer, StringComparison.OrdinalIgnoreCase))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && requirements.Brand != BrandType.None)
			{
				if (requirements.Brand.Equals(info.Brand))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.Country))
			{
				if (requirements.Country.Equals(info.CountryCode, StringComparison.OrdinalIgnoreCase))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.SubBrand))
			{
				if (EligibilityFilter.DoesTextMatchWithWildcard(requirements.SubBrand, info.SubBrand))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.Family))
			{
				if (EligibilityFilter.DoesTextMatchWithWildcard(requirements.Family, info.Family))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.Mtm))
			{
				if (EligibilityFilter.DoesTextMatchWithWildcard(requirements.Mtm, info.MTM))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.Language))
			{
				if (requirements.Language.Equals(info.Locale, StringComparison.OrdinalIgnoreCase))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && requirements.EnclosureType != EnclosureType.None)
			{
				if (requirements.EnclosureType.Equals(info.Enclosure))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && requirements.AppVersion != null)
			{
				if (EligibilityFilter.DoesTextVersionMatch(requirements.AppVersion, appVersion))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.OsVersion))
			{
				if (EligibilityFilter.DoesTextVersionMatch(requirements.OsVersion, info.OperatingSystemVerion))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.OsBitness))
			{
				if (requirements.OsBitness.Equals(info.OperatingSystemBitness, StringComparison.OrdinalIgnoreCase))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && !string.IsNullOrWhiteSpace(requirements.Tag))
			{
				if (EligibilityFilter.DoesTagMatch(requirements.Tag, tagList))
				{
					num++;
				}
				else
				{
					flag = false;
				}
			}
			return new Tuple<bool, int>(flag, num);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002B44 File Offset: 0x00000D44
		private static bool FilterTargetWithNegatableList(IEnumerable<NegatableString> filterList, string target, Func<string, string, bool> compare)
		{
			bool result = true;
			NegatableString negatableString = filterList.FirstOrDefault((NegatableString x) => x.Exclude.Equals(false) && compare(target, x.Value));
			IEnumerable<NegatableString> source = from x in filterList
				where x.Exclude.Equals(false) && !compare(target, x.Value)
				select x;
			IEnumerable<NegatableString> source2 = from x in filterList
				where x.Exclude.Equals(true) && compare(target, x.Value)
				select x;
			if ((source.Any<NegatableString>() && negatableString == null) || source2.Any<NegatableString>())
			{
				result = false;
			}
			else if (negatableString != null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002BBB File Offset: 0x00000DBB
		private static bool FilterStringRequirement(string target, string requirement, Func<string, string, bool> compare)
		{
			return string.IsNullOrWhiteSpace(requirement) || compare(target, requirement);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public static bool DoesTagMatch(string tagRequirement, IList<KeyValuePair<string, string>> actualTags)
		{
			bool flag = true;
			if (actualTags == null || !actualTags.Any<KeyValuePair<string, string>>())
			{
				return false;
			}
			string[] array = tagRequirement.Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (!flag)
				{
					break;
				}
				string requiredTagKey = text;
				string requiredTagValue = null;
				if (text.Contains('|'))
				{
					string[] array2 = text.Split(new char[] { '|' });
					if (array2.Length > 1)
					{
						requiredTagKey = array2[0];
						requiredTagValue = array2[1];
					}
				}
				bool flag2 = requiredTagKey.StartsWith("!", StringComparison.OrdinalIgnoreCase);
				if (flag2)
				{
					requiredTagKey = requiredTagKey.TrimStart(new char[] { '!' });
				}
				IEnumerable<KeyValuePair<string, string>> enumerable;
				if (string.IsNullOrWhiteSpace(requiredTagValue))
				{
					enumerable = from pair in actualTags
						where !string.IsNullOrWhiteSpace(pair.Key) && pair.Key.Trim().Equals(requiredTagKey.Trim(), StringComparison.OrdinalIgnoreCase)
						select pair;
				}
				else
				{
					enumerable = from pair in actualTags
						where !string.IsNullOrWhiteSpace(pair.Key) && pair.Key.Equals(requiredTagKey, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(pair.Value) && EligibilityFilter.DoesTextMatchWithWildcardAndComparision(requiredTagValue.Trim(), pair.Value)
						select pair;
				}
				bool flag3 = enumerable != null && enumerable.Any<KeyValuePair<string, string>>();
				if (flag2)
				{
					flag = !flag3;
				}
				else
				{
					flag = flag3;
				}
			}
			return flag;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002D04 File Offset: 0x00000F04
		private static bool DoesTextVersionMatch(string versionRequirementsPattern, string actualVersionText)
		{
			bool result = false;
			try
			{
				if (!string.IsNullOrWhiteSpace(actualVersionText) && !string.IsNullOrWhiteSpace(actualVersionText))
				{
					Version v = new Version(actualVersionText);
					if (versionRequirementsPattern.StartsWith("<"))
					{
						if (versionRequirementsPattern.StartsWith("<="))
						{
							Version v2 = new Version(versionRequirementsPattern.Substring(2));
							result = v <= v2;
						}
						else
						{
							Version v3 = new Version(versionRequirementsPattern.Substring(1));
							result = v < v3;
						}
					}
					else if (versionRequirementsPattern.StartsWith(">"))
					{
						if (versionRequirementsPattern.StartsWith(">="))
						{
							Version v4 = new Version(versionRequirementsPattern.Substring(2));
							result = v >= v4;
						}
						else
						{
							Version v5 = new Version(versionRequirementsPattern.Substring(1));
							result = v > v5;
						}
					}
					else
					{
						result = EligibilityFilter.DoesTextMatchWithWildcard(versionRequirementsPattern, actualVersionText);
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002DE4 File Offset: 0x00000FE4
		private static bool DoesTextMatchWithWildcardAndComparision(string itemToMatch, string itemToEvaluate)
		{
			return EligibilityFilter.DoesTextMatchWithComparision(itemToMatch, itemToEvaluate) || EligibilityFilter.DoesTextMatchWithWildcard(itemToMatch, itemToEvaluate);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002DF8 File Offset: 0x00000FF8
		private static bool DoesTextMatchWithComparision(string itemToMatch, string itemToEvaluate)
		{
			bool result = false;
			if (string.IsNullOrWhiteSpace(itemToMatch))
			{
				result = false;
			}
			else if (string.IsNullOrWhiteSpace(itemToEvaluate))
			{
				result = false;
			}
			else
			{
				itemToMatch = itemToMatch.Trim();
				itemToEvaluate = itemToEvaluate.Trim();
				if (!itemToEvaluate.Contains("."))
				{
					itemToEvaluate += ".0";
				}
				if (!itemToMatch.Contains("."))
				{
					itemToMatch += ".0";
				}
				Version v7;
				Version v8;
				if (!itemToMatch.StartsWith("<") && !itemToMatch.StartsWith(">"))
				{
					result = false;
				}
				else if (itemToMatch.StartsWith(">="))
				{
					Version v;
					Version v2;
					if (Version.TryParse(itemToMatch.Trim(new char[] { '>' }).Trim(new char[] { '=' }), out v) && Version.TryParse(itemToEvaluate, out v2) && v2 >= v)
					{
						result = true;
					}
				}
				else if (itemToMatch.StartsWith(">"))
				{
					Version v3;
					Version v4;
					if (Version.TryParse(itemToMatch.Trim(new char[] { '>' }), out v3) && Version.TryParse(itemToEvaluate, out v4) && v4 > v3)
					{
						result = true;
					}
				}
				else if (itemToMatch.StartsWith("<="))
				{
					Version v5;
					Version v6;
					if (Version.TryParse(itemToMatch.Trim(new char[] { '<' }).Trim(new char[] { '=' }), out v5) && Version.TryParse(itemToEvaluate, out v6) && v6 <= v5)
					{
						result = true;
					}
				}
				else if (itemToMatch.StartsWith("<") && Version.TryParse(itemToMatch.Trim(new char[] { '<' }), out v7) && Version.TryParse(itemToEvaluate, out v8) && v8 < v7)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002FC0 File Offset: 0x000011C0
		private static bool DoesTextMatchWithWildcard(string itemToMatch, string itemToEvaluate)
		{
			bool result = false;
			if (string.IsNullOrWhiteSpace(itemToMatch))
			{
				result = false;
			}
			else if (string.IsNullOrWhiteSpace(itemToEvaluate))
			{
				result = false;
			}
			else
			{
				itemToMatch = itemToMatch.Trim();
				itemToEvaluate = itemToEvaluate.Trim();
				if (!itemToMatch.Contains('*'))
				{
					if (itemToEvaluate.Equals(itemToMatch, StringComparison.OrdinalIgnoreCase))
					{
						result = true;
					}
				}
				else if (itemToMatch.StartsWith("*") && itemToMatch.EndsWith("*"))
				{
					if (itemToEvaluate.IndexOf(itemToMatch.Trim(new char[] { '*' }), StringComparison.OrdinalIgnoreCase) >= 0)
					{
						result = true;
					}
				}
				else if (itemToMatch.EndsWith("*"))
				{
					if (itemToEvaluate.StartsWith(itemToMatch.TrimEnd(new char[] { '*' }), StringComparison.OrdinalIgnoreCase))
					{
						result = true;
					}
				}
				else if (itemToMatch.StartsWith("*") && itemToEvaluate.EndsWith(itemToMatch.TrimStart(new char[] { '*' }), StringComparison.OrdinalIgnoreCase))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x020000CF RID: 207
		private static class ComparrisonMethods
		{
			// Token: 0x040003A1 RID: 929
			public static readonly Func<string, string, bool> CaseInsensitiveWithWildcard = (string actual, string filter) => EligibilityFilter.DoesTextMatchWithWildcard(actual, filter);

			// Token: 0x040003A2 RID: 930
			public static readonly Func<string, string, bool> CaseInsensitive = (string actual, string filter) => filter.Equals(actual, StringComparison.OrdinalIgnoreCase);

			// Token: 0x040003A3 RID: 931
			public static readonly Func<string, string, bool> CaseSensitive = (string actual, string filter) => filter.Equals(actual, StringComparison.Ordinal);

			// Token: 0x040003A4 RID: 932
			public static readonly Func<string, string, bool> BeginsWith = (string actual, string filter) => filter.StartsWith(actual, StringComparison.OrdinalIgnoreCase);
		}
	}
}
