using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavingData : MonoBehaviour
{
	public static SavingData instance;
	public SaveLoad saveLoadManager;
	[SerializeField]
	private PlayerData currentSavedState;
	[SerializeField]
	private PlayerData defaultSaveState;
	[SerializeField]
	private LoadFileListing loadFilesListing;

	private GenerateAutoSave generateAutoSave;

	private string DATA_PATH = "/saves/MyGame.dat";

	/* --  PlayerPrefs strings -- */

	// AUDIO SETTINGS
	public const string PLAYERPREFS_IS_SOUND_MUTED = "SOUND_MUTED";
	public const string PLAYERPREFS_SOUNDLVLMASTER = "SOUND_LVL_MASTER";
	public const string PLAYERPREFS_SOUNDLVLMUSIC = "SOUND_LVL_MUSIC";
	public const string PLAYERPREFS_SOUNDLVLSFX = "SOUND_LVL_SFX";
	public const string PLAYERPREFS_SOUNDLVLAMBIANT = "SOUND_LVL_AMBIANT";
	public const string PLAYERPREFS_SOUNDLVLVOICES = "SOUND_LVL_VOICES";

	private void Awake()
	{
		MakeSingleton();
		// load the default save state, if there isn't one, it should create a new saved state
		LoadDefaultSaveState();
	}

	public PlayerData DefaultSaveState
	{
		get
		{
			return defaultSaveState;
		}

		set
		{
			defaultSaveState = value;
		}
	}

	public PlayerData CurrentSavedState
	{
		get
		{
			return currentSavedState;
		}

		set
		{
			currentSavedState = value;
		}
	}

	public LoadFileListing LoadFilesListing
	{
		get
		{
			return loadFilesListing;
		}

		set
		{
			loadFilesListing = value;
		}
	}

	public GenerateAutoSave GenerateAutoSave
	{
		get
		{
			return generateAutoSave;
		}

		set
		{
			generateAutoSave = value;
		}
	}

	/// <summary>
	/// Use this to save the data from the player to a .dat file, binary encrypted
	/// </summary>
	public void SaveData()
	{
		FileStream file = null;
		
		try
		{
			BinaryFormatter bf = new BinaryFormatter();

			if(!string.IsNullOrEmpty(currentSavedState.NameSave))
			{
				DATA_PATH = "/saves/" + currentSavedState.NameSave + ".dat";
			}
			else if (currentSavedState.IsAutoSave)
			{
				DATA_PATH = "/saves/" + "AUTO_SAVE" + ".dat";
			}
			else
			{
				DATA_PATH = "/saves/" + FormatDateStringToDatFile(currentSavedState.TimeOfSaveState) + ".dat";
			}

			Debug.Log("save file available at : " + Application.persistentDataPath + DATA_PATH);

			// create the directory if not exist, do nothing if already exists
			System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/saves");

			file = File.Create(Application.persistentDataPath + DATA_PATH);
			// encrypt and save the data
			bf.Serialize(file, currentSavedState);

		}
		catch(Exception e)
		{
			if(e != null)
			{
				Debug.LogError("File not created, error with the player data ?");
				Debug.LogError("Error : " + e);
			}
		}
		finally
		{
			if(file != null)
			{
				file.Close();

				// then add this file to the scriptable object saveStates if not AutoSave
				if(!currentSavedState.IsAutoSave)
					saveLoadManager.savedInfos.Add(currentSavedState);
			}
		}
	} // save data

	/// <summary>
	/// Use this to load the data from a .dat file, binary encrypted, this method will decrypt the file and put 
	/// it in a PlayerData object
	/// </summary>
	public PlayerData LoadData(string dataPath = "")
	{
		FileStream file = null;
		PlayerData playerData = new PlayerData();
		try
		{
			BinaryFormatter bf = new BinaryFormatter();

			// if no dataPath set then take the default one
			if(string.IsNullOrEmpty(dataPath))
			{
				dataPath = DATA_PATH;
			}
			file = File.Open(Application.persistentDataPath + dataPath, FileMode.Open);
			// decrypting and loading the data
			playerData = bf.Deserialize(file) as PlayerData;
		} catch (Exception e)
		{
			Debug.LogError("Failed to load the save");
			Debug.LogError(e);
		}
		finally
		{
			if(file != null)
			{
				file.Close();
			}
		}
		return playerData;
	} // load data

	void MakeSingleton()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			generateAutoSave = new GenerateAutoSave();
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadDefaultSaveState()
	{
		loadFilesListing.GetSaveFiles();
		defaultSaveState = saveLoadManager.savedInfos[0];
		if(defaultSaveState == null)
		{
			if(saveLoadManager.savedInfos.Count == 0)
			{
				PlayerData newDefaultSaveState = new PlayerData();
				saveLoadManager.savedInfos.Add(newDefaultSaveState);
			}
		}
	}

	public string FormatDateStringToDatFile(SaveTime saveTime)
	{
		string day = saveTime.day < 10 ? "0" + saveTime.day.ToString() : saveTime.day.ToString();
		string month = saveTime.month < 10 ? "0" + saveTime.month.ToString() : saveTime.month.ToString();
		string hour = saveTime.hour < 10 ? "0" + saveTime.hour.ToString() : saveTime.hour.ToString();
		string minute = saveTime.minute < 10 ? "0" + saveTime.minute.ToString() : saveTime.minute.ToString();

		return day + "_" + month + "_" + saveTime.year + "-" + hour + "_" + minute + "_" + saveTime.seconds;
	}
}// class