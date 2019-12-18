using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InvPanel;
    public GameObject openButton;
    public GameObject closeButton;

    [HideInInspector]
    const int itemCount = 12;

    [HideInInspector]
    public Item[] items = new Item[itemCount];

    // Start is called before the first frame update
    void Start()
    {
        InvPanel.SetActive(false);
        closeButton.SetActive(false);
        openButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard shortcuts
        if (Input.GetKeyUp(KeyCode.I))
            Toggle();
    }

    public void Toggle() {
        InvPanel.SetActive(!InvPanel.activeInHierarchy);
        closeButton.SetActive(InvPanel.activeInHierarchy);
        openButton.SetActive(!closeButton.activeInHierarchy);
    }

/*    
        private int index(GameObject item) {
        for (int i=0; i<itemCount; i++) {
            if (items[i].item == item) return i;
        }
        return -1;
    }

    public void Add(GameObject item) {
        int idx = index(item);
        if (idx == -1) {
            Item newItem = new Item(item);
        } else {
            items[idx].count++;
        }
    }

    public void Remove(GameObject item) {
        int idx = index(item);
        if (idx != -1) {
            items[idx].count--;
            if (items[idx].count == 0) 
                items[idx] = null;
        }
    }
    public void Remove(Item item) {
        int idx = index(item.item);
        if (idx != -1) {
            if (items[idx].count == 0)
                items[idx] = null;
        }
    }
*/

    private void Display() {

    }
}
