using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>(); // The list of items in the player's inventory
	public Image[] itemSlots; // Array to hold the 8 item slot UI images
	public Image equippedItemImage; // The Image for the currently equipped item

	private Item equippedItem; // Track the currently equipped item

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	public void AddItem(Item item)
	{
		// Add the item to the inventory list
		items.Add(item);
		// Update the UI slots with the new item
		UpdateInventoryUI();
	}

	// Equip the selected item (you can modify the selection method)
	public void EquipItem(int slotIndex)
	{
		if (slotIndex >= 0 && slotIndex < items.Count)
		{
			equippedItem = items[slotIndex];
			UpdateEquippedItemUI();
		}
	}

	void UpdateInventoryUI()
	{
		// Clear all slots first
		for (int i = 0; i < itemSlots.Length; i++)
		{
			itemSlots[i].sprite = null; // Empty slot
			itemSlots[i].enabled = false; // Disable if no item
		}

		// Loop through the items and set their icons to the item slots
		for (int i = 0; i < items.Count && i < itemSlots.Length; i++)
		{
			itemSlots[i].sprite = items[i].icon; // Set the item's icon
			itemSlots[i].enabled = true; // Enable the slot
		}
	}

	void UpdateEquippedItemUI()
	{
		if (equippedItem != null)
		{
			equippedItemImage.sprite = equippedItem.icon; // Set the equipped item's icon
			equippedItemImage.enabled = true; // Enable the image
		}
		else
		{
			equippedItemImage.enabled = false; // No item equipped
		}
	}

}
