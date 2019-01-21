using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
	public static UIPlayer instance;

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
}
