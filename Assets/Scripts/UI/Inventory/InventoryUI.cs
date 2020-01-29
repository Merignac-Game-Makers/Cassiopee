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
		public ItemEntryUI DraggedEntry;
		public RectTransform OriginalParent;
	}

	public GameObject messageLabel;
	public GameObject bookPanel;
	public GameObject content;

	float size;

	public ItemEntryUI itemEntryPrefab;
	public RectTransform slotPrefab;

	public Canvas DragCanvas;

	public static InventoryUI Instance;

	// Raycast
	readonly RaycastHit[] m_RaycastHitCache = new RaycastHit[16];
	int m_TargetLayer;


	public DragData currentlyDragged { get; set; }
	public CanvasScaler DragCanvasScaler { get; private set; }

	public List<ItemEntryUI> entries { get; private set; } = new List<ItemEntryUI>();

	ItemEntryUI hoveredItem;
	HighlightableObject item;
	UIManager uiManager;

	bool? prevStatus = false;

	[HideInInspector]
	public ItemEntryUI selectedEntry;

	public override void Init(UIManager uiManager) {
		Instance = this;
		this.uiManager = uiManager;

		gameObject.SetActive(true);

		currentlyDragged = null;

		DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>();

		m_TargetLayer = 1 << LayerMask.NameToLayer("Interactable");

		size = content.GetComponent<RectTransform>().rect.height;       // slot
		var rect = GetComponent<RectTransform>().rect;                  // carré
		rect.height = size;                                             //
	}

	void OnEnable() {
		hoveredItem = null;
		//Tooltip.gameObject.SetActive(false);
	}

	public ItemEntryUI AddItemEntry(int idx, InventoryEntry inventoryEntry) {
		RectTransform slot = Instantiate(slotPrefab, content.transform);        // créer un nouvel emplacement
		ItemEntryUI itemEntry = Instantiate(itemEntryPrefab, slot);             // créer une nouvelle entrée d'inventaire dans cet emplacement																				//itemEntry.gameObject.SetActive(true);
		itemEntry.Init(this, inventoryEntry);
		if (entries.Count == 0)													// si c'est le 1er objet
			Show();																// montrer l'inventaire
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
		if (entries.Count == 0)													// si l'inventaire est vide
			Hide();																// cacher l'inventaire
	}

	/// <summary>
	/// bascule d'affichage
	/// </summary>
	public override void Toggle() {
		if (panel.transform.position.y >= 0)
			panel.GetComponentInChildren<Animator>().SetTrigger("Down");
		else
			panel.GetComponentInChildren<Animator>().SetTrigger("Up");
	}

	public void Hide() {
		if (panel.transform.position.y >= 0)
			panel.GetComponentInChildren<Animator>().SetTrigger("Down");
	}

	public void Show() {
		if (panel.transform.position.y < 0)
			panel.GetComponentInChildren<Animator>().SetTrigger("Up");
	}

	/// <summary>
	/// Actualiser l'affichage de toutes les entrés d'iventaire
	/// </summary>
	/// <param name="item"></param>
	public void UpdateEntries(HighlightableObject item) {
		this.item = item;
		for (int i = entries.Count - 1; i > 0; i--) {
			if (entries[i].inventoryEntry.count <= 0) {
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
	public void ObjectDoubleClicked(InventoryEntry usedItem) {
		InventoryManager.Instance.UseItem(usedItem);
		ObjectHoverExited(hoveredItem);
		//UpdateEntries(item);
	}


	public void ObjectHoveredEnter(ItemEntryUI hovered) {
		hoveredItem = hovered;

		//Tooltip.gameObject.SetActive(true);

		// Item itemUsed = hoveredItem.inventoryEntry.item;

		//Tooltip.Name.text = itemUsed.ItemName;
		//Tooltip.DescriptionText.text = itemUsed.GetDescription();
	}

	public void ObjectHoverExited(ItemEntryUI exited) {
		if (hoveredItem == exited) {
			hoveredItem = null;
			//Tooltip.gameObject.SetActive(false);
		}
	}

	public void HandledDroppedEntry(Vector3 position) {
		// check for drop on ItemSlots
		for (int i = 0; i < content.transform.childCount; ++i) {								// pour chaque slot
			var slot = content.transform.GetChild(i).GetComponent<RectTransform>();
			if (RectTransformUtility.RectangleContainsScreenPoint(slot, position)) {            // si on lache sur ce slot
				var entryUi = slot.GetComponentInChildren<ItemEntryUI>();						// récuperer l'entrée contenue dans ce slot
				if (entryUi != null) {                                                          // s'il y a déjà une entrée => déplacer l'entrée
					var prevParent = entryUi.transform.parent;
					entryUi.transform.SetParent(currentlyDragged.OriginalParent, false);        // vers le slot vide
					currentlyDragged.OriginalParent = prevParent as RectTransform;
					currentlyDragged.DraggedEntry.UpdateEntry();								// mettre l'entrée déposée à jour
					return;
				}
			}
		}
		// check for drop on 3D target
		DropOn3D(PlayerManager.Instance.m_InvItemDragging.DraggedEntry.inventoryEntry);
	}

	public void DropOn3D(InventoryEntry inventoryEntry) {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);			// lancer de rayon
		int count = Physics.SphereCastNonAlloc(screenRay, 1.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);		// combien d'objets sous le pointeur ?
		if (count > 0) {															// s'il y a des objets sous le pointeur
			foreach (RaycastHit rh in m_RaycastHitCache) {							// pour chacun d'eux
				if (rh.collider != null) {											// si l'objet a un collider
					Target data = rh.collider.GetComponentInParent<Target>();		// si l'objet est une 'target' (lieu de dépôt d'objet d'inventaire autorisé)
					if (data != null && data.isFree) {								// et que cet emplacement est libre
						DropItem(data, inventoryEntry);								// déposer l'objet d'inventaire
						break;
					} else {
						ShowLabel("Impossible d'utiliser cet objet ici", Input.mousePosition);
					}
				}
			}
		}
	}

	private void DropItem(Target target, InventoryEntry inventoryEntry) {
		CreateWorldRepresentation(inventoryEntry.item, target);									// créer l'objet 3D
		if (currentlyDragged!=null)																// si on es dans un 'drag & drop'
			currentlyDragged.DraggedEntry.transform.SetParent(currentlyDragged.OriginalParent);	// rattacher le 'drag & drop' à son parent original
		InventoryManager.Instance.RemoveItem(inventoryEntry);									// retirer l'objet déposé de l'inventaire
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
		if (prevStatus !=null && prevStatus != panel.transform.position.y >= 0)
			Toggle();
		prevStatus = null;
	}
}
