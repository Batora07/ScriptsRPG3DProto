using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownGraphicsQuality : MonoBehaviour
{
	private MainMenuController menuController;
	private Dropdown dropdownQuality;

	private void Awake()
	{
		dropdownQuality = GetComponent<Dropdown>();
		menuController = MainMenuController.instance;
	}

	private void OnEnable()
	{
		//Add listener for when the value of the Dropdown changes, to take action
		dropdownQuality.onValueChanged.AddListener(delegate {
			DropdownValueChanged(dropdownQuality);
		});
	}

	private void OnDisable()
	{
		// Remove Listener
		dropdownQuality.onValueChanged.RemoveListener(delegate
		{
			DropdownValueChanged(dropdownQuality);
		});
	}

	// Change the quality graphics
	void DropdownValueChanged(Dropdown change)
	{
		menuController.SetChangeGraphicsQuality((GraphicsQuality)change.value);
	}
}
