using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Sex {

	Male, Female
}

public enum Specialization {

	Warrior, Builder, Healer
}

public class Unit : WorldData {

	private SelectionManager _SelectionManager;
	private PlayerManager _PlayerManager;
	private NavMeshAgent _NavAg;
	[SerializeField]
	private Animator _Animator;
	[SerializeField]
	private GameObject _Mesh;
	private AudioSource _AS;
	public AudioSource _FootStepAS;

	public Sex _Sex;
	public Specialization _Specialization;
	public GameObject Target;
	public GameObject HoldingItem;
	public ItemType TargetType;
	public BuildingType _TargetBuildingType;
	public bool isCarryingItem = false;
	public Transform _ObjectHoldingPos;

	public Rigidbody[] BoneRbs;
	public CapsuleCollider[] BonesCapsuleColliders;
	public BoxCollider[] BonesBoxColl;
	public SphereCollider Head;

	public bool isInsideTrigger = false;

	[Range(0, 100)]
	public int Health = 100;


	void Start(){

		_SelectionManager = FindObjectOfType (typeof(SelectionManager)) as SelectionManager;
		_PlayerManager = FindObjectOfType (typeof(PlayerManager)) as PlayerManager;
		_SelectionManager.UnitsList.Add (this.gameObject);
		_NavAg = GetComponent<NavMeshAgent> ();
		//_Animator = GetComponent<Animator> ();
		_AS = GetComponent<AudioSource> ();

		if(Time.timeSinceLevelLoad > 1.0f )
			_PlayerManager.UpdateHousing ();
	}

	void ResetAnimations(){

		_Animator.SetBool ("usingAxe", false);
		_Animator.SetBool ("usingPickAxe", false);
		_Animator.SetBool ("Building", false);
		_Animator.SetBool ("Mixing", false);
	}

	void Die(){


		foreach (CapsuleCollider n in BonesCapsuleColliders) {

			n.enabled = true;
		}

		Destroy (GetComponent<BoxCollider> ());
		Destroy (_NavAg);

		foreach (Rigidbody b in BoneRbs) {

			b.isKinematic = false;
		}

		foreach (BoxCollider c in BonesBoxColl) {

			c.enabled = true;
		}

		Head.enabled = true;
	
		_Animator.enabled = false;

		_SelectionManager.UnitsList.Remove (this.gameObject);
		_Mesh.layer = 0;
		_PlayerManager.UpdateHousing ();
		Destroy (this);
		//Destroy (this.gameObject);
	}

	public void FootStep(){

		int TargetSound = Random.Range (1, 3);

		_FootStepAS.clip = Resources.Load ("Sounds/FootStep" + TargetSound.ToString()) as AudioClip;
		_FootStepAS.Play ();
	}

