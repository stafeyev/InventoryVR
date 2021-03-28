using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryMenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region UI

    [SerializeField]
    private EntityType m_Type;

    [SerializeField]
    private Text m_ItemName;

    [SerializeField]
    private Image m_Preview;

    #endregion UI

    public static Action<InventoryMenuSlot> OnSelect;
    public EntityBase EntityData { get; set; }
    public EntityType Type => m_Type;


    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSelect?.Invoke(null);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }

    public void SetData(EntityBase data)
    {
        gameObject.SetActive(true);
        EntityData = data;
        m_ItemName.text = data.ItemName;
    }
}
