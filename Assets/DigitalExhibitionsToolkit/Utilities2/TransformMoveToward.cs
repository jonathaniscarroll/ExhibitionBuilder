using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformMoveToward : MonoBehaviour
{
	public Transform TargetToMove{
		get{return _targetToMove;}
		set{_targetToMove = value;}
	}
	[SerializeField]
	private Transform _targetToMove;
	
	public Transform TargetToMoveToward{
		get{return _targetToMoveToward;}
		set{_targetToMoveToward = value;}
	}
	[SerializeField]
	private Transform _targetToMoveToward;
	public float Speed{
		get{
			return speed;
		}
		set{
			speed = value;
		}
	}
	public Vector3 Modifier = Vector3.one;
	[SerializeField]
	private float speed = 1;
	public UnityEvent OnDestination;
	
	public void MoveToward(){
		if(TargetToMove==null||TargetToMoveToward==null){
			return;
		}
		Vector3 target = new Vector3(TargetToMoveToward.position.x*Modifier.x,TargetToMoveToward.position.y*Modifier.y,TargetToMoveToward.position.z*Modifier.z);
		TargetToMove.position = Vector3.MoveTowards(TargetToMove.position,target,Speed*Time.deltaTime);
		if(Vector3.Distance(TargetToMove.position,TargetToMoveToward.position)<0.1){
			OnDestination.Invoke();
		}
	}
}
