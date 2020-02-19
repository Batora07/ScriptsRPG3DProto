using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour {

    public bool finished = false;
    public bool resetDissolve = false;
    public float speedDissolve = 0.1f;
    public Material mat;
	public Material outlineMat;


    public void OnEnable()
    {
		// try get mesh renderer, then change the outline shader effect by the dissolve shader effect
		if(GetComponent<SkinnedMeshRenderer>() != null)
		{
			GetComponent<SkinnedMeshRenderer>().material = mat;
		}
		else if(GetComponent<MeshRenderer>() != null)
		{
			GetComponent<MeshRenderer>().material = mat;
		}
		else
		{
			mat = null;
		}

		resetDissolve = true;
	}

	private void OnDisable()
	{
		// reset the outline shader 
		if(GetComponent<SkinnedMeshRenderer>() != null)
		{
			GetComponent<SkinnedMeshRenderer>().material = outlineMat;
		}
		else if(GetComponent<MeshRenderer>() != null)
		{
			GetComponent<MeshRenderer>().material = outlineMat;
		}
		else
		{
			mat = null;
		}
	}

	private void Update()
	{
		if(mat != null && !finished)
		{
			SetDissolve();
		}
		else if(finished && resetDissolve)
		{
			ResetDissolve();
		}
	}

    private void SetDissolve()
    {
		finished = false;
		float sliceAmount = mat.GetFloat("_SliceAmount");
        if (sliceAmount >= 1)
        {
            finished = true;
        }
        else
        {
            if(sliceAmount == 0)
            {
                sliceAmount = 0.1f;
            }
            sliceAmount += speedDissolve * Time.deltaTime;
            mat.SetFloat("_SliceAmount", sliceAmount);
        }
    }

    private void ResetDissolve()
    { 
        finished = false;
        mat.SetFloat("_SliceAmount", 0);
        resetDissolve = false;
    }
}
