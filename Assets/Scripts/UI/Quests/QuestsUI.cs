using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : UIBase
{
    public static QuestsUI Instance;

    public UIButton bookButton;
    public GameObject panel;
    public GameObject content;
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
        Instantiate(questPrefab, content.transform);
        questPrefab.GetComponent<SetQuestPanel>().Init(quest);
    }

}
