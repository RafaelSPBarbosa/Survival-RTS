using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : WorldData {

	public List<Image> Slots = new List<Image>();
	public List<Text> AmmountOfItems_Txt = new List<Text>();
	//public int CurSelectedSlot = -1; // -1 = None
	public Transform _SelectionBox;
	public int CurSelectedSlot = -1;
	public GameObject BuildingMenuUI;


	public List<InvItem> _Inventory = new List<InvItem>();

	public GameObject InventoryUI;

	void Start(){

		AddItem (0, 99);
		AddItem (1, 99);
		AddItem (2, 99);
		AddItem (3, 99);
		AddItem (4, 99);
	}

	public bool CheckForItem ( int id, int ammount ){

		foreach (InvItem item in _Inventory) {

			if (item.ID == id) {

				return true;
			}
		}

		return false;
	}

		
	public void AddItem(int id, int ammount){

		bool AlreadyHasItem = false;
		foreach (InvItem item in _Inventory) {

			if (item.ID == id) {

				AlreadyHasItem = true;
				item.Ammount += ammount;
				UpdateInventory ();
				return;
			}
		}

		if (AlreadyHasItem == false) {

			_Inventory.Add (new InvItem (id, ammount));
			UpdateInventory ();
		}
	}

	public void RemoveItem(int id, int ammount){

		foreach (InvItem item in _Inventory) {

			if (item.ID == id) {

				item.Ammount -= ammount;
				if (item.Ammount <= 0) {
					item.Ammount = 0;
					_Inventory.Remove (item);
				}
				
				UpdateInventory ();
				return;
			}
		}
	}

	void Update(){


		if (Input.GetKeyDown (KeyCode.E)) {

			InventoryUI.SetActive (!InventoryUI.activeSelf);

			if (InventoryUI.activeSelf == true) {

				if (BuildingMenuUI.activeSelf == true) {

					BuildingMenuUI.SetActive (false);
				}

				UpdateInventory ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (InventoryUI.activeSelf == true) {

				InventoryUI.SetActive (false);
			}
		}
	}

	public void DiscardSelected (){

		if (CurSelectedSlot != -1) {
			_Inventory.RemoveAt (CurSelectedSlot);
			UpdateInventory ();
			_SelectionBox.gameObject.SetActive (false);
			CurSelectedSlot = -1;
		}
	}
		

	public void SelectSlot (int SlotID){

		if (CurSelectedSlot == -1) {
			
			CurSelectedSlot = SlotID;

			_SelectionBox.gameObject.SetActive (true);
			_SelectionBox.transform.position = Slots [SlotID].gameObject.transform.position;

		} else {

			CurSelectedSlot = -1;
			_SelectionBox.gameObject.SetActive (false);
		}

		UpdateInventory();
	}

	public void UpdateInventory(){

		for (int i = 0; i < Slots.Count; i++) {



			Sprite[] sprites = Resources.LoadAll<Sprite>("Icons/Icons");

			if (i < _Inventory.Count) {
				
				Slots [i].sprite = sprites [_Inventory [i].ID + 1];
				AmmountOfItems_Txt [i].text = _Inventory[i].Ammount.ToString ();

			} else {

				Slots [i].sprite = sprites [0];
				AmmountOfItems_Txt [i].text = 0.ToString();
			}

		}
	}
}

public class InvItem {

	public int ID{ get; set; }
	public int Ammount{ get; set; }

	public  InvItem  ( int id , int ammount ){

		this.ID = id;
		this.Ammount = ammount;
	}

}