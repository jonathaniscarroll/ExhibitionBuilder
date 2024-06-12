using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SetVideoUrl : MonoBehaviour
{
	[SerializeField]
	private string _videoUrl;
	public string videoUrl{
		get{return _videoUrl;}
		set{
			_videoUrl = value;
			videoPlayer.url = _videoUrl;
		}
	}
	
	public VideoPlayer videoPlayer;
}
