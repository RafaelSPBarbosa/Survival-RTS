using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

	bool isSelecting = false;
	Vector3 mousePosition1;
	public List<GameObject> UnitsList = new List<GameObject> ();
	public List<GameObject> SelectedUnitsList = new List<GameObject> ();
	public GameObject InventoryUI, BuildingUI;
	public bool CanDrag = true;

	void Update()
	{
		if (CanDrag == true) {
			// If we press the left mouse button, save mouse location and begin selection
			if (Input.GetButtonDown ("LMB")) {
			
				mousePosition1 = Input.mousePosition;
			}

			if (Input.GetButton ("LMB")) {
			
				if (mousePosition1 != Input.mousePosition)
					isSelecting = true;
			}
			// If we let go of the left mouse button, end selection
			if (Input.GetButtonUp ("LMB"))
				isSelecting = false;

			if (isSelecting) {

				foreach (GameObject unit in UnitsList) {

					if (IsWithinSelectionBounds (unit)) {

						if (!SelectedUnitsList.Contains (unit)) {
							unit.SendMessage ("Select");
							SelectedUnitsList.Add (unit);
						}
					} else {
						if (SelectedUnitsList.Contains (unit)) {
							unit.SendMessage ("DeSelect");
							SelectedUnitsList.Remove (unit);
						}
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if( isSelecting )
		{
			// Create a rect from both mouse positions
			var rect = Utils.GetScreenRect( mousePosition1, Input.mousePosition );
			Utils.DrawScreenRect( rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
			Utils.DrawScreenRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ) );
		}
	}

	public bool IsWithinSelectionBounds ( GameObject gameObject )
	{
		if (!isSelecting)
			return false;

		var camera = Camera.main;
		var viewportBounds = Utils.GetViewportBounds (camera, mousePosition1, Input.mousePosition);

		return viewportBounds.Contains (camera.WorldToViewportPoint (gameObject.transform.position));
	}
}
