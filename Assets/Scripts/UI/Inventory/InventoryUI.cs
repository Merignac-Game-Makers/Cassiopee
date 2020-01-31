using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

/// <summary>
/// Handle all the UI code related to the inventory (drag'n'drop of object, using objects, equipping object etc.)
/// </summary>
public class InventoryUI : UIBase
{
	public class DragData
	{
		public EntryUI draggedEntry;
		public RectTransform originalParent;
	}

	public GameObject combinePanel;
	public GameObject messageLabel;
	public GameObject bookPanel;
	public GameObject content;

	public ItemEntryUI itemEntryPrefab;
	public RectTransform slotPrefab;

	public Canvas DragCanvas;

	public Image topButton;
	public Sprite invUp;
	public Sprite invDown;

	public static InventoryUI Instance;

	// Raycast
	readonly RaycastHit[] m_RaycastHitCache = new RaycastHit[16];
	int m_TargetLayer;


	public DragData currentlyDragged { get; set; }
	public CanvasScaler DragCanvasScaler { get; private set; }

	public List<ItemEntryUI> entries { get; private set; } = new List<ItemEntryUI>();

	EntryUI hoveredItem;
	HighlightableObject item;
	UIManager uiManager;

	bool? prevStatus = false;

	[HideInInspector]
	public EntryUI selectedEntry;

	public override void Init(UIManager uiManager) {
		Instance = this;
		this.uiManager = uiManager;
		combinePanel.SetActive(false);

		gameObject.SetActive(true);

		currentlyDragged = null;

		DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>();

		m_TargetLayer = 1 << LayerMask.NameToLayer("Interactable");
	}

	void OnEnable() {
		hoveredItem = null;
	}

	public ItemEntryUI AddItemEntry(int idx, InventoryEntry inventoryEntry) {
		RectTransform slot = Instantiate(slotPrefab, content.transform);        // créer un nouvel emplacement
		ItemEntryUI itemEntry = Instantiate(itemEntryPrefab, slot);             // créer une nouvelle entrée d'inventaire dans cet emplacement																				//itemEntry.gameObject.SetActive(true);
		itemEntry.Init(inventoryEntry);
		if (entries.Count == 0)                                                 // si c'est le 1er objet
			Show();                                                             // montrer l'inventaire
		entries.Add(itemEntry);
		return itemEntry;
	}

	/// <summary>
	/// détruire une entrée
	/// </summary>
	/// <param name="entryUi"></param>
	public void RemoveEntry(ItemEntryUI entryUi) {
		Destroy(entryUi.transform.parent.gameObject);       // détruire le slot qui contient l'entrée
		entries.Remove(entryUi);
		if (entries.Count == 0)                                                 // si l'inventaire est vide
			Hide();                                                             // cacher l'inventaire
	}

	/// <summary>
	/// bascule d'affichage
	/// </summary>
	public override void Toggle() {
		if (panel.transform.position.y >= 0) {
			Hide();
		} else {
			Show();
		}
	}

	public void Hide() {
		if (panel.transform.position.y >= 0) {
			panel.GetComponentInChildren<Animator>().SetTrigger("Down");
			topButton.sprite = invUp;
			foreach(ItemEntryUI entry in panel.GetComponentsInChildren<ItemEntryUI>()) {
				entry.Select(false);
			}
			combinePanel.SetActive(false);
		}
	}

	public void Show() {
		if (panel.transform.position.y < 0) {
			panel.GetComponentInChildren<Animator>().SetTrigger("Up");
			topButton.sprite = invDown;
		}
	}

	/// <summary>
	/// Actualiser l'affichage de toutes les entrés d'iventaire
	/// </summary>
	/// <param name="item"></param>
	public void UpdateEntries(HighlightableObject item) {
		this.item = item;
		for (int i = entries.Count - 1; i > 0; i--) {
			if ((entries[i].entry as InventoryEntry).count <= 0) {
				Destroy(entries[i].gameObject);
				entries.RemoveAt(i);
			} else {
				entries[i].UpdateEntry();
			}
		}
	}

	/// <summary>
	/// utiliser un objet (ex: boire une potion...)
	/// (inutilisé pour l'instant)
	/// </summary>
	/// <param name="usedItem"></param>
	public void ObjectDoubleClicked(Entry usedItem) {
		InventoryManager.Instance.UseItem(usedItem);
		ObjectHoverExited(hoveredItem);
		//UpdateEntries(item);
	}


	public void ObjectHoveredEnter(EntryUI hovered) {
		hoveredItem = hovered;
	}

	public void ObjectHoverExited(EntryUI exited) {
		if (hoveredItem == exited) {
			hoveredItem = null;
		}
	}

