using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuReferencer : MonoBehaviour
{
	public MainMenuController menuController;

	private Toggle toggle;

	public bool toggleShadows = true;
	public bool toggleFullscreen = false;

	public void Awake()
	{
		menuController = MainMenuController.instance;
	}

	public void OnEnable()
	{
		if(gameObject.GetComponent<Toggle>() != null)
		{
			toggle = gameObject.GetComponent<Toggle>();
			if(toggleShadows)
			{
				toggle.onValueChanged.AddListener(delegate
				{
					SetValueToggle(toggle);
				});
			}
			else if(toggleFullscreen)
			{
				toggle.onValueChanged.AddListener(delegate
				{
					SetValueToggleWindowMode(toggle);
				});
			}
		}
	}


	public void OnDisable()
	{
		if(toggle != null)
		{
			if(toggleShadows)
			{
				toggle.onValueChanged.RemoveListener(delegate
				{
					SetValueToggle(toggle);
				});
			}
			else if(toggleFullscreen)
			{
				toggle.onValueChanged.RemoveListener(delegate
				{
					SetValueToggleWindowMode(toggle);
				});
			}
		}
			
	}

	public void SetValueToggle(Toggle value)
	{
		menuController.SetShadows(value.isOn);
	}

	public void SetValueToggleWindowMode(Toggle value)
	{
		menuController.SetWindowMode(value.isOn);
	}
}
