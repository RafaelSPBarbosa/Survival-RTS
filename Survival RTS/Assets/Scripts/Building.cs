using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldData {

	public bool isBuilt;
	public bool isFullyBuilt = false;
	[Range(0,100)]
	public int BuildingPercent = 0;

	[SerializeField]
	private GameObject GhostObj;
	public Unit _AssignedUnit;
	public MeshFilter _Mesh;
	public SkinnedMeshRenderer _SkinnedMesh;
	public BuildingType _BuildingType;
	public PlayerManager _PlayerManager;
	public bool _isSelected = false;
	public GameObject _DestroyBuildingUI;
	public SelectionManager _SelectionManager;
	public bool HasUnit = false;
	public Transform BuildingPos;

	void Start(){
		
		_SelectionManager = FindObjectOfType (typeof(SelectionManager)) as SelectionManager;
		_SelectionManager.UnitsList.Add (this.gameObject);
		_PlayerManager = GameObject.Find ("PlayerManager").GetComponent<PlayerManager> ();
	}

	public void OnPlaceDown(){

		isBuilt = true;
		GhostObj.SetActive (true);
	}
		
	public void FinishedBuilding(){

		if (_BuildingType == BuildingType.Shelter) {

			_PlayerManager._Housing += 1;
			_PlayerManager.UpdateHousing ();
		}

		Destroy (GhostObj);
	}

	public void Select(){


			_isSelected = true;
			_DestroyBuildingUI.SetActive (true);
			//_Mesh.gameObject.layer = 11;
	}


	void OnTriggerStay(Collider other){

		if (_BuildingType == BuildingType.Gate) {
			if(BuildingPercent == 100)
				if(other.transform.gameObject.GetComponent<Unit>() != null)
					GetComponent<Animator> ().SetBool ("Open", true);
		}
	}

	void OnTriggerExit(Collider other){
		if (_BuildingType == BuildingType.Gate) {
			if(BuildingPercent == 100)
				if(other.transform.gameObject.GetComponent<Unit>() != null)
					GetComponent<Animator> ().SetBool ("Open", false);
		}
	}

	public void DeSelect(){


			_isSelected = false;
			_DestroyBuildingUI.SetActive (false);
	}

	public void DestroyBuilding(){

		_SelectionManager.UnitsList.Remove (this.gameObject);
		Destroy (gameObject);
	}


	void Update(){

		if (_BuildingType == BuildingType.Gate) {
			if (BuildingPercent == 100) {

				gameObject.layer = 2;
			}
		}
		
		if (_SelectionManager.SelectedUnitsList.Contains (gameObject)) {
			if (_SelectionManager.InventoryUI.activeSelf == false && _SelectionManager.BuildingUI.activeSelf == false) {

				if (_Mesh != null) {
					_Mesh.gameObject.layer = 11;

				} else {

					_SkinnedMesh.gameObject.layer = 11;
				}
			} else {

				if (_Mesh != null) {
					_Mesh.gameObject.layer = 0;

				} else {

					_SkinnedMesh.gameObject.layer = 0;
				}
			}
		} else {

			if (_Mesh != null) {
				_Mesh.gameObject.layer = 0;

			} else {

				_SkinnedMesh.gameObject.layer = 0;
			}
		}

		BuildingPercent = Mathf.Clamp (BuildingPercent, 0, 100);

		if (isBuilt == true) {
			if (_BuildingType == BuildingType.Shelter) {
				if (BuildingPercent >= 0 && BuildingPercent < 33) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Shelter/Basic Shelter 0");
				} else if (BuildingPercent >= 33 && BuildingPercent < 66) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Shelter/Basic Shelter 1");
				} else if (BuildingPercent >= 66 && BuildingPercent < 100) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Shelter/Basic Shelter 2");
				} else if (BuildingPercent >= 100) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Shelter/Basic Shelter 3");
				}
			} else if (_BuildingType == BuildingType.Wall) {
				if (BuildingPercent >= 0 && BuildingPercent < 33) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Wooden Wall/wooden_wall_0");
				} else if (BuildingPercent >= 33 && BuildingPercent < 66) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Wooden Wall/wooden_wall_1");
				} else if (BuildingPercent >= 66 && BuildingPercent < 100) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Wooden Wall/wooden_wall_2");
				} else if (BuildingPercent >= 100) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Wooden Wall/wooden_wall_3");
				}
			}
			else if (_BuildingType == BuildingType.Gate) {
				if (BuildingPercent >= 0 && BuildingPercent < 33) {

					Material[] _Mats = _SkinnedMesh.materials;
					_Mats[1] = Resources.Load<Material> ("Materials/WoodenWall");
					_SkinnedMesh.materials = _Mats;
					_SkinnedMesh.sharedMesh = Resources.Load<Mesh> ("Meshes/Wooden Gate/Wooden Gate 0");

				} else if (BuildingPercent >= 33 && BuildingPercent < 66) {
					
					Material[] _Mats = _SkinnedMesh.materials;
					_Mats[1] = Resources.Load<Material> ("Materials/Sign");
					_SkinnedMesh.materials = _Mats;
					_SkinnedMesh.sharedMesh = Resources.Load<Mesh> ("Meshes/Wooden Gate/Wooden Gate 1");
				} else if (BuildingPercent >= 66 && BuildingPercent < 100) {

					_SkinnedMesh.sharedMesh = Resources.Load<Mesh> ("Meshes/Wooden Gate/Wooden Gate 2");
				} else if (BuildingPercent >= 100) {

					_SkinnedMesh.sharedMesh = Resources.Load<Mesh> ("Meshes/Wooden Gate/Wooden Gate 3");
				}
			}
			else if (_BuildingType == BuildingType.BasicTorch) {
				if (BuildingPercent >= 0 && BuildingPercent < 33) {


					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Torch/Basic Torch 0");

				} else if (BuildingPercent >= 33 && BuildingPercent < 66) {


					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Torch/Basic Torch 1");

				} else if (BuildingPercent >= 66 && BuildingPercent < 100) {

					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Torch/Basic Torch 2");

				} else if (BuildingPercent >= 100) {
					
					_Mesh.mesh = Resources.Load<Mesh> ("Meshes/Basic Torch/Basic Torch 3");

					if (isFullyBuilt == false) {

						_Mesh.SendMessage ("DoneBuilding");
						isFullyBuilt = true;
					}

				}
			}
		}
	}
}