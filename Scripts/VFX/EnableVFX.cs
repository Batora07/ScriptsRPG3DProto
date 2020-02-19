using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableVFX : MonoBehaviour
{
	public Transform VFX;

	public void DisplayVFX()
	{
		VFX.gameObject.SetActive(true);
	}

	public void DisableVFX()
	{
		VFX.gameObject.SetActive(false);
	}
}
