using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoolObject : MonoBehaviour
{
    private ParticleSystem vfx;
    public VFXPool masterPool;
    
    // Start is called before the first frame update
    void Awake()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (vfx.isStopped)
        {
            masterPool.ReturnVFX(this);
        }
    }

    public void Play()
    {
        vfx.Play();
    }
}
