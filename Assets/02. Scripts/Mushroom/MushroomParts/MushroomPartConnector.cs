using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class MushroomPartConnector
{
    [SerializeField] public ShroomPart shroomPart;
    [SerializeField] public Transform transform;
    [SerializeField] public Transform child;
}
