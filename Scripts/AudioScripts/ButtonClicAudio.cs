using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicAudio : MonoBehaviour
{
	public AudioClip soundClick;
	private Button btn;

	private void Awake()
	{
		btn = GetComponent<Button>();
	}

	private void OnEnable()
	{
		btn.onClick.AddListener(PlaySoundClick);
	}

	private void OnDisable()
	{
		btn.onClick.RemoveListener(PlaySoundClick);
	}

	private void PlaySoundClick()
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
