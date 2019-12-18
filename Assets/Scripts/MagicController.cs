using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public Material electricMaterial;
    public List<GameObject> magicActivatedItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(GameObject item)
    {
        Debug.Log("add " + item.name);
        if (magicActivatedItems.Count > 0)
        {
            Electric electicScript = gameObject.AddComponent<Electric>();
            electicScript.transformPointA = magicActivatedItems[magicActivatedItems.Count - 1].transform;
            electicScript.transformPointB = item.transform;

            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            lr.material = electricMaterial;
        }
        magicActivatedItems.Add(item);
    }
}
