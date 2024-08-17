using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

// Create the visual representation of a mushroom
public class MushroomGenerator : MonoBehaviour
{
    private void Start()
    {
        MushroomPartManager parts = MushroomPartManager.Instance;
        
        GenerateCustomMushroom( new MushroomPart[]
        {
            parts.partsSO.volva[0], parts.partsSO.stem[0], parts.partsSO.ring[0], parts.partsSO.gill[0], parts.partsSO.cap[0], parts.partsSO.pattern[0]
        }, MushroomDataHelper.GetRandomMushroomData());
    }

    public static GameObject GenerateCustomMushroom(MushroomPart[] parts, MushroomData data, ShroomPart partType = ShroomPart.Volvae, Transform t = null)
    {
        if (t == null)
        {
            var go = new GameObject();
            t = go.transform;
        }
        
        return GenerateCustomMushroomR(parts, data, partType, t, 0);
    }
    public static GameObject GenerateCustomMushroomR(MushroomPart[] parts, MushroomData shroomParams, ShroomPart partType, Transform t, int i)
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
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor;
            }
            else
            {
                spr.color = shroomParams.capColor;
            }
            
        }
        
        foreach (var spr in mushroom.secondaryColorIn)
        {
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor0;
            }
            else
            {
                spr.color = shroomParams.capColor0;
            }
        }
        
        foreach (var spr in mushroom.tertiaryColorIn)
        {
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor1;
            }
            else
            {
                spr.color = shroomParams.capColor1;
            }
        }

        if (partType == ShroomPart.Cap)
        {
            float r = Random.Range(0.0f, 1.0f);
            if (r >= 0.5f)
            {
                GenerateCustomMushroomR(parts, shroomParams, ShroomPart.Pattern, t, i+1);
            }
        }

        if (partType == ShroomPart.Pattern)
        {
            return mushroom.gameObject;
        }
        
        
        foreach (var connector in mushroom.connectors)
        {
            GenerateCustomMushroomR(parts, shroomParams, connector.shroomPart, connector.transform, i+1);
        }

        return mushroom.gameObject;
    }

    private static MushroomPart FindMushroomPartOfType(MushroomPart[] parts, ShroomPart partType)
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