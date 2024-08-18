using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;

    [SerializeField] private SpriteRenderer outliner;
    
    
    // Start is called before the first frame update
    void Start()
    {
        outliner.sprite = renderer.sprite;
    }

    public void ChangeColor(Color c)
    {
        outliner.color = c;
    }
}
