using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItemEquip : MonoBehaviour
{
	public GameObject magicButton;
	public GameObject magicUI;

	public void Equip() {
		magicButton?.SetActive(true);
		magicUI.SetActive(true);
	}
}
