using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[SerializeField]
	private GameObject[] characters;

	[HideInInspector]
	public int selectedCharacterIndex;

	public GameObject playerInventory;

	void Awake () {
		MakeSingleton();
	}

	void OnEnable()
	{
		// we subscribe to the delegate /event
		SceneManager.sceneLoaded += LevelFinishedLoading;
	}

	void OnDisable()
	{
		// we unsubscribe to the delegate/event
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
			Instantiate(playerInventory, Vector3.zero, Quaternion.identity);

			Vector3 pos = GameObject.FindGameObjectWithTag("SpawnPosition").transform.position;
			Instantiate(characters[selectedCharacterIndex], pos, Quaternion.identity);
		}
	}

} // class
