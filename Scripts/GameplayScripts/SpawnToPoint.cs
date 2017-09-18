using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnToPoint : MonoBehaviour {

	public GameObject spawnPoint;	

	public void spawningTo(GameObject spawnPos)
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector3 pos = spawnPos.transform.position;
		player.transform.position = pos;
	}

}
