using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineUI : MonoBehaviour
{
    public Transform objectHolder;
    [Range(60f, 250f)]
    public float size = 150;

    public void SetObject(Item item) {
        Clear();
        GameObject obj = Instantiate(item.WorldObjectPrefab, objectHolder, false);
        item.animate = false;
        obj.transform.localPosition = Vector3.zero;
        objectHolder.GetComponent<RotateObject>().objectTransform = obj.transform;
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        StartCoroutine(IScale(obj));
    }

    void Clear() {
        for(int i= objectHolder.childCount-1; i>=0; i--) {
            Destroy(objectHolder.GetChild(i).gameObject);
        }
    }

    IEnumerator IScale(GameObject obj) {
        yield return new WaitForEndOfFrame();
        var colliders = obj.GetComponentsInChildren<Collider>();
        float extents = 0;
        foreach (Collider collider in colliders) {
            extents = Mathf.Max(extents, collider.bounds.extents.magnitude);
        }
        obj.transform.localScale = Vector3.one / extents * size;
    }
}
