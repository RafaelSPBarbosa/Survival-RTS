using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour {

	Text text;
	public Image image;
	// Use this for initialization
	void Start () 
	{
		text = GetComponentInChildren<Text> ();
	}

	void Update()
	{
		UpdateText ();
	}



	public void UpdateText()
	{
		text.text = (Mathf.Floor (image.fillAmount * 100)).ToString () + "%";
	}

}
