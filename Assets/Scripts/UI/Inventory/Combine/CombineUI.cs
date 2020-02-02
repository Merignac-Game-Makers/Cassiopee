using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineUI : MonoBehaviour
{
    public Transform objectHolder;
    [Range(60f, 250f)] public float size = 200;

    public Item item { get; private set; }
    public InventoryEntry entry { get; private set; }

    private void Start() {
        GetComponentInChildren<RawImage>().SizeToParent();
    }

    public void SetObject(InventoryEntry entry) {
        Clear();
        this.entry = entry;
        item = entry.item;
        GameObject obj = Instantiate(item.WorldObjectPrefab, objectHolder, false);
        item.animate = false;
        SetObjLayer(obj);
        obj.transform.localPosition = Vector3.zero;
        objectHolder.GetComponent<RotateObject>().objectTransform = obj.transform;
        gameObject.SetActive(true);                                                 // afficher le panneau
        StartCoroutine(IScale(obj));                                                // taille
    }

    public void Clear() {
        item = null;
        for(int i= objectHolder.childCount-1; i>=0; i--) {
            Destroy(objectHolder.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);                                                // masquer panneau
    }

    void SetObjLayer(GameObject obj) {
        var layer = LayerMask.NameToLayer("Interactable");                          // layer des objets intéractibles
        obj.layer = layer;
        for (int i = 0; i < obj.transform.childCount; i++)
            obj.transform.GetChild(i).gameObject.layer = layer;
    }

    IEnumerator IScale(GameObject obj) {
        yield return new WaitForEndOfFrame();
        var colliders = obj.GetComponentsInChildren<Collider>();
        float extents = 0;
        foreach (Collider collider in colliders) {
            extents = Mathf.Max(extents, collider.bounds.extents.magnitude);
        }
        obj.transform.localScale = Vector3.one / extents * size;
        //obj.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
