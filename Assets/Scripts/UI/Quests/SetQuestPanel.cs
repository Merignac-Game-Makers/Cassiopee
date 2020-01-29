using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetQuestPanel : MonoBehaviour
{
	public Image picture;
	public TMP_Text title;
	public TMP_Text text;
	public Image checkBox;

	[HideInInspector]
	public QuestBase quest;

	public void Init(QuestBase quest) {
		SetQuest(quest);
	}

	public void SetQuest(QuestBase quest) {
		this.quest = quest;
		if (quest != null) {
			picture.sprite = quest.picture;
			title.text = quest.title;
			text.text = quest.shortText;
			checkBox.enabled = quest.IsDone();
			gameObject.SetActive(true);
		}
	}


}
