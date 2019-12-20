using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNJ : InteractableObject
{
    public string name;
    public Sprite image;

    public override bool IsInteractable => true;

    public override void InteractWith(HighlightableObject target) { }

}
