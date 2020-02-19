using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAutoSave : MonoBehaviour
{
    public void UpdateAutoSave()
	{
		CreateAutoSave();
		SavingData.instance.saveLoadManager.savedInfos[0] = SavingData.instance.DefaultSaveState;
	}

	public void CreateAutoSave()
	{
		// this will create a new save state then set it to currentSavedState
		CreateSaveState newSaveState = new CreateSaveState("", true);
		// updating the default save
		SavingData.instance.DefaultSaveState = SavingData.instance.CurrentSavedState;
		
		if(SavingData.instance.saveLoadManager.savedInfos[0] == null)
		{
			SavingData.instance.saveLoadManager.savedInfos.Add(SavingData.instance.DefaultSaveState);
		}
	}
}
