using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactsPanelUI : MonoBehaviour
{

    public Toggle SunButton;
    public Toggle MoonButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        if (MagicSystem.Instance) {
            if (MagicSystem.Instance.m_SelectedArtefact == MagicSystem.SelectedArtefact.Sun)
                SunButton.Select();
            if (MagicSystem.Instance.m_SelectedArtefact == MagicSystem.SelectedArtefact.Moon)
                MoonButton.Select();
        }
   }

}
