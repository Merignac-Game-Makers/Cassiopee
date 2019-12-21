using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public BookUI bookUI;
	public InventoryUI inventoryUI;
	public DialoguesUI dialoguesUI;
	public QuestsUI questsUI;

	public UIButton bookButton;


	// Start is called before the first frame update
	void OnEnable() {
		bookUI.Init(this);
		inventoryUI.Init(this);
		dialoguesUI.Init(this);
		questsUI.Init(this);

	}

	public void ManageButtons() {
		//bookUI.Show(!inventoryUI.isOn && !questsUI.isOn);
		bookButton.gameObject.SetActive(!inventoryUI.isOn && !questsUI.isOn);
	}
}
