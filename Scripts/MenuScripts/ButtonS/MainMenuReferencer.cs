using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuReferencer : MonoBehaviour
{
	public MainMenuController menuController;

	private Toggle toggle;

	public void Awake()
	{
		menuController = MainMenuController.instance;
	}

	public void OnEnable()
	{
		if(gameObject.GetComponent<Toggle>() != null)
		{
			toggle = gameObject.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(delegate {
				SetValueToggle(toggle);
			});
		}
	}

	public void OnDisable()
	{
		if(toggle != null)
			toggle.onValueChanged.RemoveListener(delegate {
				SetValueToggle(toggle);
			}); 
	}

	public void SetValueToggle(Toggle value)
	{
		menuController.SetShadows(value.isOn);
	}
}
