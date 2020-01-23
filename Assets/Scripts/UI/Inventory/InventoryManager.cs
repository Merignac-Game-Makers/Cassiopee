using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the inventory of our character. Each slot can hold one
/// TYPE of object, but those can be stacked without limit (e.g. 1 slot used by health potions, but contains 20
/// health potions)
/// </summary>
public class InventoryManager
{

	public static InventoryManager Instance;
	private InventoryUI inventoryUI;

	/// <summary>
	/// One entry in the inventory. Hold the type of Item and how many there is in that slot.
	/// </summary>
	public class InventoryEntry
	{
		public Item item;
		public int count = 1;
		public ItemEntryUI ui;

		public InventoryEntry(Item item) {
			this.item = item;
		}
	}

	// Pas de limite au nombre d'objets en inventaire
	private const int numSlots = 0;
	public List<InventoryEntry> entries = new List<InventoryEntry>();

	CharacterData owner;

	public void Init(CharacterData owner) {
		this.owner = owner;
		Instance = this;
		inventoryUI = InventoryUI.Instance;
	}

	/// <summary>
	/// Ajouter un objet aux entrées d'inventaire
	///		rechercher si une entrée contient déjà un objet identique
	///			si OUI => ajouter 1 à la quantité
	///			si NON => ajouter une entrée
	/// </summary>
	/// <param name="item">l'objet à ajouter</param>
	public void AddItem(Item item) {
		bool found = false;
		for (int i = 0; i < entries.Count; ++i) {			// pour chaque entrée existante
			if (entries[i].item == item) {					// si l'objet contenu est identique
				entries[i].count += 1;						// ajouter 1 à la quantité
				found = true;								// trouvé
				entries[i].ui.UpdateEntry();				// mettre l'objet d'interface associé à jour	
				break;
			}
		}

		if (!found) {										// si on n'a pas trouvé
			InventoryEntry entry = new InventoryEntry(item);// créer une nouvelle entrée
			entry.ui =                                      // créer l'ojet d'interface associé
				inventoryUI.AddItemEntry(entries.Count-1, entry);
			entries.Add(entry);
		}
	
	}

	/// <summary>
	/// This will *try* to use the item. If the item return true when used, this will decrement the stack count and
	/// if the stack count reach 0 this will free the slot. If it return false, it will just ignore that call.
	/// (e.g. a potion will return false if the user is at full health, not consuming the potion in that case)
	/// </summary>
	/// <param name="inventoryEntry"></param>
	/// <returns></returns>
	public bool UseItem(InventoryEntry inventoryEntry) {
		if (inventoryEntry.item.UsedBy(owner)) {						// si l'objet est utilisable
																		// jouer le son associé
			SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData() { Clip = inventoryEntry.item is EquipmentItem ? SFXManager.ItemEquippedSound : SFXManager.ItemUsedSound });
			inventoryEntry.count -= 1;									// retirer 1 à la quantité
			inventoryEntry.ui.UpdateEntry();							// mettre l'ui à jour
			return true;
		}
		return false;
	}


	public void RemoveItem(InventoryEntry entry) {
		entry.count -= 1;												// retirer 1 à la quantité
		if (entry.count <= 0) {                                         // si la quantité est nulle
			entries.Remove(entry);										// retirer l'entrée de l'inventaire
		}
		entry.ui.UpdateEntry();
	}
}
