using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couteau : ChapterManager
{
	public GameObject lamePrefab;
	public GameObject manchePrefab;
	public GameObject couteauPrefab;

	bool lame = false;
	bool manche = false;

	public override void Act(Item item) {
		base.Act(item);
		if (item.WorldObjectPrefab.name == lamePrefab.name) 
			Lame();
		if (item.WorldObjectPrefab.name == manchePrefab.name) 
			Manche();
		if (item.WorldObjectPrefab.name == couteauPrefab.name) 
			Assemblage();
	}


	public void Lame() {
		lame = true;
		chapter.paragraphs[1].enabled = true;
		if (!manche) 
			chapter.SetParagraphVersion(1, 0);
		else 
			chapter.SetParagraphVersion(1, 2);
	}
	public void Manche() {
		manche = true;
		chapter.paragraphs[1].enabled = true;
		if (!lame) 
			chapter.SetParagraphVersion(1, 1);
		else 
			chapter.SetParagraphVersion(1, 2);
	}

	public void Assemblage() {
		chapter.SetParagraphVersion(1, 3);
	}
}
