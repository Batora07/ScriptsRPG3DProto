using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
	public SceneName nameScene;

	public delegate void ChangedScene(SceneName newScene);
	public static event ChangedScene changedScene;

	public delegate void ChangeSoundLibrary();
	public static event ChangeSoundLibrary changeSoundLib;

	public void ChangeScene(SceneName _newScene)
	{
		// useful to prevent nullref in the case no object is subscribed to this event
		if(changedScene != null)
		{
			changedScene(_newScene);
		}
	}

	public void ChangeSoundLib()
	{
		if(changeSoundLib != null)
		{
			changeSoundLib();
		}
	}

	public void OnEnable()
	{
		ChangeScene(nameScene);
		ChangeSoundLib();
	}
}