	public void Hit(){

		if (TargetType == ItemType.Rock) {

			_AS.clip = Resources.Load ("Sounds/PickAxe") as AudioClip;
			_AS.Play ();
			Target.GetComponent<Item> ()._Animator.SetTrigger ("Hit");
			Target.GetComponent<Item> ().ReduceDurability (10);
		}

		else if (TargetType == ItemType.Tree) {

			_AS.clip = Resources.Load ("Sounds/Axe") as AudioClip;
			_AS.Play ();
			Target.GetComponent<Item> ()._Animator.SetTrigger ("Hit");
			Target.GetComponent<Item> ().ReduceDurability (10);
		}

		else if (TargetType == ItemType.Grass) {

			_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
			_AS.Play ();
			Target.GetComponent<Item> ()._Animator.SetTrigger ("Hit");
			Target.GetComponent<Item> ().ReduceDurability (15);
		}

		else if (TargetType == ItemType.BlueBerry) {

			_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
			_AS.Play ();
			Target.GetComponent<Item> ()._Animator.SetTrigger ("Hit");
			Target.GetComponent<Item> ().ReduceDurability (15);
		}

		if (Target.GetComponent<Building> () != null) {
			if (_TargetBuildingType == BuildingType.Shelter) {

				if (Target != null) {
					Target.GetComponent<Building> ().BuildingPercent += 5;
					_AS.clip = Resources.Load ("Sounds/Hammering") as AudioClip;
					_AS.Play ();
				}
			}

			else if (_TargetBuildingType == BuildingType.Wall) {

				if (Target != null) {
					Target.GetComponent<Building> ().BuildingPercent += 5;
					_AS.clip = Resources.Load ("Sounds/Hammering") as AudioClip;
					_AS.Play ();
				}
			}

			else if (_TargetBuildingType == BuildingType.Gate) {

				if (Target != null) {
					Target.GetComponent<Building> ().BuildingPercent += 5;
					_AS.clip = Resources.Load ("Sounds/Hammering") as AudioClip;
					_AS.Play ();
				}
			}

			else if (_TargetBuildingType == BuildingType.BasicTorch) {

				if (Target != null) {
					Target.GetComponent<Building> ().BuildingPercent += 10;
					_AS.clip = Resources.Load ("Sounds/Hammering") as AudioClip;
					_AS.Play ();
				}
			}
		}
			
		if (Target.GetComponent<Item> () != null) {
			if (Target.GetComponent<Item> ().Durability <= 0) {

				ResetAnimations ();
				_NavAg.updatePosition = true;
				_NavAg.updateRotation = true;

				if (TargetType == ItemType.Tree) {

					_SelectionManager.gameObject.GetComponent<Inventory> ().AddItem (0,2);
					Target.GetComponent<BoxCollider> ().enabled = false;
					Target.GetComponent<NavMeshObstacle> ().enabled = false;
					Target.GetComponent<Item> ().Die();
					//Destroy (Target, 5.0f);
					Target.GetComponent<AudioSource> ().clip = Resources.Load <AudioClip> ("Sounds/TreeFalling");
					Target.GetComponent<AudioSource> ().Play ();

				} 

				else if (TargetType == ItemType.Rock) {

					_SelectionManager.gameObject.GetComponent<Inventory> ().AddItem (1, 3);
					Target.GetComponent<BoxCollider> ().enabled = false;
					Target.GetComponent<NavMeshObstacle> ().enabled = false;
					Target.GetComponent<Item> ().Die();

					int r = Random.Range (0, 100);
					if (r >= 0 && r <= 40) {

						_SelectionManager.gameObject.GetComponent<Inventory> ().AddItem (4, 1);
					}


					//Destroy (Target);
				}

				else if (TargetType == ItemType.Grass) {

					_SelectionManager.gameObject.GetComponent<Inventory> ().AddItem (2, 1);
					Target.GetComponent<BoxCollider> ().enabled = false;
					Target.GetComponent<Item> ().Die();
					int r = Random.Range (0, 100);
					if (r >= 0 && r <= 60) {

						_SelectionManager.gameObject.GetComponent<Inventory> ().AddItem (3, 1);
					}

				}

				else if (TargetType == ItemType.BlueBerry) {

					_SelectionManager.gameObject.GetComponent<PlayerManager> ().IncreaseFood (10);
					GameObject go = Instantiate (Resources.Load ("Prefabs/GrassSmoke"), Target.transform.position + new Vector3 (0, 1, 0), Quaternion.identity) as GameObject;
					Destroy (go, 1.5f);
					Target.GetComponent<Item> ().PickBerries ();
				}

				Target.GetComponent<Item> ()._TargetUnit = null;
				Target = null;
			}
		}else{
			
			if (Target.GetComponent<Building> ().BuildingPercent == 100) {

				if (_TargetBuildingType == BuildingType.Shelter) {

					ResetAnimations ();
					Target.GetComponent<Building> ().FinishedBuilding ();
					Target.GetComponent<Building> ()._AssignedUnit = null;
					Target = null;
					_NavAg.updatePosition = true;
					_NavAg.updateRotation = true;
				}

				else if (_TargetBuildingType == BuildingType.Wall) {

					ResetAnimations ();
					Target.GetComponent<Building> ().FinishedBuilding ();
					Target.GetComponent<Building> ()._AssignedUnit = null;
					Target = null;
					_NavAg.updatePosition = true;
					_NavAg.updateRotation = true;
				}

				else if (_TargetBuildingType == BuildingType.Gate) {

					ResetAnimations ();
					Target.GetComponent<Building> ().FinishedBuilding ();
					Target.GetComponent<Building> ()._AssignedUnit = null;
					Target = null;
					_NavAg.updatePosition = true;
					_NavAg.updateRotation = true;
				}

				else if (_TargetBuildingType == BuildingType.BasicTorch) {

					ResetAnimations ();
					Target.GetComponent<Building> ().FinishedBuilding ();
					Target.GetComponent<Building> ()._AssignedUnit = null;
					Target = null;
					_NavAg.updatePosition = true;
					_NavAg.updateRotation = true;
				}
			}
		}
	}

