using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	[Range(0 , 100)]
	public int _Food;

	[Range(0 , 100)]
	public int _Water;

	public int _Housing;

	public int _Units;

	private SelectionManager _SM;

	[SerializeField]
	private float TimeUntilFoodLoss = 10.0f;
	[SerializeField]
	private float TimeUntilWaterLoss = 7.0f;

	public Slider _FoodBar;
	public Slider _WaterBar;
	public Text _HousingText;

	void Start(){

		_SM = GetComponent<SelectionManager> ();

		StartCoroutine ("DecreaseFood");
		StartCoroutine ("DecreaseWater");
		UpdateHousing ();
	}

	void Update(){


		_Food = Mathf.Clamp (_Food, 0, 100);
		_Water = Mathf.Clamp (_Water, 0, 100);
		_FoodBar.value = _Food / 100.0f;
		_WaterBar.value = _Water / 100.0f;
	}

	public void UpdateHousing (){

		_Units = _SM.UnitsList.Count;
		_HousingText.text = _Units.ToString() + "/" + _Housing.ToString();
	}

	public void IncreaseFood(int Ammount){

		_Food += Ammount;
		_Food = Mathf.Clamp (_Food, 0, 100);
	}

	public void IncreaseWater(int Ammount){

		_Water += Ammount;
		_Water = Mathf.Clamp (_Water, 0, 100);
	}

	private IEnumerator DecreaseFood (){

		while (_Food > 0) {
			
			if(_Units > 0){
				
				yield return new WaitForSeconds (TimeUntilFoodLoss / _Units);
				_Food--;
				_Food = Mathf.Clamp (_Food, 0, 100);

			}
		}
	}

	private IEnumerator DecreaseWater (){

		while (_Water > 0) {

			if(_Units > 0){

				yield return new WaitForSeconds (TimeUntilWaterLoss / _Units);
				_Water--;
				_Water = Mathf.Clamp (_Water, 0, 100);
			}
		}
	}
}