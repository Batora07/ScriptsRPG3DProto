using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSaveState
{
	private PlayerData dataSave;

	/// <summary>
	/// Create a player save state (not auto-save) & without a name
	/// </summary>
	public CreateSaveState()
	{
		CreateNewSave();
	}

	/// <summary>
	/// Create a save state with a name set by the player, it'll be easier to find out
	/// </summary>
	/// <param name="nameSave"></param>
	public CreateSaveState(string nameSave)
	{
		CreateNewSave(nameSave);
	}

	/// <summary>
	/// Create a save state, use this one if you want to create an auto save
	/// </summary>
	/// <param name="nameSave"></param>
	/// <param name="isAutoSave"></param>
	public CreateSaveState(string nameSave, bool isAutoSave)
	{
		CreateNewSave(nameSave, isAutoSave);
	}

	/// <summary>
	/// Create a Save state of the player and create a .dat file in the player's folder, 
	/// then add the saved file to the saveStates scriptable object
	/// </summary>
	/// <param name="saveName">input name of the save made by the player</param>
	/// <param name="isAutoSave">Is the current save an auto save (restricted mode, player can't delete this save)</param>
	private void CreateNewSave(string saveName = "", bool isAutoSave = false)
	{
		dataSave = new PlayerData();

		SavePlayerInfos savedPlayerInfos = new SavePlayerInfos();

		if(GameManager.instance.PlayerStatus != null)
		{
			PlayerStatus playerStatus = GameManager.instance.PlayerStatus;
			// Player settings
			savedPlayerInfos.nameCharacter = playerStatus.characterName;
			savedPlayerInfos.typeCharacter = playerStatus.characterType;
			savedPlayerInfos.level = playerStatus.level;
			// current character info
			savedPlayerInfos.currentHP = playerStatus.PlayerHealth.health;
			savedPlayerInfos.maxHP = playerStatus.PlayerHealth.maxHealth;
			savedPlayerInfos.currentMP = playerStatus.PlayerMana.mana;
			savedPlayerInfos.maxMP = playerStatus.PlayerMana.maxMana;
			// scene and pos of character at save time

			/****** CREATE TRANSFORM PLAYER: ****/

			savedPlayerInfos.currentPos = GetSaveTransformFromTransform(playerStatus.gameObject.transform);

		}

		savedPlayerInfos.currentScene = GameManager.instance.nameScene;

		SaveSettings savedSettings = new SaveSettings();
		// AUDIO SETTINGS
		savedSettings = SoundManager.instance.SaveSoundSettings(savedSettings);

		// GRAPHICS SETTINGS
		savedSettings.graphicsQuality = MainMenuController.instance.GraphicsQuality;
		savedSettings.graphicsResolution = MainMenuController.instance.GraphicsResolution;
		savedSettings.isShadowsEnabled = MainMenuController.instance.Shadows;

		// CAMERA SETTINGS
		savedSettings.cameraPosAtSave = GetSaveTransformFromTransform(Camera.main.transform);
		savedSettings.cameraFOVZoom = Camera.main.fieldOfView;

		SaveTime saveTime = new SaveTime();

		dataSave.IsAutoSave = isAutoSave;

		if(isAutoSave)
			saveName = "AUTO_SAVE";

		dataSave.NameSave = saveName;
		dataSave.SavePlayerInfos = savedPlayerInfos;
		dataSave.SaveSettingsPlayer = savedSettings;
		dataSave.TimeOfSaveState = saveTime;

		// if saveName is null, then take the currentTime By Default as a namefile
		saveName = string.IsNullOrEmpty(saveName) ? SavingData.instance.FormatDateStringToDatFile(saveTime) : saveName;

		// Set the current saved state as this one
		SavingData.instance.CurrentSavedState = dataSave;

		// this will then create a .dat file of the saved state and 
		// add it to the scriptable object afterwards if all goes well after the file is created
		SavingData.instance.SaveData();
	}
	
	public SaveTransform GetSaveTransformFromTransform(Transform transform)
	{
		SaveTransform saveTransform = new SaveTransform();

		// POSITION
		saveTransform.positionX = transform.position.x;
		saveTransform.positionY = transform.position.y;
		saveTransform.positionZ = transform.position.z;

		// ROTATION
		saveTransform.rotationX = transform.rotation.x;
		saveTransform.rotationY = transform.rotation.y;
		saveTransform.rotationZ = transform.rotation.z;

		// SCALE
		saveTransform.scaleX = transform.lossyScale.x;
		saveTransform.scaleY = transform.lossyScale.y;
		saveTransform.scaleZ = transform.lossyScale.z;

		return saveTransform;
	}
}
