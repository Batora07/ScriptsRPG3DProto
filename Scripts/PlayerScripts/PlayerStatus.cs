using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	public GameObject[] playerSwords;

	private GameObject itemsPanel;

	void Start () {
		GameObject[] btns = GameObject.FindGameObjectsWithTag("SwordBtn");
		foreach(GameObject btn in btns)
		{
			btn.GetComponent<Button>().onClick.AddListener(ChangeSword);
		}

		itemsPanel = GameObject.Find("Items Panel");
		itemsPanel.SetActive(false);

		GameObject.Find("Item").GetComponent<Button>().onClick.AddListener(ActivateItemsPanel);
	}
	
	public void ActivateItemsPanel()
	{
		if (itemsPanel.activeInHierarchy)
		{
			itemsPanel.SetActive(false);
		} else
		{
			itemsPanel.SetActive(true);
		}
	}

	public void ChangeSword()
	{
		int swordIndex = int.Parse(
			UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

		for (int i = 0; i < playerSwords.Length; i++)
		{
			playerSwords[i].SetActive(false);
		}
		// for a game with multiple items, we need to save the index in the array
		// of the item we need with aa game controller e.G, so that
		// we don't have to get this for loop 
		playerSwords[swordIndex].SetActive(true);
	}
}
