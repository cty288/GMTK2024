using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.ResKit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// Create the visual representation of a mushroom
public class MushroomGenerator : MonoBehaviour
{
    
    private void Start()
    {
        MushroomPartManager parts = MushroomPartManager.Instance;

        //GenerateRandomMushroom(1, 2, new Vector3(0, 0, 0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    /// <summary>
    /// Generate a random mushroom game object
    /// </summary>
    /// <param name="minTrait"></param>
    /// <param name="maxTrait"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject GenerateRandomMushroom(int minTrait, int maxTrait, Vector3 position, int initialGrowthDay = 1) {
        
        MushroomPartManager parts = MushroomPartManager.Instance;
        return GenerateCustomMushroom(MushroomDataHelper.GetRandomMushroomData(initialGrowthDay, minTrait, maxTrait), position);
    }

    /// <summary>
    /// Spawn a mushroom given a data
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    /// <param name="partType"></param>
    /// <returns></returns>
    public static GameObject GenerateCustomMushroom(MushroomData data, Vector3 position, ShroomPart partType = ShroomPart.Volvae)
    {
        GameObject prefab = Resources.Load<GameObject>("Mushroom");
        GameObject t = Instantiate(prefab, position, Quaternion.identity);
        Mushroom m = t.GetComponent<Mushroom>();
        
        Dictionary<ShroomPart, MushroomPart> parts = GetPartsFromData(data);
        m.InitializeMushroom(data, parts);


        GenerateCustomMushroomR(parts, data, partType, m.RenderGo.transform, 0);
        return t;
    }

    public static Dictionary<ShroomPart, MushroomPart> GetPartsFromData(MushroomData data) {
        List<IMushroomTrait> traits = data.GetTraits();
        
        Dictionary<ShroomPart, MushroomPart> parts = new Dictionary<ShroomPart, MushroomPart>();
        
        HashSet<ShroomPart> unselectedParts = new HashSet<ShroomPart>();
        foreach (ShroomPart part in Enum.GetValues(typeof(ShroomPart))) {
            unselectedParts.Add(part);
        }
        
        foreach (IMushroomTrait trait in traits) {
            MushroomPart[] traitParts = GetPartsForTrait(trait);
            if (traitParts == null) {
                continue;
            }
            //get all parts that are in unselectedParts
            List<MushroomPart> availableParts = new List<MushroomPart>();
            foreach (MushroomPart part in traitParts) {
                if (unselectedParts.Contains(part.shroomPart)) {
                    availableParts.Add(part);
                }
            }
            
            if (availableParts.Count == 0) {
                continue;
            }
            
            MushroomPart selectedPart = availableParts[Random.Range(0, availableParts.Count)];
            parts[selectedPart.shroomPart] = selectedPart;
            unselectedParts.Remove(selectedPart.shroomPart);
        }
        
        foreach (ShroomPart part in unselectedParts) {
            MushroomPart[] arr = MushroomPartManager.Instance.partsSO.GetParts(part);
            if (arr.Length > 0) {
                parts[part] = arr[Random.Range(0, arr.Length)];
            }
        }
        
        return parts;
    }

    public static MushroomPart GetRandomPartForTrait(IMushroomTrait trait) {
        MushroomPartManager parts = MushroomPartManager.Instance;
        if (trait.GetVisualPartGroupIdx() < 0) {
            return null;
        }

        MushroomPart[] partsArray =
            MushroomPartManager.Instance.partsSO.traitPartGroups[trait.GetVisualPartGroupIdx()].parts;
        
        return partsArray[Random.Range(0, partsArray.Length)];
    }
    
    public static MushroomPart[] GetPartsForTrait(IMushroomTrait trait) {
        MushroomPartManager parts = MushroomPartManager.Instance;
        if (trait.GetVisualPartGroupIdx() < 0) {
            return null;
        }

        MushroomPart[] partsArray =
            MushroomPartManager.Instance.partsSO.traitPartGroups[trait.GetVisualPartGroupIdx()].parts;
        
        return partsArray;
    }
    
    
    public static GameObject GenerateCustomMushroomR(Dictionary<ShroomPart, MushroomPart> parts, MushroomData shroomParams, ShroomPart partType, Transform t, int i)
    {
        if (i >= 20) return null;
        MushroomPart mushroom = Instantiate<MushroomPart>(FindMushroomPartOfType(parts, partType), t.position, t.rotation);
        mushroom.transform.SetParent(t);
        
        if (partType == ShroomPart.Stem || partType == ShroomPart.Ring)
        {
            mushroom.transform.localScale = 
                new Vector3(mushroom.transform.localScale.x * shroomParams.stemWidth.RealValue, 
                    mushroom.transform.localScale.y * shroomParams.stemHeight.RealValue, 1);
        }
        
        if (partType == ShroomPart.Cap || partType == ShroomPart.Gills)
        {
            mushroom.transform.localScale = 
                new Vector3(mushroom.transform.localScale.x * shroomParams.capWidth.RealValue, 
                    mushroom.transform.localScale.y * shroomParams.capHeight.RealValue, 1);
        }
        
        foreach (var spr in mushroom.primaryColorIn)
        {
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor.RealValue;
            }
            else {
                spr.color = shroomParams.capColor.RealValue;
            }
            
        }
        
        foreach (var spr in mushroom.secondaryColorIn)
        {
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor0.RealValue;
            }
            else
            {
                spr.color = shroomParams.capColor0.RealValue;
            }
        }
        
        foreach (var spr in mushroom.tertiaryColorIn)
        {
            if (partType == ShroomPart.Volvae || partType == ShroomPart.Stem)
            {
                spr.color = shroomParams.stemColor1.RealValue;
            }
            else
            {
                spr.color = shroomParams.capColor1.RealValue;
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

    private static MushroomPart FindMushroomPartOfType(Dictionary<ShroomPart, MushroomPart> parts, ShroomPart partType)
    {
        if (parts.ContainsKey(partType)) {
            return parts[partType];
        }

        return null;
    }
}