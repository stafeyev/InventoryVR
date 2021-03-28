using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour 
{
    [SerializeField]
    private EntityType m_Type;
    
    public EntityType Type => m_Type;


    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
