using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour 
{
	[SerializeField]
	private InventoryMenuSlot[] m_Slots;
	public bool Active { get; private set; }


	public void Show(List<EntityBase> items)
    {
		Active = true;
		gameObject.SetActive(true);
		UpdateSlots(items);
	}

	public void Hide()
    {
		Active = false;
		gameObject.SetActive(false);
    }

	private void UpdateSlots(List<EntityBase> items)
    {
		for (int i = 0; i < m_Slots.Length; i++)
		{
			var slot = m_Slots[i];
			slot.Clear();

			foreach (var item in items)
            {
				if (item.Type == slot.Type)
                {
					slot.SetData(item);
					break;
                }
            }
		}
	}
}
