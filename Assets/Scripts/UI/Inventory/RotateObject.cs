using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public Transform objectTransform;
	[Range(0.01f, 1.0f)]
	public float SmoothFactor = 0.5f;
	public float RotationsSpeed = 5.0f;

	float angleX = 0f;
	
	void LateUpdate() {

		if (Input.GetMouseButton(0)) {
			angleX -= Input.GetAxis("Mouse X") * RotationsSpeed;
			objectTransform.rotation = Quaternion.Euler(0, angleX, 0); ;
		}

	}
}
