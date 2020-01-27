using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public abstract class PageMaker : MonoBehaviour
{
	public enum Side { left, right }

	public GameObject leftPage;
	public GameObject rightPage;

	public abstract void Make();

	public abstract void SetSide(Side side);

}
