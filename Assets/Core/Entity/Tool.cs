using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : EntityBase
{
    private void Awake()
    {
        // Вызываем базовую инициализацию, указываем тип энтити
        InitBase(EntityType.Tool);
    }
}
