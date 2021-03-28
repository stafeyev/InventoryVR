using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DragDropTool : MonoBehaviour 
{
    private Camera m_Camera;
    private RaycastHit m_HitInfo = new RaycastHit();

    private EntityBase m_DragableObject;
    private float m_SelectedDistance;
    private bool m_InvertedDrag = false;

    private InventoryMenuSlot m_SelectedMenuSlot;

    public static Action<EntityBase, GameObject> OnDrop; // Source, Target


    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    private void Start()
    {
        Raycaster3D.OnPointerDown += OnPointerDown;
        Raycaster3D.OnPointerUp += OnPointerUp;
        InventoryMenuSlot.OnSelect += OnMenuSlotSelected;
    }

    private void OnDestroy()
    {
        Raycaster3D.OnPointerDown -= OnPointerDown;
        Raycaster3D.OnPointerUp -= OnPointerUp;
        InventoryMenuSlot.OnSelect -= OnMenuSlotSelected;
    }

    private void FixedUpdate()
    {
        if (m_DragableObject == null)
            return;

        var targetDistance = m_SelectedDistance;

        // Если рейкаст встречает объект ближе изначальной дистанции выбранного объекта берем дистанцию до препятствия
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out m_HitInfo) && m_HitInfo.distance < m_SelectedDistance)
            targetDistance = m_HitInfo.distance;

        Vector3 targetPosition = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetDistance));
        m_DragableObject.OnDrag(targetPosition, Time.fixedDeltaTime);
    }

    public void OnMenuSlotSelected(InventoryMenuSlot menuSlot)
    {
        // Требуются инвертированные реакции на нажатие кнопок мыши, при взятии итема из меню инвентаря
        m_InvertedDrag = menuSlot != null;
        m_SelectedMenuSlot = menuSlot;
    }

    private void OnPointerDown(GameObject source)
    {
        if (!m_InvertedDrag)
        {
            BeginDrag(source);
        }
        else
        {
            m_InvertedDrag = false;
            EndDrag(source);
        }
    }

    private void OnPointerUp(GameObject source)
    {
        if (!m_InvertedDrag)
        {
            EndDrag(source);
        }
        else
        {
            source = m_SelectedMenuSlot.EntityData.gameObject;
            BeginDrag(source);
        }
    }

    private void BeginDrag(GameObject source)
    {
        if (source == null)
            return;

        m_DragableObject = source.GetComponent<EntityBase>();
        if (m_DragableObject == null)
            return;

        m_SelectedDistance = Vector3.Distance(transform.position, source.transform.position);
        m_DragableObject?.OnBeginDrag();        
    }

    private void EndDrag(GameObject source)
    {
        if (m_DragableObject != null)
            m_DragableObject?.OnEndDrag();

        OnDrop?.Invoke(m_DragableObject, source);
        m_DragableObject = null;
        m_InvertedDrag = false;
    }
}
