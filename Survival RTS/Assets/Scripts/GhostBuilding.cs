using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostBuilding : MonoBehaviour {

	public BoxCollider _BoxCol;
	public MeshRenderer _Mat;
	public SkinnedMeshRenderer _SkinnedMat;
	public Material _FinalMat;
	public Material[] _FinalMaterials;

	public bool _isPlacable;
	public LayerMask _IgnoreLayer;
	public Building _Building;


	public NavMeshObstacle _NavObstacle;

	public BuildingManager _BM;

	public SphereCollider _SC;

	void Start(){

		if(_BoxCol == null)
		_BoxCol = GetComponent<BoxCollider> ();

		Physics.IgnoreLayerCollision (8, 9, true);
		Physics.IgnoreLayerCollision (10, 9, true);

		UpdateState ();
	}
		
	void UpdateState(){

		if (_isPlacable == true) {
			if (_Mat != null) {
				
				_Mat.material = Resources.Load<Material> ("Materials/Ghost_Buildable");

			} else {

				Material[] _Mats = _SkinnedMat.materials;
				_Mats [0] = Resources.Load<Material> ("Materials/Ghost_Buildable");
				_Mats [1] = Resources.Load<Material> ("Materials/Ghost_Buildable");
				_SkinnedMat.materials = _Mats;
			}
		} else {
			if (_Mat != null) {
				
				_Mat.material = Resources.Load<Material> ("Materials/Ghost_Obstruct");
			} else {

				Material[] _Mats = _SkinnedMat.materials;
				_Mats [0] =  Resources.Load<Material> ("Materials/Ghost_Obstruct");
				_Mats [1] =  Resources.Load<Material> ("Materials/Ghost_Obstruct");
				_SkinnedMat.materials = _Mats;
			}
		}
	}


	void Update(){

		if (Input.GetKeyDown (KeyCode.R)) {

			this.transform.Rotate (new Vector3 (0, 45, 0));
		}
	}

	void OnTriggerStay(Collider other){


		_isPlacable = false;
		UpdateState ();
	}

	void OnTriggerExit(Collider other){

		_isPlacable = true;
		UpdateState ();
	}


	public void PlaceDown( ){

		if (_Mat != null) {
			_Mat.material = _FinalMat;
		} else {
			
			_SkinnedMat.materials = _FinalMaterials;
		}

		TimeManager _TimeManager =  (TimeManager)FindObjectOfType( typeof(TimeManager));

		if(_Building._BuildingType == BuildingType.BasicTorch)
			transform.FindChild ("Mesh").SendMessage ("UpdateDaytime", _TimeManager.isDay);

		if (_Building._BuildingType == BuildingType.Gate) {

			_BoxCol.enabled = true;
			Destroy(transform.Find ("TempColl").gameObject);
		}

		_Building.gameObject.layer = 0;
		_Building.OnPlaceDown ();

		gameObject.isStatic = true;
		if (_Building._BuildingType != BuildingType.Gate)
			gameObject.GetComponent<BoxCollider> ().isTrigger = false;

		if(_NavObstacle != null)
			_NavObstacle.enabled = true;
		transform.tag = "Building";
		if(_SC != null)
			_SC.enabled = true;
		Destroy (this);
	}
}