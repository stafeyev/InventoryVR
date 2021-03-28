using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class EntityBase : MonoBehaviour, IDragDrop
{
    #region UI

    [SerializeField]
    private int m_Index;

    [SerializeField]
    private string m_ItemName;

    [SerializeField]
    private float m_Weight = 1f;

    #endregion UI

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    // Snap
    private bool m_SmoothFollow;
    private Vector3 m_FollowPosition;
    private Quaternion m_FollowRotation;
    private float m_SnapToPointerSpeed = 1f;
    private float m_StartSnapSpeed = 1f;
    private float m_MaxSnapSpeed = 70f;
    private float m_SnapSpeedIncrease = 120f;

    public EntityType Type { get; private set; }
    public int Index => m_Index;
    public string ItemName => m_ItemName;


    protected void InitBase(EntityType type)
    {
        Type = type;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.mass = m_Weight;
        m_Collider = GetComponent<Collider>();
    }

    public void OnBeginDrag()
    {
        EnablePhysics(false);
        transform.SetParent(null);
        StartSmoothFollow(m_Rigidbody.position, m_Rigidbody.rotation);
    }

    public void OnDrag(Vector3 position, float deltaTime)
    {
        if (m_SmoothFollow)
        {
            // Задаем позицию для плавного снапа энтити
            m_FollowPosition = position;
            m_FollowRotation = m_Rigidbody.rotation;
        }
        else
        {
            // Без плавного снапа сразу задаем позицию поинтера
            m_Rigidbody.position = position;
            m_Rigidbody.rotation = m_Rigidbody.rotation;
        }
    }

    public void OnEndDrag()
    {
        EnablePhysics(true);
        StopSmoothFollow();
    }

    public void OnAttachSlot(InventorySlot slot)
    {
        EnablePhysics(false);

        var slotTransform = slot.transform;
        transform.SetParent(slotTransform.parent);
        StartSmoothFollow(slotTransform.position, slotTransform.rotation);
    }

    public void OnDetachSlot()
    {
        //EnablePhysics(true);
    }

    private void EnablePhysics(bool val)
    {
        m_Rigidbody.isKinematic = !val;
        m_Rigidbody.useGravity = val;
        m_Collider.enabled = val;
    }

    private void StartSmoothFollow(Vector3 position, Quaternion rotation)
    {
        m_FollowPosition = position;
        m_FollowRotation = rotation;
        m_SmoothFollow = true;
        m_SnapToPointerSpeed = m_StartSnapSpeed;
        StartCoroutine(SmoothFollow());
    }

    private void StopSmoothFollow()
    {
        m_SmoothFollow = false;
        StopCoroutine(SmoothFollow());
    }

    private IEnumerator SmoothFollow()
    {
        while (m_SmoothFollow)
        {
            float deltaTime = Time.fixedDeltaTime;
            m_Rigidbody.position = Vector3.Lerp(m_Rigidbody.position, m_FollowPosition, deltaTime * m_SnapToPointerSpeed);
            m_Rigidbody.rotation = Quaternion.Lerp(m_Rigidbody.rotation, m_FollowRotation, deltaTime * m_SnapToPointerSpeed);
            m_SnapToPointerSpeed += deltaTime * m_SnapSpeedIncrease;
            if (m_SnapToPointerSpeed > m_MaxSnapSpeed)
            {
                m_SnapToPointerSpeed = m_MaxSnapSpeed;
                m_SmoothFollow = false;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}

public enum EntityType
{
    Default = 0,
    Weapon = 1,
    Grenade = 2,
    Tool = 3
}
