using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSystem : MonoBehaviour
{

	public Page[] pages;

	public static BookSystem Instance;

	[HideInInspector]
	public int MaxPage => pages.Length;

	private void Awake() {
		Instance = this;
	}


	public Page GetNextPage(BookUI ui) {
		if (ui.currentPageIdx + 1 < MaxPage) {
			ui.currentPageIdx++;
			return pages[ui.currentPageIdx];
		} else {
			return null;
		}
	}

	public Page GetPrevPage(BookUI ui) {
		if (ui.currentPageIdx > 0) {
			ui.currentPageIdx--;
			return pages[ui.currentPageIdx];
		} else {
			return null;
		}
	}
}
