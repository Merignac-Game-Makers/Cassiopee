using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : MonoBehaviour
{
	public Transform target;
	public float moveSpeed = 5f;
	public float turnSpeed = 5f;

	private readonly Vector3 down = new Vector3(0, -1, 0);

	void Update() {
		Vector3 direction = target.position - transform.position;
		float dist = Vector3.Distance(target.position, transform.position);

		if (dist >= 10f) {
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = rotation;
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		} else {
			transform.RotateAround(target.position, -down, turnSpeed);
			transform.Rotate(0, 2, 0, Space.Self);
			transform.forward = Vector3.Cross(down, direction);
		}
	}
}