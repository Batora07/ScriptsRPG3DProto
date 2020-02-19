using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public GameObject buttonPanel, characterSelectPanel, createCharacterPanel, optionsPanel, loadPanel;

	public static MainMenuController instance;

	// GRAPHICS SETTINGS
	[SerializeField]
	private GraphicsQuality graphicsQuality;
	[SerializeField]
	private GraphicsResolution graphicsResolution;
	[SerializeField]
	private bool shadows = true;

	private MainMenuCamera mainMenuCamera;

	public bool Shadows
	{
		get
		{
			return shadows;
		}
	}

	public GraphicsResolution GraphicsResolution
	{
		get
		{
			return graphicsResolution;
		}
	}

	public GraphicsQuality GraphicsQuality
	{
		get
		{
			return graphicsQuality;
		}
	}

	void Awake () {
		mainMenuCamera = Camera.main.GetComponent<MainMenuCamera>();

		MakeSingleton();
	}
	
	public void PlayGame() {
		mainMenuCamera.ChangePosition(1);
		buttonPanel.SetActive(false);
		characterSelectPanel.SetActive(true);
	}

	public void BackToMainMenu()
	{
		mainMenuCamera.ChangePosition(0);
		buttonPanel.SetActive(true);
		characterSelectPanel.SetActive(false);
	}

	public void StartGame()
	{
		SceneLoader.instance.LoadLevelAsync("Village");
	}

	public void CreateCharacter()
	{
		characterSelectPanel.SetActive(false);
		createCharacterPanel.SetActive(true);
	}

	public void OpenLoadSaveMenu()
	{
		buttonPanel.SetActive(false);
		loadPanel.SetActive(true);
	}

	public void Accept()
	{
		characterSelectPanel.SetActive(true);
		createCharacterPanel.SetActive(false);
	}

	public void Cancel()
	{
		characterSelectPanel.SetActive(true);
		createCharacterPanel.SetActive(false);
	}

	public void OptionsPanel()
	{
		optionsPanel.SetActive(true);
		buttonPanel.SetActive(false);
	}

	public void CloseOptionsPanel()
	{
		optionsPanel.SetActive(false);
		buttonPanel.SetActive(true);
	}

	public void CloseLoadPanel()
	{
		loadPanel.SetActive(false);
		buttonPanel.SetActive(true);
	}

	public void SetQuality()
	{
		ChangeQualityLevel();
	}

	public void SetResolution()
	{
		ChangeResolution();
	}

	public void SetChangeGraphicsQuality(GraphicsQuality _quality)
	{
		graphicsQuality = _quality;
		SetQuality();
	}

	public void SetGraphicsResolution(GraphicsResolution _resolution)
	{
		graphicsResolution = _resolution;
		SetResolution();
	}

	public void SetShadows(bool _shadows)
	{
		shadows = _shadows;
		ChangeShadows();
	}

	private void ChangeQualityLevel()
	{
	//	string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

		switch (graphicsQuality)
		{
			case GraphicsQuality.Low:
				QualitySettings.SetQualityLevel(0);
				break;

			case GraphicsQuality.Normal:
				QualitySettings.SetQualityLevel(1);
				break;

			case GraphicsQuality.High:
				QualitySettings.SetQualityLevel(2);
				break;

			case GraphicsQuality.Ultra:
				QualitySettings.SetQualityLevel(3);
				break;
		}

		ChangeShadows();
	}

	private void ChangeShadows()
	{
		if(!shadows)
		{
			QualitySettings.shadows = ShadowQuality.Disable;

		}
		else
		{
			QualitySettings.shadows = ShadowQuality.All;
		}
	}

	private void ChangeResolution()
	{
		//string index = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

		switch (graphicsResolution)
		{
			case GraphicsResolution._1152x648:
				Screen.SetResolution(1152, 648, true);
				break;

			case GraphicsResolution._1280x720:
				Screen.SetResolution(1280, 720, true);
				break;

			case GraphicsResolution._1360x764:
				Screen.SetResolution(1360, 768, true);
				break;

			case GraphicsResolution._1920x1080:
				Screen.SetResolution(1920, 1080, true);
				break;
		}
	}

	public void SetGraphicsSettingsFromSaveFile(PlayerData saveFile)
	{
		SetChangeGraphicsQuality(saveFile.SaveSettingsPlayer.graphicsQuality);
		SetGraphicsResolution(saveFile.SaveSettingsPlayer.graphicsResolution);
		SetShadows(saveFile.SaveSettingsPlayer.isShadowsEnabled);
	}

	void MakeSingleton()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

} // class
