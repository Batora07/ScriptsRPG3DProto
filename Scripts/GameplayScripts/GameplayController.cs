using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

	public void LoadOtherWorld()
	{
		string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
		SceneLoader.instance.LoadLevelAsync(name);
	}

	public void LoadWorld(SceneName nameScene)
	{
		string scene = "";
		switch(nameScene)
		{
			case SceneName.MainMenu:
				scene = "MainMenu";
				break;
			case SceneName.OrcWorld:
				scene = "OrcWorld";
				break;
			case SceneName.WolfWorld:
				scene = "WolfWorld";
				break;
			case SceneName.Town:
				scene = "Village";
				break;
			case SceneName.SkyWorld:
				scene = "SkyWorld";
				break;
		}

		SceneLoader.instance.SetLoadingSprite(scene);

		SceneLoader.instance.LoadLevelAsync(scene);
	}
}
