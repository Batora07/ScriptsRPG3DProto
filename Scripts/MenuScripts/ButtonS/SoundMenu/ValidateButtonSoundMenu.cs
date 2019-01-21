using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValidateButtonSoundMenu : MonoBehaviour
{
	private Button btn;

   void Awake()
   {
		btn = GetComponent<Button>();
		btn.onClick.AddListener(SaveAudioChanges);
   }

	void OnDestroy()
	{
		btn.onClick.RemoveAllListeners();
	}

	private void SaveAudioChanges()
	{
		SoundManager.instance.SetMasterSoundsDefaultDataSave();
	}
}
