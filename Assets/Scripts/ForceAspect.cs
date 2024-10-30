using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ForceAspect : MonoBehaviour
{
	//aspect : 양상, 측면
    public float aspect = 1;
	void OnEnable ()
	{
	    GetComponent<Camera>().aspect = aspect;
	}
}
