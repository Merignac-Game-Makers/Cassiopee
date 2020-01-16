using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
	public Book controlledBook;
	bool isOn = false;
	MagicUI ui;

	// Start is called before the first frame update
	void Start() {
		ui = MagicUI.Instance;
	}

	public void SetHelp() {
		isOn = !isOn;
		controlledBook.C2.ToggleHelp(isOn);
		controlledBook.StartUpdateSprites2();
		//ui.picture.sprite = ui.GetPage().helpPicture;
	}

}
