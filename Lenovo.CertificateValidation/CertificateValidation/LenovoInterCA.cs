﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Lenovo.CertificateValidation
{
	// Token: 0x0200000A RID: 10
	internal static class LenovoInterCA
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000041E0 File Offset: 0x000023E0
		public static ThreadLocal<List<X509Certificate2>> ExtraCertificates { get; } = new ThreadLocal<List<X509Certificate2>>(() => new List<X509Certificate2>());

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000041E7 File Offset: 0x000023E7
		public static X509Certificate2[] IssuerCerts { get; } = new X509Certificate2[]
		{
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIFWTCCBEGgAwIBAgIQPXjX+XZJYLJhffTwHsqGKjANBgkqhkiG9w0BAQsFADCB\r\nyjELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwNiBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MUUwQwYDVQQDEzxW\r\nZXJpU2lnbiBDbGFzcyAzIFB1YmxpYyBQcmltYXJ5IENlcnRpZmljYXRpb24gQXV0\r\naG9yaXR5IC0gRzUwHhcNMTMxMjEwMDAwMDAwWhcNMjMxMjA5MjM1OTU5WjB/MQsw\r\nCQYDVQQGEwJVUzEdMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xHzAdBgNV\r\nBAsTFlN5bWFudGVjIFRydXN0IE5ldHdvcmsxMDAuBgNVBAMTJ1N5bWFudGVjIENs\r\nYXNzIDMgU0hBMjU2IENvZGUgU2lnbmluZyBDQTCCASIwDQYJKoZIhvcNAQEBBQAD\r\nggEPADCCAQoCggEBAJeDHgAWryyx0gjE12iTUWAecfbiR7TbWE0jYmq0v1obUfej\r\nDRh3aLvYNqsvIVDanvPnXydOC8KXyAlwk6naXA1OpA2RoLTsFM6RclQuzqPbROlS\r\nGz9BPMpK5KrA6DmrU8wh0MzPf5vmwsxYaoIV7j02zxzFlwckjvF7vjEtPW7ctZlC\r\nn0thlV8ccO4XfduL5WGJeMdoG68ReBqYrsRVR1PZszLWoQ5GQMWXkorRU6eZW4U1\r\nV9Pqk2JhIArHMHckEU1ig7a6e2iCMe5lyt/51Y2yNdyMK29qclxghJzyDJRewFZS\r\nAEjM0/ilfd4v1xPkOKiE1Ua4E4bCG53qWjjdm9sCAwEAAaOCAYMwggF/MC8GCCsG\r\nAQUFBwEBBCMwITAfBggrBgEFBQcwAYYTaHR0cDovL3MyLnN5bWNiLmNvbTASBgNV\r\nHRMBAf8ECDAGAQH/AgEAMGwGA1UdIARlMGMwYQYLYIZIAYb4RQEHFwMwUjAmBggr\r\nBgEFBQcCARYaaHR0cDovL3d3dy5zeW1hdXRoLmNvbS9jcHMwKAYIKwYBBQUHAgIw\r\nHBoaaHR0cDovL3d3dy5zeW1hdXRoLmNvbS9ycGEwMAYDVR0fBCkwJzAloCOgIYYf\r\naHR0cDovL3MxLnN5bWNiLmNvbS9wY2EzLWc1LmNybDAdBgNVHSUEFjAUBggrBgEF\r\nBQcDAgYIKwYBBQUHAwMwDgYDVR0PAQH/BAQDAgEGMCkGA1UdEQQiMCCkHjAcMRow\r\nGAYDVQQDExFTeW1hbnRlY1BLSS0xLTU2NzAdBgNVHQ4EFgQUljtT8Hkzl699g+8u\r\nK8zKt4YecmYwHwYDVR0jBBgwFoAUf9Nlp8Ld7LvwMAnzQzn6Aq8zMTMwDQYJKoZI\r\nhvcNAQELBQADggEBABOFGh5pqTf3oL2kr34dYVP+nYxeDKZ1HngXI9397BoDVTn7\r\ncZXHZVqnjjDSRFph23Bv2iEFwi5zuknx0ZP+XcnNXgPgiZ4/dB7X9ziLqdbPuzUv\r\nM1ioklbRyE07guZ5hBb8KLCxR/Mdoj7uh9mmf6RWpT+thC4p3ny8qKqjPQQB6rqT\r\nog5QIikXTIfkOhFf1qQliZsFay+0yQFMJ3sLrBkFIqBgFT/ayftNTI/7cmd3/SeU\r\nx7o1DohJ/o39KK9KEr0Ns5cF3kQMFfo2KwPcwVAB8aERXRTl4r0nS1S+K4ReD6bD\r\ndAUK75fDiSKxH3fzvc1D1PFMqT+1i4SvZPLQFCE=\r\n-----END CERTIFICATE-----")),
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIFRzCCBC+gAwIBAgIQfBs1NUrn23TnQV8RacprqDANBgkqhkiG9w0BAQsFADCB\r\nvTELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwOCBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MTgwNgYDVQQDEy9W\r\nZXJpU2lnbiBVbml2ZXJzYWwgUm9vdCBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTAe\r\nFw0xNDA3MjIwMDAwMDBaFw0yNDA3MjEyMzU5NTlaMIGEMQswCQYDVQQGEwJVUzEd\r\nMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xHzAdBgNVBAsTFlN5bWFudGVj\r\nIFRydXN0IE5ldHdvcmsxNTAzBgNVBAMTLFN5bWFudGVjIENsYXNzIDMgU0hBMjU2\r\nIENvZGUgU2lnbmluZyBDQSAtIEcyMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIB\r\nCgKCAQEA15VD1NzfZ645+1KktiYxBHDpt45bKro3aTWVj7vAMOeG2HO73+vRdj+K\r\nVo7rLUvwVxhOsY2lM9MLdSPVankn3aPT9w6HZbXerRzx9TW0IlGvIqHBXUuQf8BZ\r\nTqudeakC1x5JsTtNh/7CeKu/71KunK8I2TnlmlE+aV8wEE5xY2xY4fAgMxsPdL5b\r\nyxLh24zEgJRyu/ZFmp7BJQv7oxye2KYJcHHswEdMj33D3hnOPu4Eco4X0//wsgUy\r\nGUzTsByf/qV4IEJwQbAmjG8AyDoAEUF6QbCnipEEoJl49He082Aq5mxQBLcUYP8N\r\nUfSoi4T+IdpcXn31KXlPsER0b21y/wIDAQABo4IBeDCCAXQwLgYIKwYBBQUHAQEE\r\nIjAgMB4GCCsGAQUFBzABhhJodHRwOi8vcy5zeW1jZC5jb20wEgYDVR0TAQH/BAgw\r\nBgEB/wIBADBmBgNVHSAEXzBdMFsGC2CGSAGG+EUBBxcDMEwwIwYIKwYBBQUHAgEW\r\nF2h0dHBzOi8vZC5zeW1jYi5jb20vY3BzMCUGCCsGAQUFBwICMBkaF2h0dHBzOi8v\r\nZC5zeW1jYi5jb20vcnBhMDYGA1UdHwQvMC0wK6ApoCeGJWh0dHA6Ly9zLnN5bWNi\r\nLmNvbS91bml2ZXJzYWwtcm9vdC5jcmwwEwYDVR0lBAwwCgYIKwYBBQUHAwMwDgYD\r\nVR0PAQH/BAQDAgEGMCkGA1UdEQQiMCCkHjAcMRowGAYDVQQDExFTeW1hbnRlY1BL\r\nSS0xLTcyNDAdBgNVHQ4EFgQU1MAGIknrOUvdk+JcobhHdglyA1gwHwYDVR0jBBgw\r\nFoAUtnf6aUhHn1MS1cLqBzJ2B9GXBxkwDQYJKoZIhvcNAQELBQADggEBAH/ryqfq\r\ni3ZC6z6OIFQw47e53PpIPhbHD0WVEM0nhqNm8wLtcfiqwlWXkXCD+VJ+Umk8yfHg\r\nlEaAGLuh1KRWpvMdAJHVhvNIh+DLxDRoIF60y/kF7ZyvcFMnueg+flGgaXGL3FHt\r\ngDolMp9Er25DKNMhdbuX2IuLjP6pBEYEhfcVnEsRjcQsF/7Vbn+a4laS8ZazrS35\r\n9N/aiZnOsjhEwPdHe8olufoqaDObUHLeqJ/UzSwLNL2LMHhA4I2OJxuQbxq+CBWB\r\nXesv4lHnUR7JeCnnHmW/OO8BSgEJJA4WxBR5wUE3NNA9kVKUneFo7wjw4mmcZ26Q\r\nCxqTcdQmAsPAWiM=\r\n-----END CERTIFICATE-----")),
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIGCjCCBPKgAwIBAgIQUgDlqiVW/BqG7ZbJ1EszxzANBgkqhkiG9w0BAQUFADCB\r\nyjELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwNiBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MUUwQwYDVQQDEzxW\r\nZXJpU2lnbiBDbGFzcyAzIFB1YmxpYyBQcmltYXJ5IENlcnRpZmljYXRpb24gQXV0\r\naG9yaXR5IC0gRzUwHhcNMTAwMjA4MDAwMDAwWhcNMjAwMjA3MjM1OTU5WjCBtDEL\r\nMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQLExZW\r\nZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTswOQYDVQQLEzJUZXJtcyBvZiB1c2UgYXQg\r\naHR0cHM6Ly93d3cudmVyaXNpZ24uY29tL3JwYSAoYykxMDEuMCwGA1UEAxMlVmVy\r\naVNpZ24gQ2xhc3MgMyBDb2RlIFNpZ25pbmcgMjAxMCBDQTCCASIwDQYJKoZIhvcN\r\nAQEBBQADggEPADCCAQoCggEBAPUjS16l14q7MunUV/fv5Mcmfq0ZmP6onX2U9jZr\r\nENd1gTB/BGh/yyt1Hs0dCIzfaZSnN6Oce4DgmeHuN01fzjsU7obU0PUnNbwlCzin\r\njGOdF6MIpauw+81qYoJM1SHaG9nx44Q7iipPhVuQAU/Jp3YQfycDfL6ufn3B3fkF\r\nvBtInGnnwKQ8PEEAPt+W5cXklHHWVQHHACZKQDy1oSapDKdtgI6QJXvPvz8c6y+W\r\n+uWHd8a1VrJ6O1QwUxvfYjT/HtH0WpMoheVMF05+W/2kk5l/383vpHXv7xX2R+f4\r\nGXLYLjQaprSnTH69u08MPVfxMNamNo7WgHbXGS6lzX40LYkCAwEAAaOCAf4wggH6\r\nMBIGA1UdEwEB/wQIMAYBAf8CAQAwcAYDVR0gBGkwZzBlBgtghkgBhvhFAQcXAzBW\r\nMCgGCCsGAQUFBwIBFhxodHRwczovL3d3dy52ZXJpc2lnbi5jb20vY3BzMCoGCCsG\r\nAQUFBwICMB4aHGh0dHBzOi8vd3d3LnZlcmlzaWduLmNvbS9ycGEwDgYDVR0PAQH/\r\nBAQDAgEGMG0GCCsGAQUFBwEMBGEwX6FdoFswWTBXMFUWCWltYWdlL2dpZjAhMB8w\r\nBwYFKw4DAhoEFI/l0xqGrI2Oa8PPgGrUSBgsexkuMCUWI2h0dHA6Ly9sb2dvLnZl\r\ncmlzaWduLmNvbS92c2xvZ28uZ2lmMDQGA1UdHwQtMCswKaAnoCWGI2h0dHA6Ly9j\r\ncmwudmVyaXNpZ24uY29tL3BjYTMtZzUuY3JsMDQGCCsGAQUFBwEBBCgwJjAkBggr\r\nBgEFBQcwAYYYaHR0cDovL29jc3AudmVyaXNpZ24uY29tMB0GA1UdJQQWMBQGCCsG\r\nAQUFBwMCBggrBgEFBQcDAzAoBgNVHREEITAfpB0wGzEZMBcGA1UEAxMQVmVyaVNp\r\nZ25NUEtJLTItODAdBgNVHQ4EFgQUz5mp6nsm9EvJjo/X8AUm7+PSp50wHwYDVR0j\r\nBBgwFoAUf9Nlp8Ld7LvwMAnzQzn6Aq8zMTMwDQYJKoZIhvcNAQEFBQADggEBAFYi\r\n5jSkxGHLSLkBrVaoZA/ZjJHEu8wM5a16oCJ/30c4Si1s0X9xGnzscKmx8E/kDwxT\r\n+hVe/nSYSSSFgSYckRRHsExjjLuhNNTGRegNhSZzA9CpjGRt3HGS5kUFYBVZUTn8\r\nWBRr/tSk7XlrCAxBcuc3IgYJviPpP0SaHulhncyxkFz8PdKNrEI9ZTbUtD1AKI+b\r\nEM8jJsxLIMuQH12MTDTKPNjlN9ZvpSC9NOsm2a4N58Wa96G0IZEzb4boWLslfHQO\r\nWP51G2M/zjF8m48blp7FU3aEW5ytkfqs7ZO6XcghU8KCU2OvEg1QhxEbPVRSloos\r\nnD2SGgiaBS7Hk6VIkdM=\r\n-----END CERTIFICATE-----")),
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIFETCCA/mgAwIBAgIQD6sfig1a+H9GM2dNJLnUtTANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgRzIwHhcNMTgwNzE4MTIyNzM0WhcNMjgwNzE4MTIyNzM0WjB3MQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMTYwNAYDVQQDEy1EaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nIENBIC0gRzIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCr\r\nVfwyE1kV7R0+yGux5FUTKzFkrwg9ZLuDhQfMEEZx575G8SqIIomZShu+nfwgiUq3\r\nsI/gfs10TW6139U+8zV6cTD/4nGUjAjbaRNDGwwtHEItTM+1hOztSTyOD3HA98zn\r\ntGWteJC3xkyp0MoFS85CX6c4Fnzwtn+MidOzFMao0DLDwcKpgq7mLsZVp6j6eX9M\r\njdiqIpuKKvaU49zEPZ3CFTnLmBqCylCW3ctDkzlWeUBNZGiKosiLACZ3yY+T8p3V\r\nBYvKhxwSYGA18tCsjMds/nuZ88UNrwpZfM5S5E7cAAAmXwc0UqImu3mxPRFkFTF1\r\nFWuJPl7XjigRZizgsy5fAgMBAAGjggGpMIIBpTAdBgNVHQ4EFgQUq7hFp8XOoPVF\r\nuguY6PKUeyqmVFcwHwYDVR0jBBgwFoAUzsNKuZlV8rjbYL+pfr1WtZc2p9YwDgYD\r\nVR0PAQH/BAQDAgGGMBMGA1UdJQQMMAoGCCsGAQUFBwMDMBIGA1UdEwEB/wQIMAYB\r\nAf8CAQAweQYIKwYBBQUHAQEEbTBrMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5k\r\naWdpY2VydC5jb20wQwYIKwYBBQUHMAKGN2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0\r\nLmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RHMi5jcnQwRQYDVR0fBD4wPDA6oDig\r\nNoY0aHR0cDovL2NybDMuZGlnaWNlcnQuY29tL0RpZ2lDZXJ0QXNzdXJlZElEUm9v\r\ndEcyLmNybDA9BgNVHSAENjA0MDIGBFUdIAAwKjAoBggrBgEFBQcCARYcaHR0cHM6\r\nLy93d3cuZGlnaWNlcnQuY29tL0NQUzApBgNVHREEIjAgpB4wHDEaMBgGA1UEAxMR\r\nU3ltYW50ZWNQS0ktMi05MDIwDQYJKoZIhvcNAQELBQADggEBAMFVF4durxI7Uw7n\r\nrRrVfvKTz/5ncme4bJ8WXJQrC68BGtZZzsgzeftPjvSpqjRALuw8+pj9dgKXq6gf\r\nWkkh44xXy+bsbQaNBTH4zK3Us9AYDpCr1uTlWCYEfw7+gr7iDCmSO15PI6hSllUb\r\nkisXSy8Dnop3pTegEXo6Wt4Rk5OQOvVSho7qI0NdbW6ZtFxXMaXsqQ7iG/y36U5e\r\noF7IZS5Z6yt6DgaTtPVUJVLWRRZt/7lPm3XVoGaBBOUZYveqe2QOkYREGCYiF1X2\r\nyIoY6CxcIEWRwo3wQjISceaBwgU9jpVMdFLCEbFSs+QN4Wss/ucJPie8gQtGkk70\r\nFktrFqU=\r\n-----END CERTIFICATE-----")),
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIE3jCCA8agAwIBAgIQBEoWUa1bIxBb7J4sF3WaeTANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgRzIwHhcNMTgwNDI3MTI0NDEwWhcNMjgwNDI3MTI0NDEwWjBvMQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMS4wLAYDVQQDEyVEaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsyX17Gz8Ouar\r\n+5QqeIUl7dNb1zs8JtZk6SdY+/xLR5ZJdkfvpIRIrVUNQ+pNYfCsA/3F9F0LI84N\r\nWhIZHnT+gn4/I5DsCHlOBCVMLBWgR0oQOdq8gVhGqAaWSml6R8vz1tBAaoR2jc7h\r\n5Zj0aOGxJyKBMaJDHN5/NLrtEEfgSjiC+loApB2rh8EF1biw+BDjLsLNV3JPIAa/\r\nQXBfDHmwkDupIXhojpuLLLCCDI4phCLSF0Peuojy+3eVstdUsdfXsUHAkSBJUxeN\r\ndD49VU92NqGMcsfh0IzN+S+CfR6TjN5CoVqNVffeC7DlNcM6bc/0WkeacQ2oin1N\r\nmCnt0+IIsQIDAQABo4IBfjCCAXowHQYDVR0OBBYEFCSfASiosVIUu4sNoNT00BwP\r\nG2ixMB8GA1UdIwQYMBaAFM7DSrmZVfK422C/qX69VrWXNqfWMA4GA1UdDwEB/wQE\r\nAwIBhjATBgNVHSUEDDAKBggrBgEFBQcDAzASBgNVHRMBAf8ECDAGAQH/AgEAMHkG\r\nCCsGAQUFBwEBBG0wazAkBggrBgEFBQcwAYYYaHR0cDovL29jc3AuZGlnaWNlcnQu\r\nY29tMEMGCCsGAQUFBzAChjdodHRwOi8vY2FjZXJ0cy5kaWdpY2VydC5jb20vRGln\r\naUNlcnRBc3N1cmVkSURSb290RzIuY3J0MEUGA1UdHwQ+MDwwOqA4oDaGNGh0dHA6\r\nLy9jcmwzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RHMi5jcmww\r\nPQYDVR0gBDYwNDAyBgRVHSAAMCowKAYIKwYBBQUHAgEWHGh0dHBzOi8vd3d3LmRp\r\nZ2ljZXJ0LmNvbS9DUFMwDQYJKoZIhvcNAQELBQADggEBAL+OJc0WqsQ1qomEZ8iM\r\nQ/vXiavdOTjS1fFUxrHoPaPeZN+8cms6WSGQA7gxeJBzpuSy8n0ml/uwbsvDABqp\r\nbvQXx0mN+4UGsKg40qpfMdla4I9yULglYUpp+3ZtOp4oZsMtlQXjg2nZtDcZ7Otw\r\nnkYIycczLG1NYUqGqH/sOjHO2vwleboPHX0vv9u4VU3nCJ099FzOHP3ioBFmi2cB\r\nc8hGzvv4nnX4Z/baMFsfV0hBrE9PziT/JlbZ9s/AkrLXEwyoaTnqKbdT6UWAVCsf\r\nXBM49Um+F9AOULJrWXXw8Bq1KLAPJbOQrGNjVK4tYI1L9oW0R5/9oZvVADF7lOay\r\nH3c=\r\n-----END CERTIFICATE-----")),
			new X509Certificate2(Encoding.ASCII.GetBytes("-----BEGIN CERTIFICATE-----\r\nMIIFMDCCBBigAwIBAgIQBAkYG1/Vu2Z1U0O1b5VQCDANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgQ0EwHhcNMTMxMDIyMTIwMDAwWhcNMjgxMDIyMTIwMDAwWjByMQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMTEwLwYDVQQDEyhEaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nIENBMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA+NOzHH8O\r\nEa9ndwfTCzFJGc/Q+0WZsTrbRPV/5aid2zLXcep2nQUut4/6kkPApfmJ1DcZ17aq\r\n8JyGpdglrA55KDp+6dFn08b7KSfH03sjlOSRI5aQd4L5oYQjZhJUM1B0sSgmuyRp\r\nwsJS8hRniolF1C2ho+mILCCVrhxKhwjfDPXiTWAYvqrEsq5wMWYzcT6scKKrzn/p\r\nfMuSoeU7MRzP6vIK5Fe7SrXpdOYr/mzLfnQ5Ng2Q7+S1TqSp6moKq4TzrGdOtcT3\r\njNEgJSPrCGQ+UpbB8g8S9MWOD8Gi6CxR93O8vYWxYoNzQYIH5DiLanMg0A9kczye\r\nn6Yzqf0Z3yWT0QIDAQABo4IBzTCCAckwEgYDVR0TAQH/BAgwBgEB/wIBADAOBgNV\r\nHQ8BAf8EBAMCAYYwEwYDVR0lBAwwCgYIKwYBBQUHAwMweQYIKwYBBQUHAQEEbTBr\r\nMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5kaWdpY2VydC5jb20wQwYIKwYBBQUH\r\nMAKGN2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJ\r\nRFJvb3RDQS5jcnQwgYEGA1UdHwR6MHgwOqA4oDaGNGh0dHA6Ly9jcmw0LmRpZ2lj\r\nZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmwwOqA4oDaGNGh0dHA6\r\nLy9jcmwzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmww\r\nTwYDVR0gBEgwRjA4BgpghkgBhv1sAAIEMCowKAYIKwYBBQUHAgEWHGh0dHBzOi8v\r\nd3d3LmRpZ2ljZXJ0LmNvbS9DUFMwCgYIYIZIAYb9bAMwHQYDVR0OBBYEFFrEuXsq\r\nCqOl6nEDwGD5LfZldQ5YMB8GA1UdIwQYMBaAFEXroq/0ksuCMS1Ri6enIZ3zbcgP\r\nMA0GCSqGSIb3DQEBCwUAA4IBAQA+7A1aJLPzItEVyCx8JSl2qB1dHC06GsTvMGHX\r\nfgtg/cM9D8Svi/3vKt8gVTew4fbRknUPUbRupY5a4l4kgU4QpO4/cY5jDhNLrddf\r\nRHnzNhQGivecRk5c/5CxGwcOkRX7uq+1UcKNJK4kxscnKqEpKBo6cSgCPC6Ro8Al\r\nEeKcFEehemhor5unXCBc2XGxDI+7qPjFEmifz0DLQESlE/DmZAwlCEIysjaKJAL+\r\nL3J+HNdJRZboWR3p+nRka7LrZkPas7CM1ekN3fYBIM6ZMWM9CBoYs4GbT8aTEAb8\r\nB4H6i9r5gkn3Ym6hU/oSlBiFLpKR6mhsRDKyZqHnGKSaZFHv\r\n-----END CERTIFICATE-----"))
		};

		// Token: 0x04000035 RID: 53
		private const string Symantec = "-----BEGIN CERTIFICATE-----\r\nMIIFWTCCBEGgAwIBAgIQPXjX+XZJYLJhffTwHsqGKjANBgkqhkiG9w0BAQsFADCB\r\nyjELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwNiBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MUUwQwYDVQQDEzxW\r\nZXJpU2lnbiBDbGFzcyAzIFB1YmxpYyBQcmltYXJ5IENlcnRpZmljYXRpb24gQXV0\r\naG9yaXR5IC0gRzUwHhcNMTMxMjEwMDAwMDAwWhcNMjMxMjA5MjM1OTU5WjB/MQsw\r\nCQYDVQQGEwJVUzEdMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xHzAdBgNV\r\nBAsTFlN5bWFudGVjIFRydXN0IE5ldHdvcmsxMDAuBgNVBAMTJ1N5bWFudGVjIENs\r\nYXNzIDMgU0hBMjU2IENvZGUgU2lnbmluZyBDQTCCASIwDQYJKoZIhvcNAQEBBQAD\r\nggEPADCCAQoCggEBAJeDHgAWryyx0gjE12iTUWAecfbiR7TbWE0jYmq0v1obUfej\r\nDRh3aLvYNqsvIVDanvPnXydOC8KXyAlwk6naXA1OpA2RoLTsFM6RclQuzqPbROlS\r\nGz9BPMpK5KrA6DmrU8wh0MzPf5vmwsxYaoIV7j02zxzFlwckjvF7vjEtPW7ctZlC\r\nn0thlV8ccO4XfduL5WGJeMdoG68ReBqYrsRVR1PZszLWoQ5GQMWXkorRU6eZW4U1\r\nV9Pqk2JhIArHMHckEU1ig7a6e2iCMe5lyt/51Y2yNdyMK29qclxghJzyDJRewFZS\r\nAEjM0/ilfd4v1xPkOKiE1Ua4E4bCG53qWjjdm9sCAwEAAaOCAYMwggF/MC8GCCsG\r\nAQUFBwEBBCMwITAfBggrBgEFBQcwAYYTaHR0cDovL3MyLnN5bWNiLmNvbTASBgNV\r\nHRMBAf8ECDAGAQH/AgEAMGwGA1UdIARlMGMwYQYLYIZIAYb4RQEHFwMwUjAmBggr\r\nBgEFBQcCARYaaHR0cDovL3d3dy5zeW1hdXRoLmNvbS9jcHMwKAYIKwYBBQUHAgIw\r\nHBoaaHR0cDovL3d3dy5zeW1hdXRoLmNvbS9ycGEwMAYDVR0fBCkwJzAloCOgIYYf\r\naHR0cDovL3MxLnN5bWNiLmNvbS9wY2EzLWc1LmNybDAdBgNVHSUEFjAUBggrBgEF\r\nBQcDAgYIKwYBBQUHAwMwDgYDVR0PAQH/BAQDAgEGMCkGA1UdEQQiMCCkHjAcMRow\r\nGAYDVQQDExFTeW1hbnRlY1BLSS0xLTU2NzAdBgNVHQ4EFgQUljtT8Hkzl699g+8u\r\nK8zKt4YecmYwHwYDVR0jBBgwFoAUf9Nlp8Ld7LvwMAnzQzn6Aq8zMTMwDQYJKoZI\r\nhvcNAQELBQADggEBABOFGh5pqTf3oL2kr34dYVP+nYxeDKZ1HngXI9397BoDVTn7\r\ncZXHZVqnjjDSRFph23Bv2iEFwi5zuknx0ZP+XcnNXgPgiZ4/dB7X9ziLqdbPuzUv\r\nM1ioklbRyE07guZ5hBb8KLCxR/Mdoj7uh9mmf6RWpT+thC4p3ny8qKqjPQQB6rqT\r\nog5QIikXTIfkOhFf1qQliZsFay+0yQFMJ3sLrBkFIqBgFT/ayftNTI/7cmd3/SeU\r\nx7o1DohJ/o39KK9KEr0Ns5cF3kQMFfo2KwPcwVAB8aERXRTl4r0nS1S+K4ReD6bD\r\ndAUK75fDiSKxH3fzvc1D1PFMqT+1i4SvZPLQFCE=\r\n-----END CERTIFICATE-----";

		// Token: 0x04000036 RID: 54
		private const string SymantecG2 = "-----BEGIN CERTIFICATE-----\r\nMIIFRzCCBC+gAwIBAgIQfBs1NUrn23TnQV8RacprqDANBgkqhkiG9w0BAQsFADCB\r\nvTELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwOCBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MTgwNgYDVQQDEy9W\r\nZXJpU2lnbiBVbml2ZXJzYWwgUm9vdCBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTAe\r\nFw0xNDA3MjIwMDAwMDBaFw0yNDA3MjEyMzU5NTlaMIGEMQswCQYDVQQGEwJVUzEd\r\nMBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xHzAdBgNVBAsTFlN5bWFudGVj\r\nIFRydXN0IE5ldHdvcmsxNTAzBgNVBAMTLFN5bWFudGVjIENsYXNzIDMgU0hBMjU2\r\nIENvZGUgU2lnbmluZyBDQSAtIEcyMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIB\r\nCgKCAQEA15VD1NzfZ645+1KktiYxBHDpt45bKro3aTWVj7vAMOeG2HO73+vRdj+K\r\nVo7rLUvwVxhOsY2lM9MLdSPVankn3aPT9w6HZbXerRzx9TW0IlGvIqHBXUuQf8BZ\r\nTqudeakC1x5JsTtNh/7CeKu/71KunK8I2TnlmlE+aV8wEE5xY2xY4fAgMxsPdL5b\r\nyxLh24zEgJRyu/ZFmp7BJQv7oxye2KYJcHHswEdMj33D3hnOPu4Eco4X0//wsgUy\r\nGUzTsByf/qV4IEJwQbAmjG8AyDoAEUF6QbCnipEEoJl49He082Aq5mxQBLcUYP8N\r\nUfSoi4T+IdpcXn31KXlPsER0b21y/wIDAQABo4IBeDCCAXQwLgYIKwYBBQUHAQEE\r\nIjAgMB4GCCsGAQUFBzABhhJodHRwOi8vcy5zeW1jZC5jb20wEgYDVR0TAQH/BAgw\r\nBgEB/wIBADBmBgNVHSAEXzBdMFsGC2CGSAGG+EUBBxcDMEwwIwYIKwYBBQUHAgEW\r\nF2h0dHBzOi8vZC5zeW1jYi5jb20vY3BzMCUGCCsGAQUFBwICMBkaF2h0dHBzOi8v\r\nZC5zeW1jYi5jb20vcnBhMDYGA1UdHwQvMC0wK6ApoCeGJWh0dHA6Ly9zLnN5bWNi\r\nLmNvbS91bml2ZXJzYWwtcm9vdC5jcmwwEwYDVR0lBAwwCgYIKwYBBQUHAwMwDgYD\r\nVR0PAQH/BAQDAgEGMCkGA1UdEQQiMCCkHjAcMRowGAYDVQQDExFTeW1hbnRlY1BL\r\nSS0xLTcyNDAdBgNVHQ4EFgQU1MAGIknrOUvdk+JcobhHdglyA1gwHwYDVR0jBBgw\r\nFoAUtnf6aUhHn1MS1cLqBzJ2B9GXBxkwDQYJKoZIhvcNAQELBQADggEBAH/ryqfq\r\ni3ZC6z6OIFQw47e53PpIPhbHD0WVEM0nhqNm8wLtcfiqwlWXkXCD+VJ+Umk8yfHg\r\nlEaAGLuh1KRWpvMdAJHVhvNIh+DLxDRoIF60y/kF7ZyvcFMnueg+flGgaXGL3FHt\r\ngDolMp9Er25DKNMhdbuX2IuLjP6pBEYEhfcVnEsRjcQsF/7Vbn+a4laS8ZazrS35\r\n9N/aiZnOsjhEwPdHe8olufoqaDObUHLeqJ/UzSwLNL2LMHhA4I2OJxuQbxq+CBWB\r\nXesv4lHnUR7JeCnnHmW/OO8BSgEJJA4WxBR5wUE3NNA9kVKUneFo7wjw4mmcZ26Q\r\nCxqTcdQmAsPAWiM=\r\n-----END CERTIFICATE-----";

		// Token: 0x04000037 RID: 55
		private const string VeriSign = "-----BEGIN CERTIFICATE-----\r\nMIIGCjCCBPKgAwIBAgIQUgDlqiVW/BqG7ZbJ1EszxzANBgkqhkiG9w0BAQUFADCB\r\nyjELMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQL\r\nExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTowOAYDVQQLEzEoYykgMjAwNiBWZXJp\r\nU2lnbiwgSW5jLiAtIEZvciBhdXRob3JpemVkIHVzZSBvbmx5MUUwQwYDVQQDEzxW\r\nZXJpU2lnbiBDbGFzcyAzIFB1YmxpYyBQcmltYXJ5IENlcnRpZmljYXRpb24gQXV0\r\naG9yaXR5IC0gRzUwHhcNMTAwMjA4MDAwMDAwWhcNMjAwMjA3MjM1OTU5WjCBtDEL\r\nMAkGA1UEBhMCVVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYDVQQLExZW\r\nZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTswOQYDVQQLEzJUZXJtcyBvZiB1c2UgYXQg\r\naHR0cHM6Ly93d3cudmVyaXNpZ24uY29tL3JwYSAoYykxMDEuMCwGA1UEAxMlVmVy\r\naVNpZ24gQ2xhc3MgMyBDb2RlIFNpZ25pbmcgMjAxMCBDQTCCASIwDQYJKoZIhvcN\r\nAQEBBQADggEPADCCAQoCggEBAPUjS16l14q7MunUV/fv5Mcmfq0ZmP6onX2U9jZr\r\nENd1gTB/BGh/yyt1Hs0dCIzfaZSnN6Oce4DgmeHuN01fzjsU7obU0PUnNbwlCzin\r\njGOdF6MIpauw+81qYoJM1SHaG9nx44Q7iipPhVuQAU/Jp3YQfycDfL6ufn3B3fkF\r\nvBtInGnnwKQ8PEEAPt+W5cXklHHWVQHHACZKQDy1oSapDKdtgI6QJXvPvz8c6y+W\r\n+uWHd8a1VrJ6O1QwUxvfYjT/HtH0WpMoheVMF05+W/2kk5l/383vpHXv7xX2R+f4\r\nGXLYLjQaprSnTH69u08MPVfxMNamNo7WgHbXGS6lzX40LYkCAwEAAaOCAf4wggH6\r\nMBIGA1UdEwEB/wQIMAYBAf8CAQAwcAYDVR0gBGkwZzBlBgtghkgBhvhFAQcXAzBW\r\nMCgGCCsGAQUFBwIBFhxodHRwczovL3d3dy52ZXJpc2lnbi5jb20vY3BzMCoGCCsG\r\nAQUFBwICMB4aHGh0dHBzOi8vd3d3LnZlcmlzaWduLmNvbS9ycGEwDgYDVR0PAQH/\r\nBAQDAgEGMG0GCCsGAQUFBwEMBGEwX6FdoFswWTBXMFUWCWltYWdlL2dpZjAhMB8w\r\nBwYFKw4DAhoEFI/l0xqGrI2Oa8PPgGrUSBgsexkuMCUWI2h0dHA6Ly9sb2dvLnZl\r\ncmlzaWduLmNvbS92c2xvZ28uZ2lmMDQGA1UdHwQtMCswKaAnoCWGI2h0dHA6Ly9j\r\ncmwudmVyaXNpZ24uY29tL3BjYTMtZzUuY3JsMDQGCCsGAQUFBwEBBCgwJjAkBggr\r\nBgEFBQcwAYYYaHR0cDovL29jc3AudmVyaXNpZ24uY29tMB0GA1UdJQQWMBQGCCsG\r\nAQUFBwMCBggrBgEFBQcDAzAoBgNVHREEITAfpB0wGzEZMBcGA1UEAxMQVmVyaVNp\r\nZ25NUEtJLTItODAdBgNVHQ4EFgQUz5mp6nsm9EvJjo/X8AUm7+PSp50wHwYDVR0j\r\nBBgwFoAUf9Nlp8Ld7LvwMAnzQzn6Aq8zMTMwDQYJKoZIhvcNAQEFBQADggEBAFYi\r\n5jSkxGHLSLkBrVaoZA/ZjJHEu8wM5a16oCJ/30c4Si1s0X9xGnzscKmx8E/kDwxT\r\n+hVe/nSYSSSFgSYckRRHsExjjLuhNNTGRegNhSZzA9CpjGRt3HGS5kUFYBVZUTn8\r\nWBRr/tSk7XlrCAxBcuc3IgYJviPpP0SaHulhncyxkFz8PdKNrEI9ZTbUtD1AKI+b\r\nEM8jJsxLIMuQH12MTDTKPNjlN9ZvpSC9NOsm2a4N58Wa96G0IZEzb4boWLslfHQO\r\nWP51G2M/zjF8m48blp7FU3aEW5ytkfqs7ZO6XcghU8KCU2OvEg1QhxEbPVRSloos\r\nnD2SGgiaBS7Hk6VIkdM=\r\n-----END CERTIFICATE-----";

		// Token: 0x04000038 RID: 56
		private const string digiCert = "-----BEGIN CERTIFICATE-----\r\nMIIFETCCA/mgAwIBAgIQD6sfig1a+H9GM2dNJLnUtTANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgRzIwHhcNMTgwNzE4MTIyNzM0WhcNMjgwNzE4MTIyNzM0WjB3MQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMTYwNAYDVQQDEy1EaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nIENBIC0gRzIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCr\r\nVfwyE1kV7R0+yGux5FUTKzFkrwg9ZLuDhQfMEEZx575G8SqIIomZShu+nfwgiUq3\r\nsI/gfs10TW6139U+8zV6cTD/4nGUjAjbaRNDGwwtHEItTM+1hOztSTyOD3HA98zn\r\ntGWteJC3xkyp0MoFS85CX6c4Fnzwtn+MidOzFMao0DLDwcKpgq7mLsZVp6j6eX9M\r\njdiqIpuKKvaU49zEPZ3CFTnLmBqCylCW3ctDkzlWeUBNZGiKosiLACZ3yY+T8p3V\r\nBYvKhxwSYGA18tCsjMds/nuZ88UNrwpZfM5S5E7cAAAmXwc0UqImu3mxPRFkFTF1\r\nFWuJPl7XjigRZizgsy5fAgMBAAGjggGpMIIBpTAdBgNVHQ4EFgQUq7hFp8XOoPVF\r\nuguY6PKUeyqmVFcwHwYDVR0jBBgwFoAUzsNKuZlV8rjbYL+pfr1WtZc2p9YwDgYD\r\nVR0PAQH/BAQDAgGGMBMGA1UdJQQMMAoGCCsGAQUFBwMDMBIGA1UdEwEB/wQIMAYB\r\nAf8CAQAweQYIKwYBBQUHAQEEbTBrMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5k\r\naWdpY2VydC5jb20wQwYIKwYBBQUHMAKGN2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0\r\nLmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RHMi5jcnQwRQYDVR0fBD4wPDA6oDig\r\nNoY0aHR0cDovL2NybDMuZGlnaWNlcnQuY29tL0RpZ2lDZXJ0QXNzdXJlZElEUm9v\r\ndEcyLmNybDA9BgNVHSAENjA0MDIGBFUdIAAwKjAoBggrBgEFBQcCARYcaHR0cHM6\r\nLy93d3cuZGlnaWNlcnQuY29tL0NQUzApBgNVHREEIjAgpB4wHDEaMBgGA1UEAxMR\r\nU3ltYW50ZWNQS0ktMi05MDIwDQYJKoZIhvcNAQELBQADggEBAMFVF4durxI7Uw7n\r\nrRrVfvKTz/5ncme4bJ8WXJQrC68BGtZZzsgzeftPjvSpqjRALuw8+pj9dgKXq6gf\r\nWkkh44xXy+bsbQaNBTH4zK3Us9AYDpCr1uTlWCYEfw7+gr7iDCmSO15PI6hSllUb\r\nkisXSy8Dnop3pTegEXo6Wt4Rk5OQOvVSho7qI0NdbW6ZtFxXMaXsqQ7iG/y36U5e\r\noF7IZS5Z6yt6DgaTtPVUJVLWRRZt/7lPm3XVoGaBBOUZYveqe2QOkYREGCYiF1X2\r\nyIoY6CxcIEWRwo3wQjISceaBwgU9jpVMdFLCEbFSs+QN4Wss/ucJPie8gQtGkk70\r\nFktrFqU=\r\n-----END CERTIFICATE-----";

		// Token: 0x04000039 RID: 57
		private const string newDigiCert = "-----BEGIN CERTIFICATE-----\r\nMIIE3jCCA8agAwIBAgIQBEoWUa1bIxBb7J4sF3WaeTANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgRzIwHhcNMTgwNDI3MTI0NDEwWhcNMjgwNDI3MTI0NDEwWjBvMQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMS4wLAYDVQQDEyVEaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsyX17Gz8Ouar\r\n+5QqeIUl7dNb1zs8JtZk6SdY+/xLR5ZJdkfvpIRIrVUNQ+pNYfCsA/3F9F0LI84N\r\nWhIZHnT+gn4/I5DsCHlOBCVMLBWgR0oQOdq8gVhGqAaWSml6R8vz1tBAaoR2jc7h\r\n5Zj0aOGxJyKBMaJDHN5/NLrtEEfgSjiC+loApB2rh8EF1biw+BDjLsLNV3JPIAa/\r\nQXBfDHmwkDupIXhojpuLLLCCDI4phCLSF0Peuojy+3eVstdUsdfXsUHAkSBJUxeN\r\ndD49VU92NqGMcsfh0IzN+S+CfR6TjN5CoVqNVffeC7DlNcM6bc/0WkeacQ2oin1N\r\nmCnt0+IIsQIDAQABo4IBfjCCAXowHQYDVR0OBBYEFCSfASiosVIUu4sNoNT00BwP\r\nG2ixMB8GA1UdIwQYMBaAFM7DSrmZVfK422C/qX69VrWXNqfWMA4GA1UdDwEB/wQE\r\nAwIBhjATBgNVHSUEDDAKBggrBgEFBQcDAzASBgNVHRMBAf8ECDAGAQH/AgEAMHkG\r\nCCsGAQUFBwEBBG0wazAkBggrBgEFBQcwAYYYaHR0cDovL29jc3AuZGlnaWNlcnQu\r\nY29tMEMGCCsGAQUFBzAChjdodHRwOi8vY2FjZXJ0cy5kaWdpY2VydC5jb20vRGln\r\naUNlcnRBc3N1cmVkSURSb290RzIuY3J0MEUGA1UdHwQ+MDwwOqA4oDaGNGh0dHA6\r\nLy9jcmwzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RHMi5jcmww\r\nPQYDVR0gBDYwNDAyBgRVHSAAMCowKAYIKwYBBQUHAgEWHGh0dHBzOi8vd3d3LmRp\r\nZ2ljZXJ0LmNvbS9DUFMwDQYJKoZIhvcNAQELBQADggEBAL+OJc0WqsQ1qomEZ8iM\r\nQ/vXiavdOTjS1fFUxrHoPaPeZN+8cms6WSGQA7gxeJBzpuSy8n0ml/uwbsvDABqp\r\nbvQXx0mN+4UGsKg40qpfMdla4I9yULglYUpp+3ZtOp4oZsMtlQXjg2nZtDcZ7Otw\r\nnkYIycczLG1NYUqGqH/sOjHO2vwleboPHX0vv9u4VU3nCJ099FzOHP3ioBFmi2cB\r\nc8hGzvv4nnX4Z/baMFsfV0hBrE9PziT/JlbZ9s/AkrLXEwyoaTnqKbdT6UWAVCsf\r\nXBM49Um+F9AOULJrWXXw8Bq1KLAPJbOQrGNjVK4tYI1L9oW0R5/9oZvVADF7lOay\r\nH3c=\r\n-----END CERTIFICATE-----";

		// Token: 0x0400003A RID: 58
		private const string newDigiCertCA = "-----BEGIN CERTIFICATE-----\r\nMIIFMDCCBBigAwIBAgIQBAkYG1/Vu2Z1U0O1b5VQCDANBgkqhkiG9w0BAQsFADBl\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSQwIgYDVQQDExtEaWdpQ2VydCBBc3N1cmVkIElEIFJv\r\nb3QgQ0EwHhcNMTMxMDIyMTIwMDAwWhcNMjgxMDIyMTIwMDAwWjByMQswCQYDVQQG\r\nEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3d3cuZGlnaWNl\r\ncnQuY29tMTEwLwYDVQQDEyhEaWdpQ2VydCBTSEEyIEFzc3VyZWQgSUQgQ29kZSBT\r\naWduaW5nIENBMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA+NOzHH8O\r\nEa9ndwfTCzFJGc/Q+0WZsTrbRPV/5aid2zLXcep2nQUut4/6kkPApfmJ1DcZ17aq\r\n8JyGpdglrA55KDp+6dFn08b7KSfH03sjlOSRI5aQd4L5oYQjZhJUM1B0sSgmuyRp\r\nwsJS8hRniolF1C2ho+mILCCVrhxKhwjfDPXiTWAYvqrEsq5wMWYzcT6scKKrzn/p\r\nfMuSoeU7MRzP6vIK5Fe7SrXpdOYr/mzLfnQ5Ng2Q7+S1TqSp6moKq4TzrGdOtcT3\r\njNEgJSPrCGQ+UpbB8g8S9MWOD8Gi6CxR93O8vYWxYoNzQYIH5DiLanMg0A9kczye\r\nn6Yzqf0Z3yWT0QIDAQABo4IBzTCCAckwEgYDVR0TAQH/BAgwBgEB/wIBADAOBgNV\r\nHQ8BAf8EBAMCAYYwEwYDVR0lBAwwCgYIKwYBBQUHAwMweQYIKwYBBQUHAQEEbTBr\r\nMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5kaWdpY2VydC5jb20wQwYIKwYBBQUH\r\nMAKGN2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJ\r\nRFJvb3RDQS5jcnQwgYEGA1UdHwR6MHgwOqA4oDaGNGh0dHA6Ly9jcmw0LmRpZ2lj\r\nZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmwwOqA4oDaGNGh0dHA6\r\nLy9jcmwzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmww\r\nTwYDVR0gBEgwRjA4BgpghkgBhv1sAAIEMCowKAYIKwYBBQUHAgEWHGh0dHBzOi8v\r\nd3d3LmRpZ2ljZXJ0LmNvbS9DUFMwCgYIYIZIAYb9bAMwHQYDVR0OBBYEFFrEuXsq\r\nCqOl6nEDwGD5LfZldQ5YMB8GA1UdIwQYMBaAFEXroq/0ksuCMS1Ri6enIZ3zbcgP\r\nMA0GCSqGSIb3DQEBCwUAA4IBAQA+7A1aJLPzItEVyCx8JSl2qB1dHC06GsTvMGHX\r\nfgtg/cM9D8Svi/3vKt8gVTew4fbRknUPUbRupY5a4l4kgU4QpO4/cY5jDhNLrddf\r\nRHnzNhQGivecRk5c/5CxGwcOkRX7uq+1UcKNJK4kxscnKqEpKBo6cSgCPC6Ro8Al\r\nEeKcFEehemhor5unXCBc2XGxDI+7qPjFEmifz0DLQESlE/DmZAwlCEIysjaKJAL+\r\nL3J+HNdJRZboWR3p+nRka7LrZkPas7CM1ekN3fYBIM6ZMWM9CBoYs4GbT8aTEAb8\r\nB4H6i9r5gkn3Ym6hU/oSlBiFLpKR6mhsRDKyZqHnGKSaZFHv\r\n-----END CERTIFICATE-----";
	}
}