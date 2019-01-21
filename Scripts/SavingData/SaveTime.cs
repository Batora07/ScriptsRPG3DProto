using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
[SerializeField]
public class SaveTime 
{
	public int day;
	public int month;
	public int year;
	public int hour;
	public int minute;
	public int seconds;

	public DateTime saveDateTime;

	public SaveTime()
	{
		saveDateTime = DateTime.Now;
		day = saveDateTime.Day;
		month = saveDateTime.Month;
		year = saveDateTime.Year;
		hour = saveDateTime.Hour;
		minute = saveDateTime.Minute;
		seconds = saveDateTime.Second;
	}

	public void UpdateSaveTime()
	{
		saveDateTime = DateTime.Now;
		day = saveDateTime.Day;
		month = saveDateTime.Month;
		year = saveDateTime.Year;
		hour = saveDateTime.Hour;
		minute = saveDateTime.Minute;
		seconds = saveDateTime.Second;

		//Debug.Log("day = " + day + " ; month = " + month + " ; year = " + year);
	}

	public string FormatDateString(PlayerData playerDataSaveState)
	{
		if(playerDataSaveState == null)
			return "";

		if(playerDataSaveState.TimeOfSaveState == null)
			return "";

		string day = playerDataSaveState.TimeOfSaveState.day < 10 ? "0" + playerDataSaveState.TimeOfSaveState.day.ToString() : playerDataSaveState.TimeOfSaveState.day.ToString();
		string month = playerDataSaveState.TimeOfSaveState.month < 10 ? "0" + playerDataSaveState.TimeOfSaveState.month.ToString() : playerDataSaveState.TimeOfSaveState.month.ToString();
		string hour = playerDataSaveState.TimeOfSaveState.hour < 10 ? "0" + playerDataSaveState.TimeOfSaveState.hour.ToString() : playerDataSaveState.TimeOfSaveState.hour.ToString();
		string minute = playerDataSaveState.TimeOfSaveState.minute < 10 ? "0" + playerDataSaveState.TimeOfSaveState.minute.ToString() : playerDataSaveState.TimeOfSaveState.minute.ToString();

		return day + "/" + month + "/" + playerDataSaveState.TimeOfSaveState.year + " - " + hour + ":" + minute;
	}
}
