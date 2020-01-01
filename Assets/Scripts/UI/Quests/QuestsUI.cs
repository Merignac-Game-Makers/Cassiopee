using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : UIBase
{
	public static QuestsUI Instance;
	public QuestManager questSystem;

	public GameObject pending;
	public GameObject over;
	public GameObject questPrefab;

	UIManager uiManager;

	public override void Init(UIManager uiManager) {
		Instance = this;
		questSystem.Init();

		this.uiManager = uiManager;

		gameObject.SetActive(true);
		panel.SetActive(false);
	}

	public override void Toggle() {
		panel.SetActive(!isOn);
		if (isOn)
			SetQuests();
		uiManager.ManageButtons();
	}

	private void SetQuests() {
		// update pending quests list
		pending.transform.DetachChildren();                         // pending quests : remove all existing children
		foreach (QuestBase quest in questSystem.GetPending()) {      // attach all pending quests
			var questBox = Instantiate(questPrefab, pending.transform);
			questBox.GetComponent<SetQuestPanel>().Init(quest);
		}

		// update done quests list
		over.transform.DetachChildren();                          // done quests : remove all existing children
		foreach (QuestBase quest in questSystem.GetDone()) {      // attach all done quests
			var questBox = Instantiate(questPrefab, over.transform);
			questBox.GetComponent<SetQuestPanel>().Init(quest);
		}
	}

	public void AddQuest(QuestBase quest) {
		var questBox = Instantiate(questPrefab, pending.transform);
		questBox.GetComponent<SetQuestPanel>().Init(quest);
	}

	public void TerminateQuest(QuestBase quest) {
		for (int i = 0; i < pending.transform.childCount; i++) {
			var questBox = pending.transform.GetChild(i);
			if (questBox.GetComponent<SetQuestPanel>().title.text == quest.title) {
				questBox.transform.SetParent(over.transform);
				break;
			}
		}
	}
}
