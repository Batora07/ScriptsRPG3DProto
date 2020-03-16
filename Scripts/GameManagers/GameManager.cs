using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public SceneName nameScene;
	public bool isDebugging = false;

	[SerializeField]
	private PlayerStatus playerStatus;

	[SerializeField]
	private PlayerInfos playerInfos;

	[SerializeField]
	private string characterName;

	[SerializeField]
	private int levelCharacter;

	[SerializeField]
	private GameObject[] characters;

	[SerializeField]
	private int selectedCharacterIndex;

	public GameObject playerInventory;

	public CameraHandler camHandler;

	private UIPlayer uiPlayer;


	void Awake () {
		MakeSingleton();
	}

	public void SetNameScene(SceneName _nameScene)
	{
		nameScene = _nameScene;
	}

	void OnEnable()
	{
		// we subscribe to the delegate /event
		SceneHandler.changedScene += ChangedScene;
		SceneManager.sceneLoaded += LevelFinishedLoading;
	}

	void OnDisable()
	{
		// we unsubscribe to the delegate/event
		SceneHandler.changedScene -= ChangedScene;
		SceneManager.sceneLoaded -= LevelFinishedLoading;
	}

	void MakeSingleton () {
		if(instance != null)
		{
			Destroy(gameObject);
		} else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void LevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		// in all scenes except MainMenu we spawn the character
		if(scene.name != "MainMenu")
		{
			// create an automatic save state
			if(SavingData.instance.CurrentSavedState.SavePlayerInfos.level == 0)
			{
				SavingData.instance.GenerateAutoSave.CreateAutoSave();
			}
			MainMenuController.instance.SetWindowMode(SavingData.instance.CurrentSavedState.SaveSettingsPlayer.windowMode == WindowMode.Windowed ? true : false);

			// ONCE THE SCENE IS LOADED, FIRST THING TO CHANGE IS THE SOUND
			SoundManager.instance.LoadAudioPrefs(SavingData.instance.CurrentSavedState);

			// INSTANTIATE THE UI CAMERA
			Instantiate(playerInventory, Vector3.zero, Quaternion.identity);

			// THEN WE INSTANTIATE THE PLAYER CHARACTER

			// Reset player infos to UI
			Vector3 pos = GameObject.FindGameObjectWithTag("SpawnPosition").transform.position;
			GameObject charInstantiated = Instantiate(characters[SelectedCharacterIndex], pos, Quaternion.identity);
			PlayerStatus _playerStatus = charInstantiated.GetComponent<PlayerStatus>();
			PlayerStatus = _playerStatus;
			PlayerStatus.SetupPlayerStatus();

			// SET THE NAME + LVL + HEALTH INFOS
			if(playerInfos.health > 0)
			{
				PlayerStatus.SetPlayerStatus(playerInfos);
			}
			
			if(UnitFrameManager.instance != null)
			{
				UnitFrameManager.instance.CompleteSetInfosUnitFrame();
			}

			// if the player pos has been changed
			if(playerInfos.playerPos.positionX > 0 ||
				playerInfos.playerPos.positionY > 0 ||
				playerInfos.playerPos.positionZ >0)
			{
				Vector3 newPos = new Vector3(playerInfos.playerPos.positionX, playerInfos.playerPos.positionY, playerInfos.playerPos.positionZ);
				charInstantiated.transform.position = newPos;
			}

			// if the player rotation has been changed
			if(playerInfos.playerPos.rotationX > 0 ||
				playerInfos.playerPos.rotationY > 0 ||
				playerInfos.playerPos.rotationZ > 0)
			{
				Vector3 newRot = new Vector3(playerInfos.playerPos.rotationX, playerInfos.playerPos.rotationY, playerInfos.playerPos.rotationZ);
				charInstantiated.transform.eulerAngles = newRot;
			}

			// if the player scale has been changed
			if(playerInfos.playerPos.scaleX > 0 ||
				playerInfos.playerPos.scaleY > 0 ||
				playerInfos.playerPos.scaleZ > 0)
			{
				Vector3 newScale = new Vector3(playerInfos.playerPos.scaleX, playerInfos.playerPos.scaleY, playerInfos.playerPos.scaleZ);
				charInstantiated.transform.localScale = newScale;
			}

			// FINALLY WE DO THE CAMERA SETTINGS
			// if the camera pos has been changed
			if(playerInfos.cameraPos.positionX > 0 ||
				playerInfos.cameraPos.positionY > 0 ||
				playerInfos.cameraPos.positionZ > 0)
			{
				Vector3 newCamPos = new Vector3(playerInfos.cameraPos.positionX, playerInfos.cameraPos.positionY, playerInfos.cameraPos.positionZ);
				Camera.main.transform.position = newCamPos;
			}

			// if the camera rotation has been changed
			if(playerInfos.cameraPos.rotationX > 0 ||
				playerInfos.cameraPos.rotationY > 0 ||
				playerInfos.cameraPos.rotationZ > 0)
			{
				Vector3 newCamRot = new Vector3(playerInfos.cameraPos.rotationX, playerInfos.cameraPos.rotationY, playerInfos.cameraPos.rotationZ);
				Camera.main.transform.eulerAngles = newCamRot;
			}

			// if the camera scale has been changed
			if(playerInfos.cameraPos.scaleX > 0 ||
				playerInfos.cameraPos.scaleY > 0 ||
				playerInfos.cameraPos.scaleZ > 0)
			{
				Vector3 newCamScale = new Vector3(playerInfos.cameraPos.scaleX, playerInfos.cameraPos.scaleY, playerInfos.cameraPos.scaleZ);
				Camera.main.transform.localScale = newCamScale;
			}

			// Set the entityInfos
			if(playerInfos.level > 0)
			{
				_playerStatus.GetComponent<EntityStatus>().SetupEntityInfosByPlayerInfos(playerInfos);
			}

			// finally we set the camera -> Field Of View
			if(Camera.main.fieldOfView != playerInfos.cameraFoV &&
				playerInfos.cameraFoV > 0)
			{
				Camera.main.fieldOfView = playerInfos.cameraFoV;
			}

			SavingData.instance.GenerateAutoSave.UpdateAutoSave();

			// then setup the sounds
			SoundManager.instance.ChangeSoundLibs();

			// disable the enemy frame panelUI at start of the scene
			PlayerStatus.instance.selectCharacter.UnselectEntity();
			EnemyUnitFrameManager.instance.panelUI.gameObject.SetActive(false);

			// once all of this is done then disable the loading screen
			SceneLoader.instance.DisableLoadingScreen();
		}
		else
		{
			// create an automatic save state
			if(SavingData.instance.CurrentSavedState.SavePlayerInfos.level == 0)
			{
				SavingData.instance.GenerateAutoSave.CreateAutoSave();
			}
		}
	}

	public void LoadSaveFile()
	{
		PlayerData saveFile = SavingData.instance.CurrentSavedState;

		// first of all, get the int of the character type 
		selectedCharacterIndex = (int)saveFile.SavePlayerInfos.typeCharacter;

		//Also we need to get the info about the player status 
		PlayerInfos loadedPlayerInfos = new PlayerInfos();
		loadedPlayerInfos.characterType = saveFile.SavePlayerInfos.typeCharacter;
		loadedPlayerInfos.level = saveFile.SavePlayerInfos.level;
		loadedPlayerInfos.maxHealth = saveFile.SavePlayerInfos.maxHP;
		loadedPlayerInfos.health = saveFile.SavePlayerInfos.currentHP;
		loadedPlayerInfos.maxMana = saveFile.SavePlayerInfos.maxMP;
		loadedPlayerInfos.mana = saveFile.SavePlayerInfos.currentMP;
		loadedPlayerInfos.characterName = saveFile.SavePlayerInfos.nameCharacter;
		// then where is his pos
		loadedPlayerInfos.playerPos = saveFile.SavePlayerInfos.currentPos;
		// then where is the cam pos & fov
		loadedPlayerInfos.cameraFoV = saveFile.SaveSettingsPlayer.cameraFOVZoom;
		loadedPlayerInfos.cameraPos = saveFile.SaveSettingsPlayer.cameraPosAtSave;

		playerInfos = loadedPlayerInfos;

		// before loading the scene, change the quality graphics and the resolution settings & shadowsEnabling
		MainMenuController.instance.SetGraphicsSettingsFromSaveFile(saveFile);

		// load the scene saved
		SceneLoader.instance.SetLoadingSprite(saveFile.SavePlayerInfos.currentScene.ToString());
		GameplayController gpController = new GameplayController();
		gpController.LoadWorld(saveFile.SavePlayerInfos.currentScene);
		// once the scene is loaded the event will call the LevelFinishedLoading fonction
	}

	public void ChangedScene(SceneName _nameScene)
	{
		nameScene = _nameScene;
	}


	public PlayerStatus PlayerStatus
	{
		get
		{
			return playerStatus;
		}

		set
		{
			playerStatus = value;
		}
	}

	public string CharacterName
	{
		get
		{
			return characterName;
		}

		set
		{
			characterName = value;
		}
	}

	public UIPlayer UiPlayer
	{
		get
		{
			return uiPlayer;
		}

		set
		{
			uiPlayer = value;
		}
	}

	public int SelectedCharacterIndex
	{
		get
		{
			return selectedCharacterIndex;
		}

		set
		{
			selectedCharacterIndex = value;
		}
	}

	public int LevelCharacter
	{
		get
		{
			return levelCharacter;
		}

		set
		{
			levelCharacter = value;
		}
	}

	public PlayerInfos PlayerInfos
	{
		get
		{
			return playerInfos;
		}

		set
		{
			playerInfos = value;
		}
	}
} // class
