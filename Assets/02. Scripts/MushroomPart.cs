using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomPart : MonoBehaviour
{
    [SerializeField] public ShroomPart shroomPart;
    [SerializeField] public MushroomPartConnector[] connectors;
    [SerializeField] private Vector3 scaleMod;
    
    void OnDrawGizmos()
    {
        // Draws the Light bulb icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "Mark.png", false);
    }
}

public enum ShroomPart
{
    Cap,
    Gills,
    Ring,
    Stem,
    Volvae
}