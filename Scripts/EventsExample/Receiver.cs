using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {

// pipeline is Awake / OnEnable / Start
	void Start(){

	}

	void OnEnable(){
		// Subscribe to event
		Sender.playerDiedInfo += PlayerDiedListener;
	}

	void OnDisable(){
		// Unsub to prevent memory leak
		Sender.playerDiedInfo -= PlayerDiedListener;
	}

	float PlayerDiedListener(Vector3 player, Vector3 target){
		print("Function is called. Distance is " + Vector3.Distance(player, target));
		return Vector3.Distance(player, target);
	}
}
