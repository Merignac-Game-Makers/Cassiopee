using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Arrival : ChapterManager
{
	public void GrandMaMeeting() {
		chapterInstance.SetParagraphNext(0); // paragraphe 0 => avancement à l'étape suivante
	}
}
