using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float time = 0.6f;

    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            Destroy(gameObject);
        }
        time -= Time.deltaTime;
    }
}
