using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WorldData {

	public ItemType _ItemType;
	public Animator _Animator;
	public Unit _TargetUnit;
	public GameObject _Mesh;
	public SelectionManager _SelectionManager;

	public Transform InteractionPoint;

	public GameObject BlueBerry;
	private bool MouseOver;
	private bool Dead = false;
	private int DaysSinceDeath;
	private Transform _transform;

	[SerializeField]
	private LayerMask _LayerMask;

	[Range ( 0, 100)]
	public int Durability = 100;

	void Start(){

		_SelectionManager = FindObjectOfType (typeof(SelectionManager)) as SelectionManager;
		_transform = GetComponent<Transform> ();

		if (_ItemType == ItemType.BlueBerry) {

			TimeManager.OnNewDay += ReSpawnBerries;
		}

		if (_ItemType == ItemType.Tree || _ItemType == ItemType.Rock || _ItemType == ItemType.Grass) {

			TimeManager.OnNewDay += NewDay;
		}
	}

	public void Die(){

		Dead = true;
	

		if (_ItemType == ItemType.Tree) {
			Destroy (_transform.Find ("Mesh").gameObject, 5);
			_Animator.SetBool ("Alive", false);

		} else {

			Destroy (_transform.Find ("Mesh").gameObject);
		}

		if (_ItemType == ItemType.Rock) {

			GameObject go = Instantiate (Resources.Load ("Prefabs/RockPieces"), _transform.position + new Vector3 (0, 1, 0), Quaternion.identity) as GameObject;
			Destroy (go, 1.5f);

		}

		if (_ItemType == ItemType.Grass) {

			GameObject go = Instantiate (Resources.Load ("Prefabs/GrassSmoke"), _transform.position + new Vector3 (0, 1, 0), Quaternion.identity) as GameObject;
			Destroy (go, 1.5f);
		}
	}


	void NewDay(){

		if (Dead) {

			DaysSinceDeath++;
		}

		if (DaysSinceDeath == 5) {

			if (_ItemType == ItemType.Tree) {
				
				RespawnTree ();
				return;
			} else if(_ItemType == ItemType.Rock) {

				RespawnRock ();
				return;
			}else if( _ItemType == ItemType.Grass) {

				RespawnGrass();
				return;
			}
		}
	}

	private void RespawnGrass (){
		if (!Physics.CheckBox (_transform.position, new Vector3 (1.0f, 1.0f, 1.0f), Quaternion.identity , _LayerMask)) {

			TimeManager.OnNewDay -= NewDay;
			Destroy (gameObject);
			Instantiate (Resources.Load ("Prefabs/TallGrass") as GameObject, _transform.position, _transform.rotation);
		} else {

			DaysSinceDeath = 0;
		}
	}

	private void RespawnRock(){

		if (!Physics.CheckBox (_transform.position, new Vector3 (1.0f, 1.0f, 1.0f), Quaternion.identity , _LayerMask)) {


			TimeManager.OnNewDay -= NewDay;
			Destroy (gameObject);
			Instantiate (Resources.Load ("Prefabs/Rock" + Random.Range (1, 4)) as GameObject, _transform.position, _transform.rotation);
		} else {

			DaysSinceDeath = 0;
		}
	}

	private void RespawnTree(){

		if (!Physics.CheckBox (_transform.position, new Vector3 (2.0f, 2.0f, 2.0f), Quaternion.identity , _LayerMask)) {
			

			TimeManager.OnNewDay -= NewDay;
			Destroy (gameObject);
			Instantiate (Resources.Load ("Prefabs/Tree0" + Random.Range (1, 4)) as GameObject, _transform.position, _transform.rotation);
		} else {

			DaysSinceDeath = 0;
		}
	}

	void OnMouseEnter(){

		//_Mesh.layer = 12;
		MouseOver = true;
	}

	void OnMouseExit(){

		//_Mesh.layer = 0;
		MouseOver = false;
	}

	void ReSpawnBerries(){

		Durability = 100;
		BlueBerry.SetActive (true);
	}

	public void PickBerries(){

		Durability = 0;
		BlueBerry.SetActive (false);
	}

	void Update(){


		if (Input.GetKeyDown (KeyCode.Y)) {
			if (_ItemType == ItemType.BlueBerry) {
			

				ReSpawnBerries ();
			}
		}
			
		if (_SelectionManager.InventoryUI.activeSelf == false && _SelectionManager.BuildingUI.activeSelf == false) {
			if (MouseOver == true) {

				if(_Mesh != null)
				_Mesh.gameObject.layer = 12;
			} else {
				if(_Mesh != null)
				_Mesh.gameObject.layer = 0;
			}
		} else {
			if(_Mesh != null)
			_Mesh.gameObject.layer = 0;
		}
	}

	public void ReduceDurability(int _Durability ){

		Durability -= _Durability;
	}
}