using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Solidify : MonoBehaviour
{//solidify : 굳어지다
	public Shader flatShader;
    Camera cam;
	void OnEnable ()
	{
	    cam = GetComponent<Camera>();
        cam.SetReplacementShader(flatShader, "");
	}
	
}
