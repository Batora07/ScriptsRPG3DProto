using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetName : MonoBehaviour
{
	public InputField nameInputField;
	
	public void SetNameGameManager()
	{
		GameManager.instance.CharacterName = nameInputField.text;
	}
}
