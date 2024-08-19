using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _instance;

    public static VFXManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private ParticleSystem dustPlace;
    [SerializeField] private ParticleSystem dustPickUp;

    public VFXPool growVFXPool;
    public VFXPool pollinateVFXPool;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayPlace(Vector3 position)
    {
        dustPlace.gameObject.SetActive(true);
        dustPlace.transform.position = position;
        dustPlace.Play();
    }
    
    public void PlayPickup(Vector3 position)
    {
        dustPickUp.gameObject.SetActive(true);
        dustPickUp.transform.position = position;
        dustPickUp.Play();
    }

    public void PlayGrowth(Vector3 position, Vector3 scale)
    {
        growVFXPool.UseVFX(position, scale);
    }
}