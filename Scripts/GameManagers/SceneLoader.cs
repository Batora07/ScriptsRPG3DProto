using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public static SceneLoader instance;

	[SerializeField]
	private GameObject loadingScreen;
	[SerializeField]
	private Image loadingBar;
	[SerializeField]
	private Sprite defaultImage;
	[SerializeField]
	private Sprite orcWorldImage;
	[SerializeField]
	private Image loadingSpriteContainer;

	private float progressValue = 1.1f;
	public float progressMultiplier_1 = 0.3f;
	public float progressMultiplier_2 = 0.03f;

	public float loadLevelTime = 2.5f;

	private string levelName;

	private void Awake()
	{
		MakeSingleton();
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	private void Update()
	{
		ShowLoadingScreen();
	}

	void MakeSingleton()
	{
		// we have a duplicate
		if (instance != null)
		{
			Destroy(gameObject);
		} else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadLevel(string name)
	{
		levelName = name;
		StartCoroutine(LoadLevelWithName());
	}

	IEnumerator LoadLevelWithName()
	{
		loadingScreen.SetActive(true);
		progressValue = 0;
		// pauses the game to load another level
		Time.timeScale = 0f;

		//SceneManager.LoadScene(levelName);
		yield return new WaitForSeconds(loadLevelTime);
		StartCoroutine(LoadLevelAsynchronously(levelName));
		//loadingScreen.SetActive(false);
	}
	
	public void ShowLoadingScreen()
	{
		if(progressValue < 1f)
		{
			progressValue += progressMultiplier_1 * progressMultiplier_2;
			loadingBar.fillAmount = progressValue;

			// the loading bar has finished
			if(progressValue >= 1f)
			{
				progressValue = 1.1f;

				loadingBar.fillAmount = 0f;
				//loadingScreen.SetActive(false);

				// Unpause the game
				Time.timeScale = 1f;
			}
		}
	}

	public void DisableLoadingScreen()
	{
		loadingScreen.SetActive(false);
	}

	public void LoadLevelAsync(string levelName)
	{
		SetLoadingSprite(levelName);
		LoadLevel(levelName);
		// -- loads level too fast --
		//StartCoroutine(LoadLevelAsynchronously(levelName));
	}

	IEnumerator LoadLevelAsynchronously(string levelName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

		loadingScreen.SetActive(true);

		// while the operation is not done
		while(!operation.isDone)
		{
			float progress = operation.progress / 0.9f;
			loadingBar.fillAmount = progress;

			if(progress >= 1f)
			{
				//loadingScreen.SetActive(false);
			}

			yield return null;
		}
	}

	public void SetLoadingSprite(string levelName)
	{
		if(levelName == "OrcWorld")
		{
			loadingSpriteContainer.sprite = orcWorldImage;
		}
		else
		{
			loadingSpriteContainer.sprite = defaultImage;
		}
	}

} // class
