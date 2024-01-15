using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadScreenshot : MonoBehaviour
{
	public TextureEvent OutputTexture;
	
	public void Download(string url){
		StartCoroutine(DownloadImage(url));
	}
	
	IEnumerator DownloadImage(string MediaUrl)
	{   
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();
		if(request.isNetworkError || request.isHttpError) 
			Debug.Log(request.error);
		else{
			OutputTexture.Invoke(((DownloadHandlerTexture) request.downloadHandler).texture);
		}
			//YourRawImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
	} 
}