	public void DropItem(){

		if (isCarryingItem == true) {
			if (HoldingItem.GetComponent<Item>()._ItemType == ItemType.MetalOre) {

				HoldingItem.transform.position = transform.position + transform.forward * 2;
				HoldingItem.GetComponent<NavMeshObstacle> ().enabled = true;
				HoldingItem.GetComponent<BoxCollider> ().enabled = true;
				isCarryingItem = false;
				_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
				_AS.Play ();
				HoldingItem = null;
			}

			else if (HoldingItem.GetComponent<Item>()._ItemType == ItemType.WaterBucket) {

				HoldingItem.transform.position = transform.position + transform.forward * 2;
				HoldingItem.GetComponent<NavMeshObstacle> ().enabled = true;
				HoldingItem.GetComponent<BoxCollider> ().enabled = true;
				isCarryingItem = false;
				_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
				_AS.Play ();
				HoldingItem = null;
			}
		}
	}

	void OnTriggerEnter(Collider other){


		if (other.transform.gameObject == Target) {

			isInsideTrigger = true;
			_NavAg.SetDestination (this.transform.position);
			RotateTowards (Target.transform);
		}
	}

	private void RotateTowards(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
		transform.rotation = lookRotation;//Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
	}

	void OnTriggerExit(Collider other){

		if (other.transform.gameObject == Target) {

			isInsideTrigger = false;
		}
	}

