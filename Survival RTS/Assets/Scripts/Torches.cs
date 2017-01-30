using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torches : MonoBehaviour {

	public Material DayTimeMat, NightTimeMat;
	TimeManager _TimeManager;
	public GameObject Light;
	public bool isBuilt = false;

	void Start(){

		_TimeManager = (TimeManager)FindObjectOfType (typeof(TimeManager));
	}

	public void DoneBuilding (){

		isBuilt = true;

		UpdateDaytime (_TimeManager.isDay);
	}

	public void UpdateDaytime ( bool isDay ){

		if (isBuilt) {
			if (isDay == true) {

				GetComponent<MeshRenderer> ().material = DayTimeMat;
				Light.SetActive (false);
			} else {

				GetComponent<MeshRenderer> ().material = NightTimeMat;
				Light.SetActive (true);
			}
		} else {

			GetComponent<MeshRenderer> ().material = DayTimeMat;
			Light.SetActive (false);
		}
	}
}