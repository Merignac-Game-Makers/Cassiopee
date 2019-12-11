using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerDialogManager : MonoBehaviour
{
    public DialogManager npcDialogManager;
    public string dialogText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision !");
            npcDialogManager.DisplayText(dialogText);
        }
    }
}
