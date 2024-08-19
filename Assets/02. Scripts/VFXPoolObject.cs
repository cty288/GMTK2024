using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoolObject : MonoBehaviour
{
    private ParticleSystem vfx;
    private bool isUsed;
    public VFXPool masterPool;

    public Vector3 startPos;
    public Vector3 endPos;
    private float t = 0;
    [SerializeField] private bool isTimed;
    [SerializeField] private float timer;
    
    // Start is called before the first frame update
    void Awake()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (vfx.isStopped && isUsed)
        {
            isUsed = false;
            masterPool.ReturnVFX(this);
        }

        if (isTimed)
        {
            if (t >= timer)
            {
                vfx.Stop();
            }
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / timer);
        }
    }

    public void Play()
    {
        vfx.Play();
        isUsed = true;
    }

    public void PlayTimed()
    {
        vfx.Play();
        isUsed = true;
        t = 0;
    }
}

