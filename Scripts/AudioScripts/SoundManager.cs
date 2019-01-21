using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public AudioObject[] listAudioTypes;			//Drag the audioObjects children
	public SoundObjects soundObject;                //The scriptable object containing all of the sounds

	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	public AudioMixer masterMixer;

	public float previousMasterValue = 0f;

	private bool _isSoundMuted = false;
	

	public bool IsSoundMuted
	{
		get
		{
			return _isSoundMuted;
		}

		set
		{
			_isSoundMuted = value;
		}
	}

	void Awake()
	{
		//Check if there is already an instance of SoundManager
		if(instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if(instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy(gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.	
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		LoadAudioPrefs();
	}

	public void PlayClip(AudioObject audioType, AudioType type, string clipName)
	{
		int nbSounds = audioType.sounds.Count;
		for(int i = 0; i < nbSounds; ++i)
		{
			if(audioType.sounds[i].audioType == type)
			{
				if(audioType.sounds[i].name == clipName)
				{
					AudioSource audioSrc = audioType.GetComponent<AudioSource>();
					audioSrc.clip = audioType.sounds[i].trackFile;

					//Play the clip.
					audioSrc.Play();
				}
			}
		}
	}

	public void SetMusicLvl(float musicLvl)
	{
		masterMixer.SetFloat("musicVolume", musicLvl);
	}

	public void SetMasterLvl(float masterLvl)
	{
		masterMixer.GetFloat("masterVolume", out previousMasterValue);
		masterMixer.SetFloat("masterVolume", masterLvl);
	}

	public void SetSFXLvl(float sfxLvl)
	{
		masterMixer.SetFloat("sfxVolume", sfxLvl);
	}

	public void SetAmbiantLvl(float ambiantLvl)
	{
		masterMixer.SetFloat("ambiantVolume", ambiantLvl);
	}

	public void SetVoiceLvl(float voiceLvl)
	{
		masterMixer.SetFloat("voiceVolume", voiceLvl);
	}

	public void MuteMasterSounds(bool mute)
	{
		if(mute)
		{
			masterMixer.GetFloat("masterVolume", out previousMasterValue);
			masterMixer.SetFloat("masterVolume", -80);
		}
		else
		{
			masterMixer.SetFloat("masterVolume", previousMasterValue);
		}
		_isSoundMuted = mute;
	}

	public void SetMasterSoundsDefaultDataSave()
	{
		SaveSettings defaultsaveSoundsSettings = SavingData.instance.DefaultSaveState.SaveSettingsPlayer;

		defaultsaveSoundsSettings = SaveSoundSettings(defaultsaveSoundsSettings);

		SavingData.instance.DefaultSaveState.TimeOfSaveState.UpdateSaveTime();

		SaveAudioPrefs(defaultsaveSoundsSettings);
	}

	public SaveSettings SaveSoundSettings(SaveSettings saveSettings)
	{
		saveSettings.isSoundMuted = _isSoundMuted;
		masterMixer.GetFloat("masterVolume", out saveSettings.soundMasterLvl);
		masterMixer.GetFloat("musicVolume", out saveSettings.soundMusicLvl);
		masterMixer.GetFloat("sfxVolume", out saveSettings.soundSFXLvl);
		masterMixer.GetFloat("ambiantVolume", out saveSettings.soundAmbiantLvl);
		masterMixer.GetFloat("voiceVolume", out saveSettings.soundVoiceLvl);

		return saveSettings;
	}

	/// <summary>
	/// Set the audio player prefs for next launch of the game
	/// </summary>
	/// <param name="savedSettings">The settings saved from user action coming from main menu panel settings</param>
	public void SaveAudioPrefs(SaveSettings savedSettings)
	{
		// if sound muted = true then int = 1 else = -1
		int boolSoundMuted = savedSettings.isSoundMuted ? 1 : -1;
		PlayerPrefs.SetInt(SavingData.PLAYERPREFS_IS_SOUND_MUTED, boolSoundMuted);
		PlayerPrefs.SetFloat(SavingData.PLAYERPREFS_SOUNDLVLMASTER, savedSettings.soundMasterLvl);
		PlayerPrefs.SetFloat(SavingData.PLAYERPREFS_SOUNDLVLMUSIC, savedSettings.soundMusicLvl);
		PlayerPrefs.SetFloat(SavingData.PLAYERPREFS_SOUNDLVLSFX, savedSettings.soundSFXLvl);
		PlayerPrefs.SetFloat(SavingData.PLAYERPREFS_SOUNDLVLAMBIANT, savedSettings.soundAmbiantLvl);
		PlayerPrefs.SetFloat(SavingData.PLAYERPREFS_SOUNDLVLVOICES, savedSettings.soundVoiceLvl);
	}

	/// <summary>
	/// Load the audio player prefs from the previous launch of the game
	/// </summary>
	/// <param name="savedSettings">The settings saved from user action coming from main menu panel settings</param>
	public void LoadAudioPrefs()
	{
		// try to set the audiomixer values at launch
		try
		{   // if int sound muted = 1 then bool = true else = false
			bool boolSoundMuted = PlayerPrefs.GetInt(SavingData.PLAYERPREFS_IS_SOUND_MUTED) == 1 ? true : false;
			SoundManager.instance.MuteMasterSounds(boolSoundMuted);

			SoundManager.instance.SetMasterLvl(PlayerPrefs.GetFloat(SavingData.PLAYERPREFS_SOUNDLVLMASTER));
			SoundManager.instance.SetMusicLvl(PlayerPrefs.GetFloat(SavingData.PLAYERPREFS_SOUNDLVLMUSIC));
			SoundManager.instance.SetSFXLvl(PlayerPrefs.GetFloat(SavingData.PLAYERPREFS_SOUNDLVLSFX));
			SoundManager.instance.SetAmbiantLvl(PlayerPrefs.GetFloat(SavingData.PLAYERPREFS_SOUNDLVLAMBIANT));
			SoundManager.instance.SetVoiceLvl(PlayerPrefs.GetFloat(SavingData.PLAYERPREFS_SOUNDLVLVOICES));
			
			/*SaveSettings defaultsaveSoundsSettings = SavingData.instance.DefaultSaveState.SaveSettingsPlayer;

			SoundManager.instance.MuteMasterSounds(defaultsaveSoundsSettings.isSoundMuted);

			SoundManager.instance.SetMasterLvl(defaultsaveSoundsSettings.soundMasterLvl);
			SoundManager.instance.SetMusicLvl(defaultsaveSoundsSettings.soundMusicLvl);
			SoundManager.instance.SetSFXLvl(defaultsaveSoundsSettings.soundSFXLvl);
			SoundManager.instance.SetAmbiantLvl(defaultsaveSoundsSettings.soundAmbiantLvl);
			SoundManager.instance.SetVoiceLvl(defaultsaveSoundsSettings.soundVoiceLvl);*/
		}
		// not found audio prefs ? then it's the first time
		catch(System.Exception e)
		{
			Debug.Log("First launch");
		}
	}

	/// <summary>
	/// Load the audio settings from a save file
	/// </summary>
	public void LoadAudioPrefs(PlayerData saveFile)
	{
		// try to set the audiomixer values at launch
		try
		{   // if int sound muted = 1 then bool = true else = false
			SaveSettings saveSoundsSettings = saveFile.SaveSettingsPlayer;

			SoundManager.instance.MuteMasterSounds(saveSoundsSettings.isSoundMuted);

			SoundManager.instance.SetMasterLvl(saveSoundsSettings.soundMasterLvl);
			SoundManager.instance.SetMusicLvl(saveSoundsSettings.soundMusicLvl);
			SoundManager.instance.SetSFXLvl(saveSoundsSettings.soundSFXLvl);
			SoundManager.instance.SetAmbiantLvl(saveSoundsSettings.soundAmbiantLvl);
			SoundManager.instance.SetVoiceLvl(saveSoundsSettings.soundVoiceLvl);
		}
		// not found audio prefs ? then it's the first time
		catch(System.Exception e)
		{
			Debug.Log("First launch");
		}
	}

	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx(AudioSource audioSource, List<AudioClip> clips)
	{
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Count);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		//Set the pitch of the audio source to the randomly chosen pitch.
		audioSource.pitch = randomPitch;

		//Set the clip to the clip at our randomly chosen index.
		audioSource.clip = clips[randomIndex];
		
		StartCoroutine(PlayRandomizeSound(audioSource.clip.length, audioSource));
	}

	public IEnumerator PlayRandomizeSound(float nbSeconds, AudioSource audioSource)
	{
		//Play the clip.
		audioSource.Play();
		yield return new WaitForSeconds(nbSeconds);
	}

	public void ChangeSoundLibs()
	{
		foreach(AudioObject audioObj in listAudioTypes)
		{
			audioObj.ChangeSoundLib();
		}
	}
}
