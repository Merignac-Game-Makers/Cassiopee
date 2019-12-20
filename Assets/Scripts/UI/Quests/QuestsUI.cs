using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : MonoBehaviour
{
    public static QuestsUI Instance;

    public GameObject panel;
    public GameObject openButton;
    public GameObject closeButton;
    public GameObject content;
    public GameObject questPrefab;

    public void Init() {
        Instance = this;
        gameObject.SetActive(true);
        panel.SetActive(false);
        closeButton.SetActive(false);
        openButton.SetActive(true);
    }

    private void Update() {
        //Keyboard shortcut
        if (Input.GetKeyUp(KeyCode.Q))
            Toggle();
    }

    public void Toggle() {
        panel.SetActive(!panel.activeInHierarchy);
        closeButton.SetActive(panel.activeInHierarchy);
        openButton.SetActive(!closeButton.activeInHierarchy);
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
