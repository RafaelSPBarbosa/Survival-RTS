using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelBar : MonoBehaviour {

	Text text;
	public Image image;
	public int Current = 12534;
	public int ToNextLevel = 28000;
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
		text.text = Current.ToString () + " / " + ToNextLevel.ToString();
	}
}
