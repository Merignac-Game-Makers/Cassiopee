using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool state = false;
    public GameObject panel;
    public Text dialogText;

    void Start()
    {
        panel.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (state && Input.GetMouseButtonDown(0))
        {
            state = false;
            panel.SetActive(state);
        }
    }

    public void DisplayText(string textToDisplay)
    {
        dialogText.text = textToDisplay;
        state = true;
        panel.SetActive(state);
    }
}
