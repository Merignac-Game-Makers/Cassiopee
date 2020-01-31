using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour {

	public Transform objectTransform;
	[Range(0.01f, 1.0f)]
	public float SmoothFactor = 0.5f;
	public float RotationsSpeed = 5.0f;

	private Vector3 cameraOffset;

	void Start() {
		cameraOffset = transform.position - objectTransform.position;
	}

	void LateUpdate() {

		if (Input.GetMouseButton(0)) {
			Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
			cameraOffset = camTurnAngle * cameraOffset;
			Vector3 newPos = objectTransform.position + cameraOffset;

			transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
			transform.LookAt(objectTransform);
		}

	}
}
