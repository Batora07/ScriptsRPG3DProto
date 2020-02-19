using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
	public static LocalizationManager instance;
	public LocaleLangage langage;

	void Awake()
	{
		MakeSingleton();
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
			DontDestroyOnLoad(gameObject);
		}
	}
}
