using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
	[SerializeField]
	private string _nameSave = "save";                   // name of the save state
	[SerializeField]
	private SaveTime _timeOfSaveState;
	[SerializeField]
	private SavePlayerInfos _savePlayerInfos;
	[SerializeField]
	private SaveSettings _saveSettingsPlayer;
	[SerializeField]
	private bool isAutoSave = false;

	public PlayerData()	{}

	public PlayerData(SavePlayerInfos savePlayerInfos, SaveSettings saveSettings, SaveTime timeOfSaveState)
	{
		_savePlayerInfos = savePlayerInfos;
		_saveSettingsPlayer = saveSettings;
		_timeOfSaveState = timeOfSaveState;
	}

	public SavePlayerInfos SavePlayerInfos
	{
		get
		{
			return _savePlayerInfos;
		}

		set
		{
			_savePlayerInfos = value;
		}
	}

	public SaveSettings SaveSettingsPlayer
	{
		get
		{
			return _saveSettingsPlayer;
		}

		set
		{
			_saveSettingsPlayer = value;
		}
	}

	public string NameSave
	{
		get
		{
			return _nameSave;
		}

		set
		{
			_nameSave = value;
		}
	}

	public SaveTime TimeOfSaveState
	{
		get
		{
			return _timeOfSaveState;
		}

		set
		{
			_timeOfSaveState = value;
		}
	}

	public bool IsAutoSave
	{
		get
		{
			return isAutoSave;
		}

		set
		{
			isAutoSave = value;
		}
	}
}//class