using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassAnimMsg : MonoBehaviour {

	public void Hit(){

		transform.parent.GetComponent<Unit> ().Hit ();
	}
}