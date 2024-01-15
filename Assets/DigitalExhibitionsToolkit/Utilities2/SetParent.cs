using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
	[SerializeField]
	private Transform _transformToSet;
	public Transform TransformToSet{
		get{
			return _transformToSet;
		} set{
			_transformToSet = value;
			originalParent = _transformToSet.parent;
		}
	}
	[SerializeField]
	private Transform _parent;
	public Transform Parent{
		get{
			return _parent;
		} set{
			_parent = value;
		}
	}
	
	private Transform originalParent;
	
	void Start(){
		if(TransformToSet !=null)
		originalParent = TransformToSet.parent;
	}
	public void Set(){
		if(TransformToSet!=null)
			TransformToSet.parent = Parent;
		else
			Debug.Log("null");
	}
	
	public void UnSet(){
		if(TransformToSet!=null)
			TransformToSet.parent = null;
		else
			Debug.Log("null");
	}
	
	public void Reset(){
		if(TransformToSet!=null)
			TransformToSet.parent = originalParent;
		else
			Debug.Log("null");
	}
}
