using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class Raycaster3D : MonoBehaviour 
{
    private Camera m_Camera;
    private RaycastHit m_HitInfo = new RaycastHit();

    public static Action<GameObject> OnPointerDown;
    public static Action<GameObject> OnPointerUp;


    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    private void Update() 
	{
        if (Input.GetMouseButtonDown(0))
        {
            PointerDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            PointerUp();
        }		
	}

    private void PointerDown()
    {
        var source = GetObjectOnPointer();
        OnPointerDown?.Invoke(source);
    }

    private void PointerUp()
    {
        var source = GetObjectOnPointer();
        OnPointerUp?.Invoke(source);
    }

    private GameObject GetObjectOnPointer()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out m_HitInfo))
            return null;

        return m_HitInfo.collider.gameObject;
    }
}

