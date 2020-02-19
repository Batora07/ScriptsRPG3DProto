using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class LoadFileListing : MonoBehaviour
{
	public List<PlayerData> listPlayerData = new List<PlayerData>();

	private PlayerData autoSave;
	private string fullPath = "";
	private string path = "/saves";

	public delegate void UpdateSaveFilesListsEvent();
	public static event UpdateSaveFilesListsEvent updatedSaveList;

	public void GetSaveFiles()
	{
		// first of all, set the default save available in the scriptable oject
		// as the first element in listPlayerData 		
		listPlayerData.Clear();

		if((SavingData.instance.saveLoadManager.savedInfos[0] != null))
		{
			if(SavingData.instance.saveLoadManager.savedInfos[0].IsAutoSave)
			{
				autoSave = SavingData.instance.saveLoadManager.savedInfos[0];
			}
		}

		fullPath = Application.persistentDataPath + path;
		int fCount = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories).Length;
		string[] pathNames = Directory.GetFiles(fullPath);
		string[] fileNames = new string[fCount];
		
		// get the files names
		for(int i = 0; i < fCount; ++i)
		{
			string fileName = GetNameFileFromPath(pathNames[i], '\\');
			Debug.Log(fileName);
			fileNames[i] = fileName;
		}

		// process each file as a PlayerData (load file then create a new PlayerData) 
		int nbFiles = fileNames.Length;
		for(int i = 0; i < nbFiles; ++i)
		{
			PlayerData newSaveFile = SavingData.instance.LoadData("/saves/" + fileNames[i]);
			if(string.IsNullOrEmpty(newSaveFile.NameSave))
			{
				newSaveFile.NameSave = GetNameFileFromPath(fileNames[i], '.', false);
				Debug.Log(newSaveFile.NameSave);
			}
			// skip this part of the loop if the save is an autosave
		/*	if(newSaveFile.IsAutoSave)
				continue;*/

			listPlayerData.Add(newSaveFile);
		}
		listPlayerData = ReorderListByTimeSaved(listPlayerData);
		ProcessSaveFilesToScriptableObj(listPlayerData, SavingData.instance.saveLoadManager.savedInfos);
	}

	/// <summary>
	/// Get the proper name from a full path of string
	/// </summary>
	/// <param name="path">The full string on which we want to get the right substring</param>
	/// <param name="charSelect">the char that determiens if we want to get the left or right side of this substring</param>
	/// <param name="right"> should we check the right side of the string path ? Default bool = true</param>
	/// <returns></returns>
	private string GetNameFileFromPath(string path, char charSelect, bool right = true)
	{
		string result = path;
		int nbChar = path.Length - 1;
		int countStringToRemove = 0;

		for(int i = nbChar; i >= 0; i--)
		{
			// we need to prevent player to put this kind of char in the name of his save
			// we also need to prevent him to put a " " blank char in his saveState name
			// TODO
			if(result[i] == charSelect)
			{
				countStringToRemove = i;
				break;
			}
		}
		// 
		result = right==true ? result.Substring(countStringToRemove + 1) : result.Substring(0, result.Length-countStringToRemove);

		return result;
	}

	public void ProcessSaveFilesToScriptableObj(List<PlayerData> playerDatas, List<PlayerData> scriptObjData)
	{
		// remove all elements of scriptableObject except for AutoSave
		int nbScriptOjbData = scriptObjData.Count;

		scriptObjData.RemoveAt(0);
		scriptObjData = ReorderListByTimeSaved(scriptObjData);
		scriptObjData.Insert(0, autoSave);
		playerDatas.Insert(0, autoSave);
	}

	public List<PlayerData> ReorderListByTimeSaved(List<PlayerData> listToReorder, bool isAscending = false)
	{
		// descending
		if(!isAscending)
		{
			listToReorder.Sort((a, b) => b.TimeOfSaveState.saveDateTime.CompareTo(a.TimeOfSaveState.saveDateTime));
			return listToReorder;
		}
		// ascending
		else
		{
			listToReorder.Sort((a, b) => a.TimeOfSaveState.saveDateTime.CompareTo(b.TimeOfSaveState.saveDateTime));
			return listToReorder;
		}
	}


	public void UpdatedSaveList()
	{
		if(updatedSaveList != null)
		{
			updatedSaveList();
		}
	}
}
