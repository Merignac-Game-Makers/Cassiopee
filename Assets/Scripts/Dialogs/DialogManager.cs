using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool state => panel.activeInHierarchy;
    public GameObject panel;
    public Text dialogText;

    public static DialogManager Instance;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        panel.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (state && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            panel.SetActive(false);
        }
    }

    public void DisplayText(string textToDisplay)
    {
        dialogText.text = textToDisplay;
        panel.SetActive(true);
        PlayerControl.Instance.StopAgent();
    }
}
