using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimeManager : MonoBehaviour {

	public delegate void NewDay();
	public static event NewDay OnNewDay;

	public ParticleSystem FireFlies, Smoke;
	public Light NightLight, DayLight;
	public bool isDay = true;

	public AudioMixerSnapshot DaySnapshot, NightSnapshot;

	public int CurTime;
	private bool isTimeRunning = true;

	public float TimeSpeed = 1f;

	void Start(){

		var em2 = Smoke.emission;
		em2.enabled = false;

		var em = FireFlies.emission;
		em.enabled = false;

		StartCoroutine ("IncreaseTimer");
	}

	IEnumerator IncreaseTimer(){

		while (isTimeRunning == true) {

			CurTime += 1;
			yield return new WaitForSeconds (3 / TimeSpeed);
		}
	}

	public IEnumerator AddFog(){

		for(float i = 0.0f; i< 0.04f; i += 0.0005f ){
			
			RenderSettings.fogDensity = i;
			yield return null;
		}
	}

	public IEnumerator RemoveFog(){


		for(float i = 0.0f; i< 0.04f; i += 0.0005f ){

			RenderSettings.fogDensity = 0.04f - i;
			yield return null;
		}
	}

	public IEnumerator TurnDay(){

		var em = FireFlies.emission;
		em.enabled = false;
		var em2 = Smoke.emission;
		em2.enabled = false;

		OnNewDay ();

		DaySnapshot.TransitionTo (10.0f);

		StartCoroutine ("RemoveFog");


		DayLight.enabled = true;

		for (float i = 0.0f; i < 1.0f; i += 0.005f * TimeSpeed) {

			NightLight.intensity = 1.0f - i;
			DayLight.intensity = i;
			yield return null;
		}

		NightLight.enabled = false;

		WarnDependentObjects (true);
	}

	void WarnDependentObjects( bool isDay){

		Torches[] AllTorches = FindObjectsOfType (typeof(Torches)) as Torches[];

		foreach (Torches t in AllTorches) {

			t.UpdateDaytime (isDay);
		}
	}

	public IEnumerator TurnNight(){


		var em = FireFlies.emission;
		em.enabled = true;

		var em2 = Smoke.emission;
		em2.enabled = true;

		NightSnapshot.TransitionTo (10.0f);

		StartCoroutine ("AddFog");

		NightLight.enabled = true;

		for (float i = 0.0f; i < 1.0f; i += 0.005f * TimeSpeed) {

			DayLight.intensity = 1.0f - i;
			NightLight.intensity = i;
			yield return null;
		}

		DayLight.enabled = false;

		WarnDependentObjects (false);
	}

	void FixedUpdate(){

		if (Input.GetKey (KeyCode.T)) {

			Time.timeScale = 50;
		} else {

			Time.timeScale = 1;
		}

		if (isDay == true) {
			if (CurTime >= 60) {
				StartCoroutine ("TurnNight");
				isDay = false;

			}
		}else {

			if (CurTime >= 120) {
				StartCoroutine ("TurnDay");
				CurTime = 0;
				isDay = true;

			}
		}
	}
}
