using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Create the visual representation of a mushroom
public class MushroomGenerator : MonoBehaviour
{
    [SerializeField] private MushroomPart[] volva;
    [SerializeField] private MushroomPart[] stem;
    [SerializeField] private MushroomPart[] ring;
    [SerializeField] private MushroomPart[] gill;
    [SerializeField] private MushroomPart[] cap;

    private void Start()
    {
        GenerateCustomMushroom( new MushroomPart[]
        {
            volva[0], stem[0], ring[0], gill[0], cap[0]
        });
    }

    public GameObject GenerateCustomMushroom(MushroomPart[] parts, ShroomPart partType = ShroomPart.Volvae)
    {
        MushroomPart mushroom = Instantiate<MushroomPart>(FindMushroomPartOfType(parts, partType), transform);

        foreach (var connector in mushroom.connectors)
        {
            GenerateCustomMushroomR(parts, connector.shroomPart, connector.transform, 0);
        }

        return mushroom.gameObject;
    }
    public GameObject GenerateCustomMushroomR(MushroomPart[] parts, ShroomPart partType, Transform t, int i)
    {
        if (i >= 20) return null;
        MushroomPart mushroom = Instantiate<MushroomPart>(FindMushroomPartOfType(parts, partType), t);

        foreach (var connector in mushroom.connectors)
            {
                GenerateCustomMushroomR(parts, connector.shroomPart, connector.transform, i+1);
            }

        return mushroom.gameObject;
    }


    private MushroomPart FindMushroomPartOfType(MushroomPart[] parts, ShroomPart partType)
    {
        foreach (var part in parts)
        {
            if (part.shroomPart == partType)
            {
                return part;
            }
        }

        return null;
    }
}
