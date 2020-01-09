using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    MagicUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = MagicUI.Instance;
    }

    public void SetHelp() {
        ui.picture.sprite = ui.GetPage().helpPicture;
    }

}
