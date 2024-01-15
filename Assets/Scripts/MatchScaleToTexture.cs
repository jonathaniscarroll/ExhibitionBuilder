using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScaleToTexture : MonoBehaviour
{
	public MeshRenderer MeshRenderer;
	public float scale;
	public Transform TransformToScale;
	public void Resize(){
		var texture = MeshRenderer.material.mainTexture;
		
		if(texture.width<texture.height){
			float ratio = (float)texture.width/(float)texture.height;
			float x = TransformToScale.localScale.x;
			float y = x * ratio;
			Vector3 newSize = new Vector3(x,y,1);
			Debug.Log("width " +texture.width + " height " + texture.height);
			//Debug.Log("new size " + newSize + " from ratio " + ratio + " " + texture.width/texture.height);
			TransformToScale.localScale = newSize;  
		} else {
			float ratio = (float)texture.width/(float)texture.height;
			float y = TransformToScale.localScale.y;
			float x = y * ratio;
			Vector3 newSize = new Vector3(x,y,1);
			//Debug.Log("new size " + newSize + " from ratio " + ratio);
			TransformToScale.localScale = newSize;  
		}
		 
	}
}
