using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicActivable : Activable
{
    public Material electricMaterial;

    static List<GameObject> activatedList;

    public void AddAndActivate()
    {
        activatedList.Add(gameObject);
        Debug.Log(activatedList.Count);
        if(activatedList.Count > 1)
        {
            Electric electicScript = gameObject.AddComponent<Electric>();
            electicScript.transformPointA = activatedList[activatedList.Count - 2].transform;
            electicScript.transformPointB = transform;

            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            lr.material = electricMaterial;
        }
    }
}
