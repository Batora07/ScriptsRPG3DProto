using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNPC : MonoBehaviour
{
	public Material mat;
	public Projector projector;
//	public Color tint;

	public void OnEnable()
	{
		mat = projector.material;
	}

	public void SetColorRadius(Color newTint)
	{
		mat.SetColor("_Color", newTint);
	}
}
