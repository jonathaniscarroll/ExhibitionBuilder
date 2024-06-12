using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AwsLister : MonoBehaviour
{
	private const string awsBucketName = "detail-deb";
	private const string awsAccessKey = "AKIAU5WTADE5ER557ZWX";
	private const string awsSecretKey = "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";
	private const string region = "ca-central-1";
	private const string serviceName = "s3";

	public GameObject buttonPrefab; // Prefab for the button
	public Transform buttonContainer; // Container to hold the buttons
	public UnityEvent onListed;

	private Dictionary<string, GameObject> existingButtons = new Dictionary<string, GameObject>();


	public void ListFilesInS3()
	{
		try
		{
			var endpointUri = $"https://{awsBucketName}.s3.{region}.amazonaws.com";
			var requestUri = new Uri(endpointUri + "?list-type=2");
			string amzDate = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
			string dateStamp = DateTime.UtcNow.ToString("yyyyMMdd");

			// Step 1: Create Canonical Request
			var canonicalUri = "/";
			var canonicalQueryString = "list-type=2";
			var canonicalHeaders = $"host:{requestUri.Host}\nx-amz-content-sha256:UNSIGNED-PAYLOAD\nx-amz-date:{amzDate}\n";
			var signedHeaders = "host;x-amz-content-sha256;x-amz-date";
			var payloadHash = "UNSIGNED-PAYLOAD";
			var canonicalRequest = $"GET\n{canonicalUri}\n{canonicalQueryString}\n{canonicalHeaders}\n{signedHeaders}\n{payloadHash}";

			// Step 2: Create String to Sign
			var credentialScope = $"{dateStamp}/{region}/{serviceName}/aws4_request";
			var stringToSign = $"AWS4-HMAC-SHA256\n{amzDate}\n{credentialScope}\n{SHA256Hash(canonicalRequest)}";

			// Step 3: Calculate the Signature
			var signingKey = GetSignatureKey(awsSecretKey, dateStamp, region, serviceName);
			var signature = Sign(stringToSign, signingKey);

			// Step 4: Add Authorization Header
			var authorizationHeader = $"AWS4-HMAC-SHA256 Credential={awsAccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={BitConverter.ToString(signature).Replace("-", "").ToLower()}";

			// Create and configure the request
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
			request.Method = "GET";
			request.Headers.Add("x-amz-date", amzDate);
			request.Headers.Add("x-amz-content-sha256", payloadHash);
			request.Headers.Add("Authorization", authorizationHeader);

			// Send the request and get the response
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				Debug.Log("List completed with status: " + response.StatusCode);
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					var responseBody = reader.ReadToEnd();
					Debug.Log("Response Body: " + responseBody);

					// Parse XML response to list keys and construct full URLs
					XDocument xml = XDocument.Parse(responseBody);
					foreach (XElement element in xml.Descendants("{http://s3.amazonaws.com/doc/2006-03-01/}Key"))
					{
						string fileKey = element.Value;
						string fileUrl = $"https://{awsBucketName}.s3.{region}.amazonaws.com/{Uri.EscapeDataString(fileKey)}";
						Debug.Log("File URL: " + fileUrl);

						// Check if the button already exists
						if (!existingButtons.ContainsKey(fileKey))
						{
							// Create a button for each new file
							CreateButton(fileKey, fileUrl);
						}
					}
					onListed.Invoke();
				}
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

	void CreateButton(string text, string url)
	{
		GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
		Button button = buttonObj.GetComponent<Button>();
		TMPro.TMP_Text buttonText = buttonObj.GetComponentInChildren<TMPro.TMP_Text>();
		buttonObj.SetActive(true);

		if (buttonText != null)
		{
			buttonText.text = text;
		}

		button.onClick.AddListener(() => CopyToClipboard(url));

		// Add the new button to the dictionary
		existingButtons.Add(text, buttonObj);
	}

	void CopyToClipboard(string text)
	{
		GUIUtility.systemCopyBuffer = text;
		Debug.Log("Copied to clipboard: " + text);
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

	private static byte[] Sign(string stringToSign, byte[] key)
	{
		return HmacSHA256(key, Encoding.UTF8.GetBytes(stringToSign));
	}
}
