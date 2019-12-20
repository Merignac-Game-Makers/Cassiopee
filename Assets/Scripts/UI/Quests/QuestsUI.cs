using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : UIBase
{
    public static QuestsUI Instance;

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
        BookUI.Instance.Show(!panel.activeInHierarchy);
    }

    public void Show(bool on) {
        gameObject.SetActive(on);
    }

    public void AddQuest(QuestBase quest) {
        Instantiate(questPrefab, content.transform);
        questPrefab.GetComponent<SetQuestPanel>().Init(quest);
    }

}
