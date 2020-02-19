using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogTab : MonoBehaviour
{
	public Sprite unselectedItem;
	public Sprite selectedItem;

	public Text questName;
	public Text lvlRequired;

	public Quest questInfos;

	private Button btn;

	public void Awake()
	{
		btn = GetComponent<Button>();
		btn.onClick.AddListener(DisplayQuestInfos);
	}

	public void DisplayQuestInfos()
	{
		btn.GetComponent<Image>().sprite = selectedItem;
		QuestLogDisplay.instance.ClearSelectedQuest();
		QuestLogDisplay.instance.DisplaySelectedQuest(questInfos);
	}

	public void Display(Quest quest)
	{
		questName.text = quest.questName;
		lvlRequired.text = quest.requiredLvl.ToString();
	}
}
