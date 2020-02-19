using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettingsArray : MonoBehaviour
{
	public ButtonSettings[] buttonSettingsArray;

	public void OnEnable()
	{
		buttonSettingsArray = gameObject.GetComponentsInChildren<ButtonSettings>();
	}
}
