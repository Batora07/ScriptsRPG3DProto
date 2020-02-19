using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToVillage : MonoBehaviour {

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player")
		{
			GameManager.instance.PlayerInfos = PlayerStatus.instance.SetPlayerInfos();
			PlayerStatus.instance.entityStatus.SetupEntityInfosByPlayerInfos(GameManager.instance.PlayerInfos);
			SavingData.instance.GenerateAutoSave.UpdateAutoSave();
			SceneLoader.instance.LoadLevelAsync("Village");
		}
	}
}
