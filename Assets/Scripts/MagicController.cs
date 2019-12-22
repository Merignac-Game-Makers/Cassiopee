using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
	public LineRenderer lineRendered;
	public Material magicMaterial;

	[HideInInspector]
	public List<Activable> magicActivatedItems;

	public static MagicController Instance;

	[HideInInspector]
	List<Activable> cassiopee;

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		cassiopee = new List<Activable>();
		cassiopee.AddItem(GameObject.Find("SphereA"));
		cassiopee.AddItem(GameObject.Find("SphereB"));
		cassiopee.AddItem(GameObject.Find("Well"));
		cassiopee.AddItem(GameObject.Find("SphereC"));
		cassiopee.AddItem(GameObject.Find("SphereD"));
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
			for (int i = magicActivatedItems.Count - 1; i >= idx; i--) {
				Activable obj = magicActivatedItems[i];
				Destroy(obj.GetComponent<Electric>());
				Destroy(obj.GetComponentInChildren<LineRenderer>()?.gameObject);
				magicActivatedItems.RemoveAt(i);
				obj.m_IsActive = false;
				obj.Highlight(false);
			}
		}
		if (TestSuccess()) {
			Debug.Log("DONE !!!");
			PlayerControl.Instance.gameObject.GetComponentInChildren<MeshRenderer>().material = magicMaterial;
		}
	}

	bool TestSuccess() {
		return magicActivatedItems.IsLke(cassiopee);
	}
}
public static class Extensions
{
	public static Activable Last(this List<Activable> list) {
		return list[list.Count - 1];
	}
	public static void AddItem(this List<Activable> list, GameObject item) {
		if (item.GetComponentInChildren<Activable>() != null)
			list.Add(item.GetComponentInChildren<Activable>());
	}
	public static bool IsLke(this List<Activable> list, List<Activable> other) {
		if (list.Count != other.Count)
			return false;
		else {
			for (int i = 0; i < list.Count; i++) {
				if (list[i] != other[i])
					return false;
			}
			return true;
		}

	}
}
