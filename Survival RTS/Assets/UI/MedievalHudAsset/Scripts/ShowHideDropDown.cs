using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowHideDropDown : MonoBehaviour {

	public GameObject DropPanel = null;
	public Image Button, Arrow = null;
	public Sprite ArrowDown, ArrowUp = null;
	// Use this for initialization
	void Start () {
	
	}

	public void ToggleDropPanel()
	{
		if (DropPanel.activeInHierarchy) {
			DropPanel.SetActive (false);
			Arrow.sprite = ArrowDown;
		} else {
			DropPanel.SetActive (true);
			Arrow.sprite = ArrowUp;
		}
	}

}
