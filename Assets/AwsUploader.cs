using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;

public class AwsUploader : MonoBehaviour
{
	private const string awsBucketName = "detail-deb";
	private const string awsAccessKey = "AKIAU5WTADE5ER557ZWX";
	private const string awsSecretKey = "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";
	private const string region = "ca-central-1";
	private const string serviceName = "s3";

	private string videoPath;


	IEnumerator ShowFileBrowser()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, "Select File", "Select");

		if (FileBrowser.Success)
		{
			videoPath = FileBrowser.Result[0];
			Debug.Log("File selected: " + videoPath);
			UploadFileToAWS3(Path.GetFileName(videoPath), videoPath);
		}
		else
		{
			Debug.LogError("File selection failed or canceled.");
		}
	}
	
	public bool hasUploaded;
	
	public StringEvent OutputUrl;

	public void UploadFileToAWS3(string fileName, string filePath)
	{
		try
		{
			var endpointUri = $"https://{awsBucketName}.s3.{region}.amazonaws.com/{Uri.EscapeDataString(fileName)}";
			var requestUri = new Uri(endpointUri);
			string amzDate = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
			string dateStamp = DateTime.UtcNow.ToString("yyyyMMdd");

			// Step 1: Create Canonical Request
			var canonicalUri = $"/{Uri.EscapeDataString(fileName)}";
			var canonicalQueryString = "";
			var canonicalHeaders = $"host:{requestUri.Host}\nx-amz-acl:public-read\nx-amz-date:{amzDate}\n";
			var signedHeaders = "host;x-amz-acl;x-amz-date";
			var payloadHash = HashRequestBody(filePath);
			var canonicalRequest = $"PUT\n{canonicalUri}\n{canonicalQueryString}\n{canonicalHeaders}\n{signedHeaders}\n{payloadHash}";

			// Step 2: Create String to Sign
			var credentialScope = $"{dateStamp}/{region}/{serviceName}/aws4_request";
			var stringToSign = $"AWS4-HMAC-SHA256\n{amzDate}\n{credentialScope}\n{SHA256Hash(canonicalRequest)}";

			// Step 3: Calculate the Signature
			var signingKey = GetSignatureKey(awsSecretKey, dateStamp, region, serviceName);
			var signature = Sign(stringToSign, signingKey);

			// Step 4: Add Authorization Header
			var authorizationHeader = $"AWS4-HMAC-SHA256 Credential={awsAccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={BitConverter.ToString(signature).Replace("-", "").ToLower()}";

			// Create and configure the request
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUri);
			request.Method = "PUT";
			request.Headers.Add("x-amz-date", amzDate);
			request.Headers.Add("x-amz-content-sha256", payloadHash);
			request.Headers.Add("x-amz-acl", "public-read");
			request.Headers.Add("Authorization", authorizationHeader);
			request.ContentType = "binary/octet-stream";

			// Read the file content and write it to the request stream
			var fileContent = File.ReadAllBytes(filePath);
			request.ContentLength = fileContent.Length;
			using (var stream = request.GetRequestStream())
			{
				stream.Write(fileContent, 0, fileContent.Length);
			}

			Debug.Log("Sent bytes: " + request.ContentLength + ", for file: " + fileName);

			// Send the request and get the response
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				Debug.Log("Upload completed with status: " + response.StatusCode);
				string fileUrl = endpointUri;
				OutputUrl.Invoke(fileUrl);
				Debug.Log("File URL: " + fileUrl);
				hasUploaded = true;
			}
		}
			catch (WebException ex)
			{
				Debug.LogError("Web exception occurred: " + ex.Message);
				if (ex.Response != null)
				{
					using (var responseStream = ex.Response.GetResponseStream())
						using (var reader = new StreamReader(responseStream))
						{
							var responseText = reader.ReadToEnd();
							Debug.LogError("Response error: " + responseText);
						}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception occurred: " + ex.Message);
			}
	}

	private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
	{
		byte[] kDate = HmacSHA256(Encoding.UTF8.GetBytes("AWS4" + key), Encoding.UTF8.GetBytes(dateStamp));
		byte[] kRegion = HmacSHA256(kDate, Encoding.UTF8.GetBytes(regionName));
		byte[] kService = HmacSHA256(kRegion, Encoding.UTF8.GetBytes(serviceName));
		byte[] kSigning = HmacSHA256(kService, Encoding.UTF8.GetBytes("aws4_request"));
		return kSigning;
	}

	private static byte[] HmacSHA256(byte[] key, byte[] data)
	{
		using (var hmacsha256 = new HMACSHA256(key))
		{
			return hmacsha256.ComputeHash(data);
		}
	}

	private static string SHA256Hash(string value)
	{
		using (var sha256 = SHA256.Create())
		{
			byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
			return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
		}
	}

	private static string HashRequestBody(string filePath)
	{
		using (var sha256 = SHA256.Create())
			using (var stream = File.OpenRead(filePath))
			{
				var hash = sha256.ComputeHash(stream);
				return BitConverter.ToString(hash).Replace("-", "").ToLower();
			}
	}

	private static byte[] Sign(string stringToSign, byte[] key)
	{
		return HmacSHA256(key, Encoding.UTF8.GetBytes(stringToSign));
	}

	// Method to be called by the Inspector button
	public void StartUpload()
	{
		StartCoroutine(ShowFileBrowser());
	}
}
