using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory
{
	// Token: 0x02000042 RID: 66
	public class ActiveDirectiveAgent
	{
		// Token: 0x06000182 RID: 386 RVA: 0x0000ABC9 File Offset: 0x00008DC9
		public ActiveDirectiveAgent()
		{
			this.iContainerSystem = new RegistrySystem();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000ABDC File Offset: 0x00008DDC
		public AppPoliciesResponse GetPolicies(AppPoliciesRequest requestXml)
		{
			AppPoliciesResponse appPoliciesResponse = null;
			try
			{
				if (requestXml != null)
				{
					appPoliciesResponse = new AppPoliciesResponse();
					PolicyList policyList = new PolicyList();
					IContainer container = this.iContainerSystem.LoadContainer(Path.Combine(Registry.CurrentUser.ToString(), Constants.ADLocation, requestXml.PolicyInformation.AppName));
					string location = Constants.User;
					List<Policy> list = new List<Policy>();
					if (container != null)
					{
						List<IContainerValue> list2 = container.GetValues(false).ToList<IContainerValue>();
						if (list2 != null)
						{
							for (int i = 0; i < list2.Count; i++)
							{
								IContainerValue containerValue = list2[i];
								Policy policy = new Policy
								{
									Name = containerValue.GetName(),
									Value = containerValue.GetValueAsString(),
									Location = location,
									Type = ((containerValue.GetType() == typeof(string)) ? typeof(string).Name : typeof(bool).Name)
								};
								list.Add(policy);
							}
						}
					}
					container = this.iContainerSystem.LoadContainer(Path.Combine(Registry.LocalMachine.ToString(), Constants.ADLocation, requestXml.PolicyInformation.AppName));
					location = Constants.LocalMachine;
					if (container != null)
					{
						List<IContainerValue> list3 = container.GetValues(false).ToList<IContainerValue>();
						if (list3 != null)
						{
							for (int j = 0; j < list3.Count; j++)
							{
								IContainerValue policyValue = list3[j];
								Policy policy = list.Find((Policy x) => x.Name == policyValue.GetName());
								if (policy == null)
								{
									policy = new Policy
									{
										Name = policyValue.GetName(),
										Value = policyValue.GetValueAsString(),
										Location = location,
										Type = ((policyValue.GetType() == typeof(string)) ? typeof(string).Name : typeof(bool).Name)
									};
									list.Add(policy);
								}
								else
								{
									policy.Value = policyValue.GetValueAsString();
									policy.Location = location;
								}
							}
						}
					}
					policyList.Policy = list.ToArray();
					appPoliciesResponse.PolicyList = policyList;
					appPoliciesResponse.Policy = new PolicyInformation();
					appPoliciesResponse.Policy.AppName = requestXml.PolicyInformation.AppName;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return appPoliciesResponse;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AE6C File Offset: 0x0000906C
		public AppPoliciesResponse GetPolicies(string appName)
		{
			AppPoliciesResponse appPoliciesResponse = null;
			try
			{
				appPoliciesResponse = new AppPoliciesResponse();
				PolicyList policyList = new PolicyList();
				IContainer container = this.iContainerSystem.LoadContainer(Path.Combine(Registry.CurrentUser.ToString(), Constants.ADLocation, appName));
				string location = Constants.User;
				List<Policy> list = new List<Policy>();
				if (container != null)
				{
					List<IContainerValue> list2 = container.GetValues(false).ToList<IContainerValue>();
					if (list2 != null)
					{
						for (int i = 0; i < list2.Count; i++)
						{
							IContainerValue containerValue = list2[i];
							Policy policy = new Policy
							{
								Name = containerValue.GetName(),
								Value = containerValue.GetValueAsString(),
								Location = location,
								Type = ((containerValue.GetType() == typeof(string)) ? typeof(string).Name : typeof(bool).Name)
							};
							list.Add(policy);
						}
					}
				}
				container = this.iContainerSystem.LoadContainer(Path.Combine(Registry.LocalMachine.ToString(), Constants.ADLocation, appName));
				location = Constants.LocalMachine;
				if (container != null)
				{
					List<IContainerValue> policyValues = container.GetValues(false).ToList<IContainerValue>();
					if (policyValues != null)
					{
						int policies2;
						int policies;
						for (policies = 0; policies < policyValues.Count; policies = policies2 + 1)
						{
							IContainerValue containerValue2 = policyValues[policies];
							Policy policy = list.Find((Policy x) => x.Name == policyValues[policies].GetName());
							if (policy == null)
							{
								policy = new Policy
								{
									Name = containerValue2.GetName(),
									Value = containerValue2.GetValueAsString(),
									Location = location,
									Type = ((containerValue2.GetType() == typeof(string)) ? typeof(string).Name : typeof(bool).Name)
								};
								list.Add(policy);
							}
							else
							{
								policy.Value = containerValue2.GetValueAsString();
								policy.Location = location;
							}
							policies2 = policies;
						}
					}
				}
				policyList.Policy = list.ToArray();
				appPoliciesResponse.PolicyList = policyList;
				appPoliciesResponse.Policy = new PolicyInformation();
				appPoliciesResponse.Policy.AppName = appName;
			}
			catch
			{
				throw;
			}
			return appPoliciesResponse;
		}

		// Token: 0x040000A0 RID: 160
		private IContainerSystem iContainerSystem;
	}
}
