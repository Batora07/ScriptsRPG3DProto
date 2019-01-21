using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerReferencer : MonoBehaviour
{
	private SoundManager soundManager;

	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	private Toggle toggle;
	private Slider slider;
	[SerializeField]
	private AudioType audioType;

	public void Awake()
	{
		soundManager = SoundManager.instance;
	}

	public void OnEnable()
	{
		if(gameObject.GetComponent<Toggle>() != null)
		{
			toggle = gameObject.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(delegate {
				SetValueToggle(toggle);
			});
		}

		if(gameObject.GetComponent<Slider>() != null)
		{
			slider = gameObject.GetComponent<Slider>();
			switch(audioType)
			{
				case AudioType.Ambient:
					slider.onValueChanged.AddListener(delegate { SetAmbiantVolume(); });
					break;
				case AudioType.Voice:
					slider.onValueChanged.AddListener(delegate { SetVoiceVolume(); });
					break;
				case AudioType.Music:
					slider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
					break;
				case AudioType.SFX:
					slider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
					break;
				case AudioType.Master:
					slider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
					break;
			}
		}
	}

	public void OnDisable()
	{
		if(toggle != null)
		{
			toggle.onValueChanged.RemoveListener(delegate
			{
				SetValueToggle(toggle);
			});
		}
	}

	public void SetValueToggle(Toggle value)
	{
		soundManager.MuteMasterSounds(value.isOn);
	}

	public void SetMasterVolume()
	{
		soundManager.SetMasterLvl(slider.value);
	}

	public void SetSFXVolume()
	{
		soundManager.SetSFXLvl(slider.value);
	}

	public void SetMusicVolume()
	{
		soundManager.SetMusicLvl(slider.value);
	}

	public void SetAmbiantVolume()
	{
		soundManager.SetAmbiantLvl(slider.value);
	}

	public void SetVoiceVolume()
	{
		soundManager.SetVoiceLvl(slider.value);
	}
}
