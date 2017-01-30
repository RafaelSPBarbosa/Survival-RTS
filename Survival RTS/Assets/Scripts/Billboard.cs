using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	Transform TargetObj;

	void Start()
	{
		TargetObj = Camera.main.transform;
	}

	void Update () {

		//this.transform.LookAt(new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z));

		Vector3 newRotation = new Vector3(this.transform.eulerAngles.x, TargetObj.eulerAngles.y , this.transform.eulerAngles.z);
		this.transform.eulerAngles = newRotation;
	}
}
