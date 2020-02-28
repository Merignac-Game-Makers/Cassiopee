using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : MonoBehaviour
{
	public Transform target;
	public float moveSpeed = 5f;
	public float turnSpeed = 5f;
	public float radius = 20f;
	public float altitude = 10f;

	private readonly Vector3 down = new Vector3(0, -1, 0);

	void Update() {
		Vector3 direction = target.position - transform.position;
		float dist = Vector3.Distance(target.position, transform.position);

		if (dist >= radius) {
			transform.LookAt(Aim());
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		} else {
			transform.RotateAround(target.position, -down, turnSpeed);
			transform.forward = Vector3.Cross(down, direction);
			transform.position = new Vector3(transform.position.x, target.position.y + altitude, transform.position.z);
		}
	}

	private Vector3 Aim() {
		Vector3 direction = target.position - transform.position;
		Vector3 perp = Vector3.Cross(down, direction).normalized * radius;
		Vector3 aim = new Vector3(target.position.x + perp.x, target.position.y + altitude, target.position.z + perp.z);
		return aim;
	}
}