	void Update(){

		if (Health <= 0) {

			Die ();
		}

		if (Target == null) {

			isInsideTrigger = false;
		}
			
		if (Target != null) {
			if (Vector3.Distance (this.transform.position, Target.transform.position) < 4) {

				Building TargetBuilding = Target.GetComponent<Building> ();
				if (TargetBuilding != null) {// -------------- Buildings------------------
					if (TargetBuilding.BuildingPercent < 100) {
						if (TargetBuilding != null) {

							_Animator.SetBool ("Building", true);

							if (_NavAg.velocity == Vector3.zero) {
								_NavAg.updatePosition = false;
								_NavAg.updateRotation = false;
								transform.position = TargetBuilding.BuildingPos.position;
								RotateTowards (Target.transform);

							}
						}
					} else {

						if (TargetBuilding._BuildingType == BuildingType.Shelter) {
							if (isCarryingItem) {

								if (_NavAg.velocity == Vector3.zero) {
									_SelectionManager.GetComponent<PlayerManager> ().IncreaseWater (25);
									Destroy (HoldingItem);
									HoldingItem = null;
									isCarryingItem = false;
								}
							}
						}
					}
				} else {// --------------------------------------- OBjects
					
					if (TargetType == ItemType.Tree) {


						if (_NavAg.velocity == Vector3.zero) {

							_Animator.SetBool ("usingAxe", true);
							_NavAg.updatePosition = false;
							_NavAg.updateRotation = false;
							transform.position = Target.GetComponent<Item> ().InteractionPoint.position;
							RotateTowards (Target.transform);

						}

					} else if (TargetType == ItemType.Rock) {



						if (_NavAg.velocity == Vector3.zero) {
							_Animator.SetBool ("usingPickAxe", true);
							_NavAg.updatePosition = false;
							_NavAg.updateRotation = false;
							transform.position = Target.GetComponent<Item> ().InteractionPoint.position;
							RotateTowards (Target.transform);

						}

					} else if (TargetType == ItemType.Grass) {


						if (_NavAg.velocity == Vector3.zero) {
							_Animator.SetBool ("Mixing", true);
							_NavAg.updatePosition = false;
							_NavAg.updateRotation = false;
							transform.position = Target.GetComponent<Item> ().InteractionPoint.position;
							RotateTowards (Target.transform);

						}

					} else if (TargetType == ItemType.BlueBerry) {

						if (Target.GetComponent<Item> ().Durability == 100) {

							//Target.GetComponent<Item> ().PickBerries ();
							//_SelectionManager.GetComponent<PlayerManager> ().IncreaseFood (5);
							//Target = null;

							if (_NavAg.velocity == Vector3.zero) {
								_Animator.SetBool ("Mixing", true);
								_NavAg.updatePosition = false;
								_NavAg.updateRotation = false;
								transform.position = Target.GetComponent<Item> ().InteractionPoint.position;
								RotateTowards (Target.transform);

							}
						}
					} else if (TargetType == ItemType.Stone) {

						Destroy (Target.gameObject);
						_SelectionManager.GetComponent<Inventory> ().AddItem (1, 1);
						Target = null;

						if (_NavAg.velocity == Vector3.zero) {
							_NavAg.updatePosition = false;
							_NavAg.updateRotation = false;
							transform.position = Target.GetComponent<Item> ().InteractionPoint.position;
							RotateTowards (Target.transform);

						}

	
					} else if (TargetType == ItemType.MetalOre) {

						ResetAnimations ();
						Target.GetComponent<NavMeshObstacle> ().enabled = false;
						Target.GetComponent<BoxCollider> ().enabled = false;
						_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
						_AS.Play ();
						HoldingItem = Target;
						Target = null;
						isCarryingItem = true;
					} else if (TargetType == ItemType.WaterBucket) {

						ResetAnimations ();
						Target.GetComponent<NavMeshObstacle> ().enabled = false;
						Target.GetComponent<BoxCollider> ().enabled = false;
						_AS.clip = Resources.Load ("Sounds/PickupItem") as AudioClip;
						_AS.Play ();
						HoldingItem = Target;
						Target = null;
						isCarryingItem = true;

					}

				}
			}

			if (Vector3.Distance (this.transform.position, Target.transform.position) < 12) {
				if (TargetType == ItemType.Lake) {
					if (isCarryingItem == false) {
						isCarryingItem = true;
						Target.GetComponent<Item> ()._TargetUnit = null;
						GameObject go = Instantiate (Resources.Load ("Prefabs/WaterBucket"), this.transform.position, Quaternion.identity) as GameObject;
						Target = go;
						TargetType = go.GetComponent<Item> ()._ItemType;
						Target.GetComponent<NavMeshObstacle> ().enabled = false;
						Target.GetComponent<BoxCollider> ().enabled = false;
						HoldingItem = Target;
					}
				}
			}
		}

		if (isCarryingItem == true) {

			HoldingItem.transform.position = _ObjectHoldingPos.position;

			_NavAg.speed = 2;
		} else {

			_NavAg.speed = 4;
		}

		if (Input.GetButtonDown ("RMB")) {

			if (_SelectionManager.SelectedUnitsList.Contains (this.gameObject)) {
				
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit)) {

					if (hit.transform.gameObject.tag == "Floor") {
						_NavAg.SetDestination (hit.point);
						//if (isCarryingItem == false)
						if (Target != null) {
							if (TargetType == ItemType.Building) {

								Target.GetComponent<Building> ()._AssignedUnit = null;
								_NavAg.updatePosition = true;
								_NavAg.updateRotation = true;
							} else {
								
								Target.GetComponent<Item> ()._TargetUnit = null;
								_NavAg.updatePosition = true;
								_NavAg.updateRotation = true;

							}
							Target = null;
						}
						ResetAnimations ();

					} else if (hit.transform.gameObject.tag == "Interactable") {
					
						if (_Specialization == Specialization.Builder) {
							if (hit.transform.gameObject.GetComponent<Item> ()._TargetUnit == null) {
								Target = hit.transform.gameObject;
								Target.GetComponent<Item> ()._TargetUnit = this;
								TargetType = Target.GetComponent<Item> ()._ItemType;
								_NavAg.SetDestination (Target.transform.position);
								GetComponent<BoxCollider> ().enabled = false;
								GetComponent<BoxCollider> ().enabled = true;
							}
						}
					}
					else if (hit.transform.gameObject.tag == "Building") {
						if (hit.transform.gameObject.GetComponent<Building> ().BuildingPercent != 100) {
							if (_Specialization == Specialization.Builder) {
								if (hit.transform.gameObject.GetComponent<Building> ()._AssignedUnit == null) {

									Target = hit.transform.gameObject;
									TargetType = ItemType.Building;
									_TargetBuildingType = Target.GetComponent<Building> ()._BuildingType;
									Target.GetComponent<Building> ()._AssignedUnit = this;
									_NavAg.SetDestination (Target.transform.position);
									GetComponent<BoxCollider> ().enabled = false;
									GetComponent<BoxCollider> ().enabled = true;
								}
							}
						} else {
							if (isCarryingItem == true) {

								Target = hit.transform.gameObject;
								TargetType = ItemType.Building;
								_TargetBuildingType = Target.GetComponent<Building> ()._BuildingType;
								_NavAg.SetDestination (Target.transform.position);
							}
						}
					}
				}
			}
		}

		//Animations
		if (_NavAg.velocity != Vector3.zero) {

			_Animator.SetBool ("Walking", true);
		} else {

			_Animator.SetBool ("Walking", false);
		}

		//Carrying Anim

		_Animator.SetBool ("CarryingItem", isCarryingItem);

		if (Health > 0) {
			if (_SelectionManager.SelectedUnitsList.Contains (gameObject)) {
				if (_SelectionManager.InventoryUI.activeSelf == false && _SelectionManager.BuildingUI.activeSelf == false) {
				
					_Mesh.layer = 11;
				} else {

					_Mesh.layer = 0;
				}
			} else {

				_Mesh.layer = 0;
			}
		}
	}


	public void Select (){

		//_Mesh.layer = 11;
		_AS.clip = Resources.Load ("Sounds/SelectUnit") as AudioClip;
		_AS.Play ();
	}

	public void DeSelect (){

		//_Mesh.layer = 0;
	}
}