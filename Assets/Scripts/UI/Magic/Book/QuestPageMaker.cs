using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public class QuestPageMaker : PageMaker
{
	public Image background;
	public Sprite leftBackground;
	public Sprite rightBackground;

	public TMP_Text title;
	public VerticalLayoutGroup content;

	public GameObject questContainerPrefab;

	Side side = left;

	public override void Make() {
		title.text = "Objectifs";


	}

	public override void SetSide(Side side) {
		this.side = side;
		title.enabled = side == left;
		background.sprite = side == left ? leftBackground : rightBackground; 
	}


}
