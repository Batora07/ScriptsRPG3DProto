using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownGraphicsResolution : MonoBehaviour
{
	public MainMenuController menuController;
	public Dropdown dropDownResolution;

	private void Awake()
	{
		dropDownResolution = GetComponent<Dropdown>();
	}

	private void OnEnable()
	{
		//Add listener for when the value of the Dropdown changes, to take action
		dropDownResolution.onValueChanged.AddListener(delegate {
			DropdownValueChanged(dropDownResolution);
		});
	}

	private void OnDisable()
	{
		// Remove Listener
		dropDownResolution.onValueChanged.RemoveListener(delegate
		{
			DropdownValueChanged(dropDownResolution);
		});
	}

	// Change the quality graphics
	void DropdownValueChanged(Dropdown change)
	{
		menuController.SetGraphicsResolution((GraphicsResolution)change.value);
	}
}
