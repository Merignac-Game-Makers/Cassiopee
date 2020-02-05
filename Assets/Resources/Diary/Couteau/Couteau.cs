using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couteau : ChapterManager
{
	public GameObject lamePrefab;
	public GameObject manchePrefab;

	bool lame = false;
	bool manche = false;

	public override void Act(Item item) {
		base.Act(item);
		if (item.WorldObjectPrefab == lamePrefab) Lame();
		if (item.WorldObjectPrefab == manchePrefab) Manche();
	}


	public void Lame() {
		lame = true;
		chapter.paragraphs[1].enabled = true;
		if (!manche) chapter.SetParagraphVersion(1, 1);
		else chapter.SetParagraphVersion(1, 2);
	}
	public void Manche() {
		manche = true;
		chapter.paragraphs[1].enabled = true;
		if (!lame) chapter.SetParagraphVersion(1, 0);
		else chapter.SetParagraphVersion(1, 2);
	}

	public void Assemblage() {
		chapter.SetParagraphVersion(1, 3);
	}
}
