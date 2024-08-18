using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomPart : MonoBehaviour
{
    [SerializeField] public ShroomPart shroomPart;
    [SerializeField] public MushroomPartConnector[] connectors;
    [SerializeField] private Vector3 scaleMod;

    [SerializeField] public SpriteRenderer[] primaryColorIn;
    [SerializeField] public SpriteRenderer[] secondaryColorIn;
    [SerializeField] public SpriteRenderer[] tertiaryColorIn;

    // Change the size of current part.
    public void SetPartSize(float height, float width)
    {
        var p = transform.parent;

        transform.parent = null;
        
        foreach (var connector in connectors)
        {
            if (connector.child != null)
            {
                connector.child.parent = null;
            }
        }

        transform.localScale = new Vector3(width * scaleMod.x, height * scaleMod.y, 1);
        
        foreach (var connector in connectors)
        {
            if (connector.child != null)
            {
                connector.child.parent = connector.transform;
                connector.child.position = connector.transform.position;
            }
        }

        transform.parent = p;
    }
    
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
    Stem,
    Volvae,
    Pattern,
    //Global
}