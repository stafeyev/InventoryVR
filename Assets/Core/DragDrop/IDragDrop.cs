using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragDrop
{
    void OnBeginDrag();
    void OnDrag(Vector3 position, float deltaTime);
    void OnEndDrag();
}
