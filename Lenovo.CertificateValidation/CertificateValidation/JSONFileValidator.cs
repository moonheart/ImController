using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Lenovo.Tools.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000009 RID: 9
	public sealed class JSONFileValidator
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003AD9 File Offset: 0x00001CD9
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00003AE1 File Offset: 0x00001CE1
		private JTokenType JTokenObjType { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003AEA File Offset: 0x00001CEA
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003AF2 File Offset: 0x00001CF2
		private string EmbeddedJSON { get; set; }

		// Token: 0x0600003B RID: 59 RVA: 0x00003AFC File Offset: 0x00001CFC
		public TrustStatus GetTrustStatus(string signJsonData)
		{
			TrustStatus trustStatus = TrustStatus.NotSignedWithTrustedCertificate;
			this.JTokenObjType = JTokenType.Null;
			this.EmbeddedJSON = string.Empty;
			try
			{
				string empty = string.Empty;
				string x509CertEncoded;
				if (this.GetEmbeddedSignature(signJsonData, out x509CertEncoded, out empty))
				{
					Logger.Log(Logger.LogSeverity.Information, "GetTrustStatus : Successfully retrived security token and signature");
					trustStatus = this.ValidateEmbeddedSignature(x509CertEncoded, empty);
					if (trustStatus == TrustStatus.FileTrusted)
					{
						trustStatus = this.ValidateFileContent(signJsonData, this.EmbeddedJSON);
					}
				}
			}
			catch
			{
				trustStatus = TrustStatus.ExceptionThrown;
			}
			return trustStatus;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003B70 File Offset: 0x00001D70
		public TrustStatus GetFileTrustStatus(string filename)
		{
			TrustStatus trustStatus = TrustStatus.NotSignedWithTrustedCertificate;
			this.JTokenObjType = JTokenType.Null;
			this.EmbeddedJSON = string.Empty;
			try
			{
				if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
				{
					string signJsonData = File.ReadAllText(filename);
					trustStatus = this.GetTrustStatus(signJsonData);
				}
				else if (string.IsNullOrEmpty(filename))
				{
					trustStatus = TrustStatus.FileNameNullOrEmpty;
				}
				else
				{
					trustStatus = TrustStatus.FileNotFound;
				}
			}
			catch
			{
				trustStatus = TrustStatus.ExceptionThrown;
			}
			Logger.Log(Logger.LogSeverity.Information, string.Format("GetTrustStatus : Trust status: {0}", trustStatus));
			return trustStatus;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003BF0 File Offset: 0x00001DF0
		private bool GetEmbeddedSignature(string inputJsonData, out string signerCertificate, out string securityToken)
		{
			try
			{
				signerCertificate = string.Empty;
				securityToken = string.Empty;
				JObject jobject = null;
				if (this.IsValidJson(inputJsonData))
				{
					Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : JSON is Valid. Parsing JSON");
					if (this.JTokenObjType == JTokenType.Object)
					{
						JObject jobject2 = JObject.Parse(inputJsonData);
						JToken value = jobject2.GetValue("Signature");
						if (value != null && !string.IsNullOrEmpty(value.ToString()))
						{
							jobject = (JObject)jobject2.GetValue("Signature");
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : Json object is null or empty. Type is Object");
						}
					}
					else if (this.JTokenObjType == JTokenType.Array)
					{
						JObject jobject3 = JArray.Parse(inputJsonData).Children<JObject>().FirstOrDefault((JObject obj) => obj["Signature"] != null);
						if (jobject3 != null && !string.IsNullOrEmpty(jobject3.ToString()))
						{
							jobject = (JObject)jobject3.GetValue("Signature");
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : Json object is null or empty. Type is Array");
						}
					}
					if (jobject != null && !string.IsNullOrEmpty(jobject.ToString()))
					{
						using (IEnumerator<KeyValuePair<string, JToken>> enumerator = jobject.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, JToken> keyValuePair = enumerator.Current;
								if (keyValuePair.Key.Contains("SignatureToken"))
								{
									securityToken = keyValuePair.Value.ToString();
									if (string.IsNullOrEmpty(securityToken))
									{
										Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : SecurityToken is null or empty");
										return false;
									}
								}
								else if (keyValuePair.Key.Contains("X509Certificate"))
								{
									signerCertificate = keyValuePair.Value.ToString();
									if (string.IsNullOrEmpty(signerCertificate))
									{
										Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : SignerCertificate is null or empty");
										return false;
									}
								}
							}
							goto IL_1B1;
						}
						goto IL_19C;
						IL_1B1:
						return true;
					}
					IL_19C:
					Logger.Log(Logger.LogSeverity.Information, "GetEmbeddedSignature : signatureAttributes is null or empty.");
					return false;
				}
				return false;
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "GetEmbeddedSignature : Exception while getting embeded signature. Message: " + ex.Message);
				throw ex;
			}
			return true;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003E04 File Offset: 0x00002004
		private TrustStatus ValidateEmbeddedSignature(string x509CertEncoded, string token)
		{
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			TrustStatus result;
			try
			{
				X509Certificate2 x509Certificate = new X509Certificate2(this.Base64UrlDecode(x509CertEncoded));
				if (x509Certificate != null)
				{
					string empty = string.Empty;
					trustStatus = this.ValidateToken(x509Certificate, token);
					if (trustStatus == TrustStatus.FileTrusted)
					{
						trustStatus = CertificateTools.ValidateCertificateProperties(x509Certificate);
						if (trustStatus == TrustStatus.FileTrusted)
						{
							trustStatus = CertificateTools.ValidateIssuerAndRootCertificate(x509Certificate);
						}
						if (trustStatus == TrustStatus.FileTrusted && CertificateTools.IsDisAllowedThumbprint(x509Certificate))
						{
							trustStatus = TrustStatus.FileNotTrusted;
						}
					}
				}
				result = trustStatus;
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "ValidateEmbeddedSignature : Exception while validating signature. Message: " + ex.Message);
				result = TrustStatus.ExceptionThrown;
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003E84 File Offset: 0x00002084
		private TrustStatus ValidateToken(X509Certificate2 validatingCertificate, string token)
		{
			TrustStatus result;
			try
			{
				JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
				X509ChainPolicy chainPolicy = new X509ChainPolicy
				{
					RevocationMode = X509RevocationMode.NoCheck
				};
				X509CertificateValidator certificateValidator = X509CertificateValidator.CreateChainTrustValidator(false, chainPolicy);
				X509SecurityToken issuerSigningToken = new X509SecurityToken(validatingCertificate);
				TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
				{
					ValidAudience = "http://www.lenovo.com",
					IssuerSigningToken = issuerSigningToken,
					ValidIssuer = "self",
					CertificateValidator = certificateValidator,
					ValidateLifetime = false
				};
				SecurityToken securityToken;
				ClaimsIdentity claimsIdentity = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, ref securityToken).Identity as ClaimsIdentity;
				Claim claim = ((claimsIdentity != null) ? claimsIdentity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata") : null);
				if (claim != null && claim.Value != null && !string.IsNullOrEmpty(claim.Value))
				{
					this.EmbeddedJSON = claim.Value;
					Logger.Log(Logger.LogSeverity.Information, "ValidateToken : EmbededJSON value has been set.");
					result = TrustStatus.FileTrusted;
				}
				else
				{
					result = TrustStatus.FileNotTrusted;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "ValidateEmbeddedSignature : Exception while validating signature. Message: " + ex.Message);
				result = TrustStatus.ExceptionThrown;
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003F78 File Offset: 0x00002178
		private TrustStatus ValidateFileContent(string signJsonData, string embeddedJSON)
		{
			JToken jsonwithNoSignature = this.GetJSONWithNoSignature(signJsonData);
			JToken embeddedJSON2 = JToken.Parse(embeddedJSON);
			if (this.IsFileTampered(jsonwithNoSignature, embeddedJSON2))
			{
				return TrustStatus.FileNotTrusted;
			}
			return TrustStatus.FileTrusted;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003FA4 File Offset: 0x000021A4
		private JToken GetJSONWithNoSignature(string signJsonData)
		{
			JToken result = null;
			try
			{
				if (this.JTokenObjType == JTokenType.Object)
				{
					JObject jobject = JObject.Parse(signJsonData);
					if (jobject.Remove("Signature"))
					{
						result = jobject;
						Logger.Log(Logger.LogSeverity.Information, "GetJSONWithNoSignature : Successfully retrived JSON without signature");
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "GetJSONWithNoSignature : No signature found in JSON");
					}
				}
				else if (this.JTokenObjType == JTokenType.Array)
				{
					JArray jarray = JArray.Parse(signJsonData);
					JObject jobject2 = jarray.Children<JObject>().FirstOrDefault((JObject obj) => obj["Signature"] != null);
					if (jobject2 != null && !string.IsNullOrEmpty(jobject2.ToString()))
					{
						if (jarray.Remove(jobject2))
						{
							result = jarray;
							Logger.Log(Logger.LogSeverity.Information, "GetJSONWithNoSignature : Successfully retrived JSON without signature");
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Information, "GetJSONWithNoSignature : No signature found in JSON");
						}
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "GetJSONWithNoSignature : No signature found in JSON");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "GetJSONWithNoSignature : Exception while getting JSON without signature. Message: " + ex.Message);
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000040A0 File Offset: 0x000022A0
		private bool IsFileTampered(JToken fileJSON, JToken embeddedJSON)
		{
			return !JToken.DeepEquals(fileJSON, embeddedJSON);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000040AC File Offset: 0x000022AC
		private byte[] Base64UrlDecode(string input)
		{
			string text = input.Replace('-', '+');
			text = text.Replace('_', '/');
			switch (text.Length % 4)
			{
			case 0:
				goto IL_60;
			case 2:
				text += "==";
				goto IL_60;
			case 3:
				text += "=";
				goto IL_60;
			}
			throw new Exception("Illegal base64url string!");
			IL_60:
			return Convert.FromBase64String(text);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004120 File Offset: 0x00002320
		private bool IsValidJson(string jsonData)
		{
			if (string.IsNullOrEmpty(jsonData))
			{
				return false;
			}
			jsonData = jsonData.Trim();
			if ((jsonData.StartsWith("{") && jsonData.EndsWith("}")) || (jsonData.StartsWith("[") && jsonData.EndsWith("]")))
			{
				try
				{
					JToken jtoken = JToken.Parse(jsonData);
					this.JTokenObjType = jtoken.Type;
					return true;
				}
				catch (JsonReaderException ex)
				{
					Logger.Log(Logger.LogSeverity.Error, "IsValidJson : JsonReaderException while validating JSON. Message: " + ex.Message);
					return false;
				}
				catch (Exception ex2)
				{
					Logger.Log(Logger.LogSeverity.Error, "IsValidJson : Exception while validating JSON. Message: " + ex2.Message);
					return false;
				}
				return false;
			}
			return false;
		}

		// Token: 0x0400002E RID: 46
		private const string SignatureKeyName = "Signature";

		// Token: 0x0400002F RID: 47
		private const string SignatureKeyTokenName = "SignatureToken";

		// Token: 0x04000030 RID: 48
		private const string x509CertificatekeyName = "X509Certificate";
	}
}