	public void HandledDroppedEntry(Vector3 position) {
		// check for drop on ItemSlots
		for (int i = 0; i < content.transform.childCount; ++i) {                                // pour chaque slot
			var slot = content.transform.GetChild(i).GetComponent<RectTransform>();
			if (RectTransformUtility.RectangleContainsScreenPoint(slot, position)) {            // si on lache sur ce slot
				var entryUi = slot.GetComponentInChildren<ItemEntryUI>();                       // récuperer l'entrée contenue dans ce slot

				if (entryUi != null) {                                                          // s'il y a déjà une entrée => déplacer l'entrée
					var prevParent = entryUi.transform.parent;
					entryUi.transform.SetParent(currentlyDragged.originalParent, false);        // vers le slot vide
					currentlyDragged.originalParent = prevParent as RectTransform;
					currentlyDragged.draggedEntry.UpdateEntry();                                // mettre l'entrée déposée à jour
					return;
				}
			}
		}
		// check for drop on 3D target
		//DropOn3D(PlayerManager.Instance.m_InvItemDragging.draggedEntry.entry);
		DropOn3D(currentlyDragged.draggedEntry.entry);
	}

	public void DropOn3D(Entry entry) {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);         // lancer de rayon
		int count = Physics.SphereCastNonAlloc(screenRay, 1.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);     // combien d'objets sous le pointeur ?
		if (count > 0) {                                                            // s'il y a des objets sous le pointeur
			foreach (RaycastHit rh in m_RaycastHitCache) {                          // pour chacun d'eux
				if (rh.collider != null) {                                          // si l'objet a un collider
					if (entry is InventoryEntry) {
						bool combinable = (entry as InventoryEntry).item.combinable;
						Target data = rh.collider.GetComponentInParent<Target>();       // si l'objet est une 'target' (lieu de dépôt d'objet d'inventaire autorisé)
						if (data != null && data.isFree && !combinable) {               // et que cet emplacement est libre et que l'objet actuel n'est pas combinable
							PlayerManager.Instance.RequestInteraction(data);
							//DropItem(data, entry as InventoryEntry);                    // déposer l'objet d'inventaire
						} else {
							Loot other = rh.collider.GetComponentInParent<Loot>();
							if (other != null && combinable) {                          // si l'item actuellement sélectionné dans l'inventaire est combinable
								Debug.Log("Combine");                                       // combiner
							} else {
								ShowLabel("Impossible d'utiliser cet objet ici", Input.mousePosition);
							}
						}
						break;
					} else if (entry is OrbEntryUI.OrbEntry) {
						// si l'objet est une ' magic target' (lieu de dépôt d'objet d'inventaire autorisé)
						MagicEffectBase data = rh.collider.GetComponentInChildren<MagicEffectBase>();
						if (data != null && data.isFree) {                              // et que cet emplacement est libre
							DropItem(data, entry as OrbEntryUI.OrbEntry);               // déposer l'objet d'inventaire
							break;
						} else {
							ShowLabel("Impossible ici", Input.mousePosition);
						}
						break;
					}
				}
			}
		}
	}

	public void DropItem(Target target, Entry entry) {
		if (entry is InventoryEntry) {
			CreateWorldRepresentation((entry as InventoryEntry).item, target);                                              // créer l'objet 3D
			if (currentlyDragged != null)                                                               // si on es dans un 'drag & drop'
				currentlyDragged.draggedEntry.transform.SetParent(currentlyDragged.originalParent);     // rattacher le 'drag & drop' à son parent original
			InventoryManager.Instance.RemoveItem(entry as InventoryEntry);                                                // retirer l'objet déposé de l'inventaire
		}
	}

	private void DropItem(MagicEffectBase target, OrbEntryUI.OrbEntry entry) {
		if (target != null && target.isFree) {          // si on lâche l'orbe sur une cible de magie
			target.MakeMagicalStuff(entry.orb);         // déclencher la magie
			entry.ui.Select(false);
		}

	}

	void CreateWorldRepresentation(Item item, Target target) {
		var pos = target.gameObject.transform.position + Vector3.up * item.WorldObjectPrefab.gameObject.transform.localScale.y / 2;
		// if the item have a world object prefab set use that...
		if (item.WorldObjectPrefab != null) {
			var obj = Instantiate(item.WorldObjectPrefab, pos, new Quaternion());
			obj.transform.parent = target.gameObject.transform;
			obj.layer = LayerMask.NameToLayer("Interactable");
		}
	}

	void ShowLabel(string text, Vector2 position) {
		messageLabel.GetComponentInChildren<TMP_Text>().text = text;
		messageLabel.transform.position = position;
		StartCoroutine(IShow(messageLabel, 2));
	}

	IEnumerator IShow(GameObject obj, float s) {
		obj.SetActive(true);
		yield return new WaitForSeconds(s);
		obj.SetActive(false);
	}

	public void SaveAndHide() {
		prevStatus = panel.transform.position.y >= 0;
		Hide();
	}

	public void Restore() {
		if (prevStatus != null && prevStatus != panel.transform.position.y >= 0)
			Toggle();
		prevStatus = null;
	}
}
