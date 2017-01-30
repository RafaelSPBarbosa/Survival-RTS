using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	private Camera _Camera;

	[SerializeField]
	private Camera _OutLineCamera;

	[SerializeField]
	private Camera _InteractableOutLineCamera;

	[SerializeField]
	private BoxCollider _Bounds;

	[SerializeField]
	private float _Speed , _MinZoom = 25.0f, _MaxZoom = 75.0f;

	public Vector2 MinPos, MaxPos;

	void Update () {
	
		transform.Translate (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")) * _Speed);


		Vector3 pos = transform.position;

		pos.x = Mathf.Clamp (pos.x , MinPos.x, MaxPos.x);
		pos.z = Mathf.Clamp (pos.z , MinPos.y, MaxPos.y);

		transform.position = pos;


		if (_Camera.fieldOfView > _MinZoom) {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {

				_Camera.fieldOfView -= 5;
			}
		}

		if (_Camera.fieldOfView < _MaxZoom) {
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {

				_Camera.fieldOfView += 5;
			}
		}

		_OutLineCamera.fieldOfView = _Camera.fieldOfView;
		_InteractableOutLineCamera.fieldOfView = _Camera.fieldOfView;
	}
}