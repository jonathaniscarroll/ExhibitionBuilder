using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
	[SerializeField]
	private string _imageUrl;
	public string imageUrl {get{return _imageUrl;}set{_imageUrl = value;StartCoroutine(DownloadImage(_imageUrl));}}
	public Texture2DEvent OutputTexture;

	void Start()
	{
		StartCoroutine(DownloadImage(imageUrl));
	}

	IEnumerator DownloadImage(string url)
	{
		using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
		{
			yield return uwr.SendWebRequest();

			if (uwr.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(uwr.error);
			}
			else
			{
				Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
				// Use the texture as needed
				ApplyTexture(texture);
			}
		}
	}

	void ApplyTexture(Texture2D texture)
	{
		// Example: Apply texture to a material
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material.mainTexture = texture;
		}
		OutputTexture.Invoke(texture);

		// Or you can do something else with the texture
		Debug.Log("Texture downloaded and applied successfully.");
	}
}
