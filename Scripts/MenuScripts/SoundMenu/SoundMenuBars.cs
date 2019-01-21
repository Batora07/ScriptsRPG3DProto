using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMenuBars : MonoBehaviour
{
	public Slider sliderBar;

	public AudioType audioType;

	public void OnEnable()
	{
		SetupProperFillGauge();
	}

	public void SetupProperFillGauge()
	{
		SaveSettings defaultSettings = SavingData.instance.DefaultSaveState.SaveSettingsPlayer;
		switch(audioType)
		{
			case AudioType.Master:
				sliderBar.value = defaultSettings.soundMasterLvl;
				break;
			case AudioType.Music:
				sliderBar.value = defaultSettings.soundMusicLvl;
				break;
			case AudioType.SFX:
				sliderBar.value = defaultSettings.soundSFXLvl;
				break;
			case AudioType.Ambient:
				sliderBar.value = defaultSettings.soundAmbiantLvl;
				break;
			case AudioType.Voice:
				sliderBar.value = defaultSettings.soundVoiceLvl;
				break;
		}
	}
}
