using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	[field:SerializeField]
	public Transform TransformToFollow{get;set;}
	[field:SerializeField]
	public Transform TransformToMove{get;set;}
	
	void Update(){
		if(TransformToFollow!=null&&TransformToMove!=null)
		TransformToMove.position = TransformToFollow.position;
	}
}
