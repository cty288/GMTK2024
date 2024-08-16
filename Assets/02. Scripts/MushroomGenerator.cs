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
        }, new MushroomParams(2, 1.2f, 1, 2, Color.red, Color.cyan, Color.gray));
    }

    public GameObject GenerateCustomMushroom(MushroomPart[] parts, MushroomParams shroomParams, ShroomPart partType = ShroomPart.Volvae)
    {
        return GenerateCustomMushroomR(parts, shroomParams, partType, transform, 0);
    }
    public GameObject GenerateCustomMushroomR(MushroomPart[] parts, MushroomParams shroomParams, ShroomPart partType, Transform t, int i)
    {
        if (i >= 20) return null;
        MushroomPart mushroom = Instantiate<MushroomPart>(FindMushroomPartOfType(parts, partType), t.position, t.rotation);
        mushroom.transform.parent = t;
        
        if (partType == ShroomPart.Stem || partType == ShroomPart.Ring)
        {
            mushroom.transform.localScale = 
                new Vector3(mushroom.transform.localScale.x * shroomParams.stemWidth, 
                    mushroom.transform.localScale.y * shroomParams.stemHeight, 1);
        }
        
        if (partType == ShroomPart.Cap || partType == ShroomPart.Gills)
        {
            mushroom.transform.localScale = 
                new Vector3(mushroom.transform.localScale.x * shroomParams.capWidth, 
                    mushroom.transform.localScale.y * shroomParams.capHeight, 1);
        }

        foreach (var spr in mushroom.primaryColorIn)
        {
            spr.color = shroomParams.primary;
        }
        
        foreach (var spr in mushroom.secondaryColorIn)
        {
            spr.color = shroomParams.secondary;
        }
        
        foreach (var spr in mushroom.tertiaryColorIn)
        {
            spr.color = shroomParams.tertiary;
        }

        foreach (var connector in mushroom.connectors)
        {
            GenerateCustomMushroomR(parts, shroomParams, connector.shroomPart, connector.transform, i+1);
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

public struct MushroomParams
{
    public MushroomParams(float sH, float sW, float cH, float cW, Color p, Color s, Color t)
    {
        stemHeight = sH;
        stemWidth = sW;
        capHeight = cH;
        capWidth = cW;
        primary = p;
        secondary = s;
        tertiary = t;
    }
    public float stemHeight;
    public float stemWidth;
    public float capHeight;
    public float capWidth;
    public Color primary;
    public Color secondary;
    public Color tertiary;
}