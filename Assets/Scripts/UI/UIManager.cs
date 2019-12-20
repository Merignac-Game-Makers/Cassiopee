using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BookUI bookUI;
    public InventoryUI inventoryUI;
    public DialoguesUI dialoguesUI;
    public QuestsUI questsUI;



    // Start is called before the first frame update
    void OnEnable()
    {
        bookUI.Init();
        inventoryUI.Init();
        dialoguesUI.Init();
        questsUI.Init();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
