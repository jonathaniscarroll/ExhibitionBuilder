using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/BoolGameEvent")]
public class BoolGameEvent : ScriptableObject
{
	private List<BoolGameEventListener> listeners = new List<BoolGameEventListener>();
	public void Raise(bool input){
		
		for(int i = listeners.Count-1;i>=0;i--){
			listeners[i].OnEventRaised(input);
			Debug.Log(name,listeners[i].gameObject);
		}
	}
	public void RegisterListener(BoolGameEventListener listener){
		listeners.Add(listener);
	}
	public void UnRegisterListener(BoolGameEventListener listener){
		listeners.Remove(listener);
	}
}
