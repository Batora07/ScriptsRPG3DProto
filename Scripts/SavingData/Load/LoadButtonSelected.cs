using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonSelected : MonoBehaviour
{
	private Button btn;
	public AudioClip soundClickSave;
	public AudioClip soundClickLoad;

	[SerializeField]
	private LoadMenu loadMenu;

	private void Awake()
	{
		btn = GetComponent<Button>();
	}

	private void OnEnable()
	{
		if(loadMenu._isLoadMenu)
		{
			btn.onClick.AddListener(LoadSaveFile);
		}
		else
		{
			btn.onClick.AddListener(SaveButtonClick);
		}
	}

	private void OnDisable()
	{
		btn.onClick.RemoveAllListeners();
	}

	private void LoadSaveFile()
	{
		if(SavingData.instance.CurrentSavedState.SavePlayerInfos.level > 0)
		{
			PlaySoundClick(soundClickLoad);
			GameManager.instance.LoadSaveFile();
		}
	}

	private void SaveButtonClick()
	{
		PlaySoundClick(soundClickSave);
	}

	private void PlaySoundClick(AudioClip soundClick)
	{
		if(soundClick == null)
			return;

		int nbAudioSource = SoundManager.instance.listAudioTypes.Length;
		for(int i = 0; i < nbAudioSource; ++i)
		{
			if(SoundManager.instance.listAudioTypes[i].audioType == AudioType.SFX)
			{
				SoundManager.instance.listAudioTypes[i].clipToPlay = soundClick;
				SoundManager.instance.listAudioTypes[i].PlayClip();
			}
		}

	}
}
