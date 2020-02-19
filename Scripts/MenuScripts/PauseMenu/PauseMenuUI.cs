using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
	public static PauseMenuUI instance;
	public Transform pauseMenu;

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	public void EnablePauseMenu()
	{
		pauseMenu.gameObject.SetActive(true);
	}
}
