using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelsArray : MonoBehaviour
{
	public SettingsPanel[] settingsPanelArray;

	public void OnEnable()
	{
		settingsPanelArray = gameObject.GetComponentsInChildren<SettingsPanel>();
	}
}
