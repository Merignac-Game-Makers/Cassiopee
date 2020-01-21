using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the inventory of our character. The inventory has a maximum of 12 slot, each slot can hold one
/// TYPE of object, but those can be stacked without limit (e.g. 1 slot used by health potions, but contains 20
/// health potions)
/// </summary>
public class InventorySystem
{

	public static InventorySystem Instance;

	/// <summary>
	/// One entry in the inventory. Hold the type of Item and how many there is in that slot.
	/// </summary>
	public class InventoryEntry
	{
		public int count;
		public Item item;
	}

	// Pas de limite au nombre d'objets en inventaire
	private const int numSlots = 0;
	public List<InventoryEntry> Entries = new List<InventoryEntry>();

	CharacterData owner;

	public void Init(CharacterData owner) {
		this.owner = owner;
		Instance = this;
	}

	/// <summary>
	/// Add an item to the inventory. This will look if this item already exist in one of the slot and increment the
	/// stack counter there instead of using another slot.
	/// </summary>
	/// <param name="item">The item to add to the inventory</param>
	public void AddItem(Item item) {
		bool found = false;
		for (int i = 0; i < Entries.Count; ++i) {		// pour chaque slot existant
			//if (Entries[i] == null) {					
			//	if (firstEmpty == -1)
			//		firstEmpty = i;
			//} else 
			if (Entries[i].item == item) {				// si l'objet contenu dans le slot est identique
				Entries[i].count += 1;					// ajouter 1 à la quantité
				found = true;							// trouvé
				break;
			}
		}

		if (!found) {									// si on n'a pas trouvé
			InventoryEntry entry = new InventoryEntry();// créer un nouveau slot
			entry.item = item;							// contenant l'objet
			entry.count = 1;							// quantité = 1
			InventoryUI.Instance.AddItemEntry(Entries.Count);
			Entries.Add(entry);
		}

		// UIManager.Instance.inventoryButton.gameObject.GetComponentInParent<Animator>()?.SetTrigger("startColor");
	}

	/// <summary>
	/// This will *try* to use the item. If the item return true when used, this will decrement the stack count and
	/// if the stack count reach 0 this will free the slot. If it return false, it will just ignore that call.
	/// (e.g. a potion will return false if the user is at full health, not consuming the potion in that case)
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool UseItem(InventoryEntry item) {
		//true mean it get consumed and so would be removed from inventory.
		//(note "consumed" is a loose sense here, e.g. armor get consumed to be removed from inventory and added to
		//equipement by their subclass, and de-equiping will re-add the equipement to the inventory 
		if (item.item.UsedBy(owner)) {
			item.count -= 1;							// retirer 1 à la quantité

			if (item.count <= 0) {						// si la quantité est nulle
				for (int i = 0; i < numSlots; ++i) {	// rechercher le slot
					if (Entries[i] == item) {			
						Entries.RemoveAt(i);			// retirer le slot de l'inventaire
						break;
					}
				}
			}
			return true;
		}
		return false;
	}


	public void RemoveItem(int EntryIndex) {
		//it get consumed and so would be removed from inventory.
		//(note "consumed" is a loose sense here, e.g. armor get consumed to be removed from inventory and added to
		//equipement by their subclass, and de-equiping will re-add the equipement to the inventory 
		InventoryEntry entry = Entries[EntryIndex];						// trouver le slot
		entry.count -= 1;												// retirer 1 à la quantité
		if (entry.count <= 0) {                                         // si la quantité est nulle
			InventoryUI.Instance.RemoveItemEntry(EntryIndex);
			Entries.RemoveAt(EntryIndex);                               // retirer le slot de l'inventaire
		} else {
			InventoryUI.Instance.itemEntries[EntryIndex].UpdateEntry();
		}

	}
}
