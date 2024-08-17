using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TraitPartGroup {
    [SerializeField] public String Name;
    [SerializeField] public MushroomPart[] parts;
}
[CreateAssetMenu]
public class MushroomPartsSO : ScriptableObject
{
    [SerializeField] public MushroomPart[] volva;
    [SerializeField] public MushroomPart[] stem;
    [SerializeField] public MushroomPart[] ring;
    [SerializeField] public MushroomPart[] gill;
    [SerializeField] public MushroomPart[] cap;
    [SerializeField] public MushroomPart[] pattern;


    [SerializeField] public TraitPartGroup[] traitPartGroups;
    

    public MushroomPart[] GetParts(ShroomPart part) {
        switch (part) {
            case ShroomPart.Volvae:
                return volva;
            case ShroomPart.Stem:
                return stem;
            case ShroomPart.Ring:
                return ring;
            case ShroomPart.Gills:
                return gill;
            case ShroomPart.Cap:
                return cap;
            case ShroomPart.Pattern:
                return pattern;
            default:
                return null;
        }
    }
}