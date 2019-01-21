using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSigil : MonoBehaviour
{
	public Image[] characterSigils;

	[SerializeField]
	private Button btn;

	private int selectedCharacter = 0;

	private void Awake()
	{
		btn.onClick.AddListener(ChangeSelectedCharacterIndex);
	}

	private void OnDestroy()
	{
		btn.onClick.RemoveListener(ChangeSelectedCharacterIndex);
	}

	public void OnEnable()
	{
		ChangeSelectedCharacterIndex();
		GetSigilSelectedCharacter();
	}

	private void ChangeSelectedCharacterIndex()
	{
		selectedCharacter = GameManager.instance.SelectedCharacterIndex;
	}

	public void GetSigilSelectedCharacter()
	{ 
		for(int i = 0; i < characterSigils.Length; ++i)
		{
			characterSigils[i].gameObject.SetActive(false);
		}

		characterSigils[selectedCharacter].gameObject.SetActive(true);
	}
}
