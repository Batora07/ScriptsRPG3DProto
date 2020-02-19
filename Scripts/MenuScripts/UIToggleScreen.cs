using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleScreen : MonoBehaviour
{
	public bool isDisplayed = false;
	public Transform panelToDisplay;

	public void ToggleView()
	{
		if(panelToDisplay != null)
		{
			panelToDisplay.gameObject.SetActive(!panelToDisplay.gameObject.activeSelf);
		}
	}
}
