using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : WorldData {

	[SerializeField]
	private GameObject _BuildingMenuUI;
	public GameObject _InventoryUI;

	[SerializeField]
	private Inventory _Inv;

	public GameObject[] _TabsObjs;
	public Image[] _TabsImgs;

	public int _CurTab = 0;

	public Sprite _Tab_Selected, _Tab_DeSelected;

	public bool _ShowingToolTip = false;
	public Text _ToolTipText;

	public AudioSource _ButtonAS;
	public AudioSource _As;

	public BuildingType _BuildingType;


	[Header("Building Mechanics")]
	public LayerMask _RayLayerMask;
	private bool _isBuilding = false;
	private GameObject _GhostBuilding;

	[SerializeField]
	private GameObject _ToolTipObj;

	public void UpdateToolTip_Text( string _Text){

		string _NewText = _Text.Replace ("\\n", "\n");

		_ToolTipText.text = _NewText;
	}

	public void PlayMouseOverSFX(){

		_ButtonAS.clip = Resources.Load<AudioClip> ("Sounds/MouseOver_Btn");
		_ButtonAS.Play ();
	}

	public void UpdateToolTip_State( bool _State){

		if (_State == true) {

			_ToolTipObj.transform.position = Input.mousePosition;

			if (_ToolTipObj.activeSelf == false) {
				_ToolTipObj.SetActive (true);
			}



		} else {

			if(_ToolTipObj.activeSelf == true)
				_ToolTipObj.SetActive (false);
		}
	}

	void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (_BuildingMenuUI.activeSelf == true) {




				_BuildingMenuUI.SetActive (false);
			}
		}

		if (Input.GetKeyDown (KeyCode.B)) {

			_BuildingMenuUI.SetActive (!_BuildingMenuUI.activeSelf);

			if (_BuildingMenuUI.activeSelf == true) {

				if (_InventoryUI.activeSelf == true) {

					_InventoryUI.SetActive (false);
				}

				UpdateBuildingMenu ();
			}
		}
			

		if (_isBuilding == true) {
			if (_GhostBuilding != null) {

				// Placing down Buildings
				if (_GhostBuilding.GetComponent<GhostBuilding> ()._isPlacable == true) {
					if (Input.GetMouseButtonDown (0)) {

						if (VerifyCost (_GhostBuilding.GetComponent<Building> ()._BuildingType , true)) {
							_GhostBuilding.SendMessage ("PlaceDown");

						} else {

							Destroy (_GhostBuilding);
						}

						_isBuilding = false;
						_GhostBuilding = null;
					}
				}

				if (Input.GetMouseButtonDown (1)) {

					_isBuilding = false;
					Destroy (_GhostBuilding);
					_GhostBuilding = null;
				}

				if (_GhostBuilding != null) {
					//Moving the building towards the mouse's world position.
					Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit _hit;

					if (Physics.Raycast (_ray, out _hit, Mathf.Infinity, _RayLayerMask)) {

						_GhostBuilding.transform.position = _hit.point;
					}
				}
			}
		}
	}

	public void ChangeTab(int TabID){

		_CurTab = TabID;
		_As.clip = Resources.Load <AudioClip>("Sounds/Page Flip");
		_As.Play ();
		UpdateBuildingMenu ();
	}

	public bool VerifyCost(BuildingType Type, bool Spend){

		switch (Type) {

		case BuildingType.BasicTorch:
			if (_Inv.CheckForItem (0, 3) && _Inv.CheckForItem (4, 2)) {

				if (Spend == true) {
					_Inv.RemoveItem (0, 3);
					_Inv.RemoveItem (4, 2);
				}

				return true;
			} else {

				return false;
			}
			break;

		case BuildingType.Wall:
			if (_Inv.CheckForItem (0, 10) && _Inv.CheckForItem (2, 2)) {

				if (Spend == true) {
					_Inv.RemoveItem (0, 10);
					_Inv.RemoveItem (2, 2);
				}

				return true;
			} else {
				
				return false;
			}
			break;

		case BuildingType.Gate:

			if (_Inv.CheckForItem (0, 10) && _Inv.CheckForItem (2, 5) && _Inv.CheckForItem (1, 4)) {

				if (Spend == true) {
					_Inv.RemoveItem (0, 10);
					_Inv.RemoveItem (2, 5);
					_Inv.RemoveItem (1, 4);
				}

				return true;
			} else {

				return false;
			}
			break;

		case BuildingType.Shelter:

			if (_Inv.CheckForItem (0, 5)) {

				if (Spend == true) {
					_Inv.RemoveItem (0, 5);
				}

				return true;
			} else {

				return false;
			}
			break;

		}

		return false;
	}

	public void CreateBuilding(BuildingType Type){

		if (VerifyCost (Type , false) == false) {

			return;
		}

		switch (Type) {

		default:

			print ("ERROR! There is no object with this ID!");

			break;

		case BuildingType.BasicTorch:
			
			_GhostBuilding = Instantiate (Resources.Load<GameObject> ("Prefabs/Buildings/Basic Torch"), Vector3.zero, Quaternion.identity) as GameObject;
			break;

		case BuildingType.Gate:

			_GhostBuilding = Instantiate (Resources.Load<GameObject> ("Prefabs/Buildings/Wooden Gate"), Vector3.zero, Quaternion.identity) as GameObject;
			break;

		case BuildingType.Shelter:

			_GhostBuilding = Instantiate (Resources.Load<GameObject> ("Prefabs/Buildings/Basic Shelter"), Vector3.zero, Quaternion.identity) as GameObject;
			break;

		case BuildingType.Wall:

			_GhostBuilding = Instantiate (Resources.Load<GameObject> ("Prefabs/Buildings/Wooden Wall"), Vector3.zero, Quaternion.identity) as GameObject;
			break;

		}

		_BuildingMenuUI.SetActive (false);
		_GhostBuilding.GetComponent<GhostBuilding> ()._BM = this;
		_isBuilding = true;
		_GhostBuilding.GetComponent<Building> ()._BuildingType = Type;
	}
		
	public void UpdateBuildingMenu(){

		for (int i = 0; i < _TabsImgs.Length; i ++) {

			if (i != _CurTab) {
				_TabsObjs [i].SetActive (false);
				_TabsImgs [i].sprite = _Tab_DeSelected;
			} else {

				_TabsObjs [i].SetActive (true);
				_TabsImgs [i].sprite = _Tab_Selected;
			}
		}
	}
}