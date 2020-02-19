using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
	public AudioType audioType;
	public List<Sounds> sounds = new List<Sounds>();
	public AudioClip clipToPlay;
	private AudioSource audioSource;

	public void Start()
	{
		ChangeSoundLib();
	}

	public void GenerateSoundLists()
	{
		sounds.Clear();
		int countSounds = SoundManager.instance.soundObject.sounds.Length;
		for(int i = 0; i < countSounds; ++i)
		{
			if(SoundManager.instance.soundObject.sounds[i].scene == GameManager.instance.nameScene)
			{
				int nbSounds = SoundManager.instance.soundObject.sounds[i].sounds.Length;

				for(int j = 0; j < nbSounds; ++j)
				{
					if(SoundManager.instance.soundObject.sounds[i].sounds[j].audioType == audioType) {
						sounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j]);

						try
						{
							if(SoundManager.instance.soundObject.sounds[i].sounds[j].playOnAwake)
							{
								clipToPlay = SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile;
								audioSource.playOnAwake = true;
								audioSource.loop = SoundManager.instance.soundObject.sounds[i].sounds[j].loop;
								// peculiar volume to set ?
								if(SoundManager.instance.soundObject.sounds[i].sounds[j].audioVolume > -100.0f)
								{
									SetSoundVolume(SoundManager.instance.soundObject.sounds[i].sounds[j].audioVolume);
								}
								PlayClip();
							}
						}
						catch(System.Exception e)
						{
							Debug.LogError(e);
						}
						
					}
				}
			}
		}
	}

	public void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		SceneHandler.changeSoundLib += ChangeSoundLib;
	}

	public void PlayClip()
	{
		audioSource.clip = clipToPlay;
		audioSource.Play();
	}

	public void OnDestroy()
	{
		SceneHandler.changeSoundLib -= ChangeSoundLib;
	}
	
	public void ChangeSoundLib()
	{
		ClearCurrentSounds();
		GenerateSoundLists();
	}

	public void ClearCurrentSounds()
	{
		clipToPlay = null;
		audioSource.Stop();
	}

	public void SetSoundVolume(float volumeLevel)
	{
		// then change the proper sound volume
		int nbSoundsManagers = SoundManager.instance.listAudioTypes.Length;
		for(int i = 0; i < nbSoundsManagers; ++i)
		{
			if(audioType == SoundManager.instance.listAudioTypes[i].audioType)
			{
				switch(audioType)
				{
					case AudioType.Ambient:
						SoundManager.instance.SetAmbiantLvl(volumeLevel);
						break;
					case AudioType.Master:
						SoundManager.instance.SetMasterLvl(volumeLevel);
						break;
					case AudioType.Music:
						SoundManager.instance.SetMusicLvl(volumeLevel);
						break;
					case AudioType.SFX:
						SoundManager.instance.SetSFXLvl(volumeLevel);
						break;
					case AudioType.Voice:
						SoundManager.instance.SetVoiceLvl(volumeLevel);
						break;
				}
			}
		}
	}
}
