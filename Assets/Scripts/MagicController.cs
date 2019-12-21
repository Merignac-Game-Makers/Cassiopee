using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
	public LineRenderer lineRendered;

	[HideInInspector]
	public List<Activable> magicActivatedItems;

	public static MagicController Instance;

	private void Awake() {
		Instance = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void AddOrRemove(Activable item) {
		if (!magicActivatedItems.Contains(item)) {
			Debug.Log("add " + item.name);
			if (magicActivatedItems.Count > 0) {
				LineRenderer lr = Instantiate(lineRendered, item.transform);

				Electric electicScript = item.gameObject.AddComponent<Electric>();
				electicScript.transformPointA = magicActivatedItems.Last().transform;
				electicScript.transformPointB = item.transform;
			}
			magicActivatedItems.Add(item);
		} else {
			Debug.Log("remove " + item.name);
			int idx = magicActivatedItems.IndexOf(item);
			for (int i=magicActivatedItems.Count-1; i>=idx; i--) {
				Activable obj = magicActivatedItems[i];
				Destroy(obj.GetComponent<Electric>());
				Destroy(obj.GetComponentInChildren<LineRenderer>()?.gameObject);
				magicActivatedItems.RemoveAt(i);
				obj.m_IsActive = false;
				obj.Highlight(false);
			}
		}
	}
}
public static class Extensions
{
	public static Activable Last(this List<Activable> list) {
		return list[list.Count - 1];
	}
}
