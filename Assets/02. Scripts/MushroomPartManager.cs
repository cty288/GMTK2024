using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MushroomPartManager : MonoBehaviour
{
    private static MushroomPartManager _instance;
    public static MushroomPartManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<MushroomPartManager>();
                _instance.partsSO = AssetDatabase.LoadAssetAtPath<MushroomPartsSO>("Assets/DefaultMushroomParts.asset");
                return _instance;
            }
        }
    }

    [SerializeField] public MushroomPartsSO partsSO;
}
