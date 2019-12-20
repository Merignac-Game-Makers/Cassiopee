using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : UIBase
{
    public static QuestsUI Instance;

    public UIButton bookButton;
    public GameObject panel;
    public GameObject pending;
    public GameObject over;
    public GameObject questPrefab;


    public override void Init() {
        Instance = this;
        gameObject.SetActive(true);
        panel.SetActive(false);
    }

    private void Update() {
        //Keyboard shortcut
        if (Input.GetKeyUp(KeyCode.Q))
            Toggle();
    }

    public override void Toggle() {
        panel.SetActive(!panel.activeInHierarchy);
        bookButton.gameObject.SetActive(!panel.activeInHierarchy);
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
