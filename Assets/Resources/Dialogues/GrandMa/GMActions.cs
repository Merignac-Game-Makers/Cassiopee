using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMActions : MonoBehaviour
{
	public GameObject cupboard;
	public GameObject magicItems;

	public void OpenCupboard() {
		if (magicItems != null)
			magicItems.SetActive(true);
		var anim = cupboard.gameObject.GetComponentInChildren<Animator>();
		if (anim) {
			anim.Play("OpenDoors");
		}
	}

}
