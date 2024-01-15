using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SerializeImage : MonoBehaviour
{
	public Texture2DEvent OutputTexture2D;
	public StringEvent OutputSerializedImage;
	public void Serialize(Texture2D input){
		byte[] bytes;
 
		bytes = input.EncodeToPNG();
		string output = System.Convert.ToBase64String(bytes);
		Debug.Log("sreializeing " + output);
		OutputSerializedImage.Invoke("test");
	}
	
	public void Deserialize(string input){
		byte[] bytes = File.ReadAllBytes(input);
		Texture2D output = new Texture2D(2,2);
		output.LoadImage(bytes);
		OutputTexture2D.Invoke(output);
	}
}
