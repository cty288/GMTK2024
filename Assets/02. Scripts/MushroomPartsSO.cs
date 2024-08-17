using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class MushroomPartsSO : ScriptableObject
{
    [SerializeField] public MushroomPart[] volva;
    [SerializeField] public MushroomPart[] stem;
    [SerializeField] public MushroomPart[] ring;
    [SerializeField] public MushroomPart[] gill;
    [SerializeField] public MushroomPart[] cap;
}