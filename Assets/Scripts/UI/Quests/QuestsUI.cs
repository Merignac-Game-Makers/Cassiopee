using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : UIBase
{
    public static QuestsUI Instance;

    public UIButton bookButton;
    public GameObject bookPanel;

    public GameObject pending;
    public GameObject over;
    public GameObject questPrefab;

    UIManager uiManager;

    public override void Init(UIManager uiManager) {
        Instance = this;
        this.uiManager = uiManager;

        gameObject.SetActive(true);
        panel.SetActive(false);
    }

    public override void Toggle() {
        panel.SetActive(!isOn);
        uiManager.ManageButtons();
    }

    public void AddQuest(QuestBase quest) {
        var questBox = Instantiate(questPrefab, pending.transform);
        questBox.GetComponent<SetQuestPanel>().Init(quest);
    }
    public void TerminateQuest(QuestBase quest) {
        for (int i=0; i < pending.transform.childCount; i++) {
            var questBox = pending.transform.GetChild(i);
            if (questBox.GetComponent<SetQuestPanel>().title.text == quest.title) {
                questBox.transform.SetParent(over.transform);
                break;
            }
        }
    }
}
