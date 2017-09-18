using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherWorlds : MonoBehaviour {

	public GameObject loadBtn;

	private void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player")
		{
			loadBtn.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider target)
	{
		if (target.tag == "Player")
		{
			loadBtn.SetActive(false);
		}
	}

} // class
