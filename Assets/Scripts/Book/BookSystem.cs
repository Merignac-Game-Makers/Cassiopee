using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSystem : MonoBehaviour
{

	public Page[] pages;

	[HideInInspector]
	public int MaxPage => pages.Length;


	public Page GetNextPage(BookUI ui) {
		if (ui.currentPage + 1 < MaxPage) {
			ui.currentPage++;
			return pages[ui.currentPage];
		} else {
			return null;
		}
	}

	public Page GetPrevPage(BookUI ui) {
		if (ui.currentPage > 0) {
			ui.currentPage--;
			return pages[ui.currentPage];
		} else {
			return null;
		}
	}
}
