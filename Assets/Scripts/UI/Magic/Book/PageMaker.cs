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
	public VerticalLayoutGroup leftContent;

	public GameObject rightPage;
	public VerticalLayoutGroup rightContent;


	public abstract void Make();

	public virtual void SetSide(Side side) {
		leftPage.SetActive(side == left);
		rightPage.SetActive(side == right);
	}

	public virtual void Clear() {
		Clear(leftContent);
		Clear(rightContent);
	}
	public virtual void Clear(VerticalLayoutGroup content) {
		foreach (Transform child in content.transform) {
			Destroy(child.gameObject);
		}
	}
}
