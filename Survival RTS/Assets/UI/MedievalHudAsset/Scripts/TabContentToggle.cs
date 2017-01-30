using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabContentToggle : MonoBehaviour {

	Toggle toggle;
	public GameObject content;
	// Use this for initialization
	void Start () {
		this.toggle = GetComponent<Toggle> ();
	}

	public void ToggleContent()
	{
		if (toggle.isOn)
			content.SetActive (true);
		else
			content.SetActive (false);
	}

}
