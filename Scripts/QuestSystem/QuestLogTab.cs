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
		QuestLogTab[] otherQuests = this.transform.parent.GetComponentsInChildren<QuestLogTab>();
		foreach(QuestLogTab others in otherQuests)
		{
			others.GetComponent<Button>().GetComponent<Image>().sprite = unselectedItem;
		}
		
		btn.GetComponent<Image>().sprite = selectedItem;
		QuestLogDisplay.instance.ClearSelectedQuest();
		QuestLogDisplay.instance.DisplaySelectedQuest(questInfos);
		QuestLogDisplay.instance.currentQuestDisplayed = questInfos;
	}

	public void Display(Quest quest)
	{
		questName.text = quest.questName;
		lvlRequired.text = quest.requiredLvl.ToString();
	}
}
