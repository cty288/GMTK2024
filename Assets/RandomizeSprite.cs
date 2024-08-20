using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    [SerializeField]
    private SpriteRenderer renderer;
    private void Awake()
    {
        renderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
