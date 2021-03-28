using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public class InventoryEvent : UnityEvent<int, InventoryAction> { };

    #region UI

    [Header("Menu")]

    [SerializeField]
    private InventoryMenu m_Menu;

    [Header("Slots params")]

    [SerializeField]
    private InventorySlot[] m_Slots;

    public EntityBase[] TestEntities;

    #endregion UI

    private List<EntityBase> m_Items = new List<EntityBase>();
    private InventoryMenuSlot m_SelectedMenuSlot;

    /// <summary>Событие на смену содержимого инвентаря</summary>
    public static InventoryEvent OnItemAction = new InventoryEvent();


    private void Start()
    {
        Raycaster3D.OnPointerDown += OnPointerDown;
        Raycaster3D.OnPointerUp += OnPointerUp;
        InventoryMenuSlot.OnSelect += OnMenuSlotSelected;
        DragDropTool.OnDrop += OnDrop;
    }

    private void OnDestroy()
    {
        Raycaster3D.OnPointerDown -= OnPointerDown;
        Raycaster3D.OnPointerUp -= OnPointerUp;
        InventoryMenuSlot.OnSelect -= OnMenuSlotSelected;
        DragDropTool.OnDrop -= OnDrop;
    }

    public void OnPointerDown(GameObject source)
    {
        if (source != gameObject)
            return;

        m_SelectedMenuSlot = null;
        m_Menu.Show(m_Items);
    }

    public void OnPointerUp(GameObject source)
    {
        if (m_Menu.Active)
            m_Menu.Hide();

        if (m_SelectedMenuSlot != null)
        {
            // Если был выбран слот в меню, забираем соответствующий итем
            var entity = m_SelectedMenuSlot.EntityData;
            if (entity != null)
                GetItem(entity);
        }
    }

    public void OnMenuSlotSelected(InventoryMenuSlot menuSlot)
    {
        m_SelectedMenuSlot = menuSlot;
    }

    public void OnDrop(EntityBase entity, GameObject target)
    {
        if (entity == null || target != this.gameObject)
            return;

        SetItem(entity);
    }

    public InventorySlot TryGetSlot(EntityType type)
    {
        foreach (var slot in m_Slots)
        {
            if (slot.Type == type)
                return slot;
        }

        return null;
    }

    public void GetItem(EntityBase item)
    {
        // Открепить итем от рюкзака
        item.OnDetachSlot();
        m_Items.Remove(item);
        m_SelectedMenuSlot.Clear();

        OnItemAction?.Invoke(item.Index, InventoryAction.GetItem);
    }

    public void SetItem(EntityBase item)
    {
        var slot = TryGetSlot(item.Type);
        if (slot == null)
            return;

        // Прикрепить итем к рюкзаку
        m_Items.Add(item);
        item.OnAttachSlot(slot);

        OnItemAction?.Invoke(item.Index, InventoryAction.SetItem);
    }
}

public enum InventoryAction
{
    GetItem,
    SetItem
}