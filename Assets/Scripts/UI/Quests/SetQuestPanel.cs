using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetQuestPanel : MonoBehaviour
{
	public Image picture;
	public Text title;
	public TMP_Text text;
	public void Init(QuestBase quest) {
		picture.sprite = quest.picture;
		title.text = quest.title;
		text.text = quest.shortText;
	}
}
