using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // if pointer is NOT over UI
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (collision.gameObject.CompareTag("Player")) {
                Debug.Log("Collision !");
                npcDialogManager.DisplayText(dialogText);
            }
        }
    }
}
