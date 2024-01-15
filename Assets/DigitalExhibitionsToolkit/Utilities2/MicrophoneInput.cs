using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
	#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
	void Start(){
		List<string> devices = new List<string>();
		int s =0;
		foreach(string i in Microphone.devices){
			Debug.Log(s + i);
			devices.Add(i);
			s++;
		}
		OutputDevices.Invoke(devices);
	}
	
	[SerializeField]
	private int Device;
	public int device{
		get{
			return Device;
		}
		set{
			Debug.Log("setting device " + Microphone.devices[value]);
			Device = value;
		}
	}
	
	[SerializeField]
	private int length;
	public int Length{
		get{
			return length;
		} set{
			length = value;
		}
	}
	public StringListEvent OutputDevices;

	public AudioClipEvent eventOnRecord;
	
	AudioClip clip;
	public void StartRecord(){
		//AudioSource audio = GetComponent<AudioSource>();
		clip = Microphone.Start(Microphone.devices[device], true, Length, 22050);

		//Debug.Log("start playing... position is " + Microphone.GetPosition(null));
		eventOnRecord.Invoke(clip);
		Debug.Log(Microphone.devices[device]);
	}
	
	public void StopRecord(){
		Microphone.End(Microphone.devices[device]);
		//playbackSource.Stop();
	}
	#endif
}
