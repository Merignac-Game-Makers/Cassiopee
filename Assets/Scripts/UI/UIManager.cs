using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BookUI bookUI;
    public InventoryUI inventoryUI;
    //public DialogUI dialogUI;

    // Start is called before the first frame update
    void OnEnable()
    {
        bookUI.gameObject.SetActive(true);
        bookUI.Init();

        inventoryUI.gameObject.SetActive(true);
        inventoryUI.Init();

        //dialogUI.gameObject.SetActive(true);
        //dialogUI.Init();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
