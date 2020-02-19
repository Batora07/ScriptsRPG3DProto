using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
	public static QuestsManager instance;
	public Quests questsScriptableObject;
	public Quest _currentQuest;

	public delegate void UpdateCurrentQuest();
	public static event UpdateCurrentQuest currentQuestUpdated;	

	void Awake()
	{
		MakeSingleton();
		SubscribeToEvents();
		CurrentQuestUpdated();
	}

	void MakeSingleton()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			FillQuestsDatabaseFromXML();
			DontDestroyOnLoad(gameObject);
		}
	}

	public void FillQuestsDatabaseFromXML()
	{
		// Fill the xml quests db from file
		XMLManager.instance.LoadQuestItems();
		QuestsBindingFromXMLDB(XMLManager.instance.questsDB.list);
	}

	public void SubscribeToEvents()
	{
		int nbTotalQuests = questsScriptableObject.questsLists.Length;

		for(int i = 0; i < nbTotalQuests; ++i)
		{
			foreach(KillQuest qL in questsScriptableObject.questsLists[i].killQuests)
			{
				if(qL.questStatus == QuestStatus.InProgress)
				{
					qL.InProgressQuest();
					ChangeCurrentQuest(qL);
				}
				else if(qL.questStatus == QuestStatus.Unlock)
				{
					qL.InProgressQuest();
				}
				else if(qL.questStatus == QuestStatus.Locked)
				{
					qL.LockQuest();
					qL.SubscribeEventLock();
				}
				qL.SetRequirements();
			}
		}		
	}

	public void ChangeCurrentQuest(Quest quest)
	{
		// <------------- TODO ----------------->
		/*
		 * DISPLAY ANIMATION WHEN QUEST IS FINISHED, also change text of the quest to display the end text short description
		 * */
		_currentQuest = quest;
		CurrentQuestUpdated();
	}

	public void QuestsBindingFromXMLDB(List<Quest> dbQuestsDeserialized)
	{
		int nblists = questsScriptableObject.questsLists.Length;
		for(int i = 0; i < nblists; ++i)
		{
			List<KillQuest> listKillQuest = new List<KillQuest>();
			foreach(Quest qDB in dbQuestsDeserialized)
			{			
				if(qDB.questType == QuestType.Kill)
				{
					if(questsScriptableObject.questsLists[i].questStatus == qDB.questStatus)
					{
						if(string.IsNullOrEmpty(questsScriptableObject.questsLists[i].name))
						{
							questsScriptableObject.questsLists[i].name = qDB.questStatus.ToString();
						}

						listKillQuest.Add(new KillQuest(qDB));						
					}
				}
			}
			questsScriptableObject.questsLists[i].killQuests = listKillQuest.ToArray();
		}
	}

	public void CurrentQuestUpdated()
	{
		if(currentQuestUpdated != null)
		{
			currentQuestUpdated();
		}
	}

}
