using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeopleWalk : MonoBehaviour {

	public Transform[] walk_Points;
	public float walk_Speed = 1f;
	public bool isIdle;

	private int walk_Index;

	private NavMeshAgent navAgent;
	private Animator anim;

	void Awake () {
		anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent>();
	}
	
	void Start () {
		if (isIdle)
		{
			anim.Play("Idle");
		} else
		{
			anim.Play("Walk");
		}
	}

	void Update()
	{
		if (!isIdle)
		{
			ChooseWalkPoint();
		}
	}

	void ChooseWalkPoint()
	{
		if(navAgent.remainingDistance <= 0.1f)
		{
			navAgent.SetDestination(walk_Points[walk_Index].position);

			if(walk_Index == walk_Points.Length - 1)
			{
				walk_Index = 0;
			} else
			{
				walk_Index++;
			}
		}
	}
}
