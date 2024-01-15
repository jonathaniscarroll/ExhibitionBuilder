using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

	[System.Serializable]
	public class NamedStringReference{
		public string Name;
		public StringReference StringRef;
		public NamedStringReference(string _name,string _value){
			Name = _name;
			StringRef = new StringReference();
			StringRef.Value = _value;
			StringRef.UseConstant = true;
		}
		public NamedStringReference(string _name,StringReference _stringRef){
			Name = _name;
			StringRef = _stringRef;
			
		}
	}

[CreateAssetMenu]
public class NamedStringReferenceList : ScriptableObject
{

	
	public List<NamedStringReference> NamedStringReferences;
	
	public StringStringDictionaryEvent DictionaryEvent;
	
	public Dictionary<string,string> Dictionary;
	
	public void SaveToDictionary(){
		//Debug.Log("save dicationary");
		Dictionary = new Dictionary<string, string>();
		foreach(NamedStringReference namedString in NamedStringReferences){
			Dictionary.Add(namedString.Name,namedString.StringRef);
		}
		DictionaryEvent.Invoke(Dictionary);
	}
	
	public void SetValues(Dictionary<string,string> input){
		foreach(NamedStringReference nsr in NamedStringReferences){
			if(input.ContainsKey(nsr.Name)){
				//Debug.Log(input[nsr.Name] + " " + nsr.StringReference.Value);
				nsr.StringRef.Value = input[nsr.Name];
			}
		}
	}
}
