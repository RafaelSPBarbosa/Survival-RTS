using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBuildingBtnCond : WorldData {

	public BuildingType _BuildingType;

	void Start(){

		Button MyButton = GetComponent<Button> ();
		BuildingManager _BM = FindObjectOfType (typeof(BuildingManager)) as BuildingManager;

		MyButton.onClick.AddListener(() => _BM.CreateBuilding(_BuildingType));
	}
}
