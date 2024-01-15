using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UploadScreenshot : MonoBehaviour
{

	public string screenShotURL= "http://www.my-server.com/cgi-bin/screenshot.pl";
	[SerializeField]
	private string _artworkName;
	public string ArtworkName{
		get{
			return _artworkName;
		} set {
			_artworkName = value;
		}
	}
	public StringEvent OutputFileName;

	// Use this for initialization
	public void Input(Texture2D input)
	{
		StartCoroutine(UploadPNG(input));
	}

	IEnumerator UploadPNG(Texture2D tex)
	{
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		//int width = Screen.width;
		//int height = Screen.height;
		//var tex = new Texture2D( width, height, TextureFormat.RGB24, false );

		// Read screen contents into the texture
		//tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		//tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		//Destroy( tex );

		// Create a Web Form
		WWWForm form = new WWWForm();
		//form.AddField("name", ArtworkName);
		string name = ArtworkName+".png";
		form.AddBinaryData("imageFile", bytes, name, "image/png");

		WWW w = new WWW (screenShotURL, form);

		yield return w;

		if (w.error != null) {
			//error : 
			//if (OnErrorAction != null)
			//	OnErrorAction (w.error); //or OnErrorAction.Invoke (w.error);
			Debug.Log(w.error);
		} else {
			//success
			//if (OnCompleteAction != null)
			//	OnCompleteAction (w.text); //or OnCompleteAction.Invoke (w.error);
			OutputFileName.Invoke(name);
			Debug.Log(w.text);
		}
		w.Dispose ();
	}
}