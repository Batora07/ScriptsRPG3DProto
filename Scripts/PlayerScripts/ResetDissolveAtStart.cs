using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDissolveAtStart : MonoBehaviour
{
    void OnEnable()
	{
		if(GetComponent<SkinnedMeshRenderer>() != null)
			GetComponent<SkinnedMeshRenderer>().material.SetFloat("_SliceAmount", 0);
		if(GetComponent<MeshRenderer>() != null)
			GetComponent<MeshRenderer>().material.SetFloat("_SliceAmount", 0);
	}
}
