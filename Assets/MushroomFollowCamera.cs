using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class MushroomFollowCamera : MonoBehaviour
{
    public static MushroomFollowCamera instance;
    private Camera c;
    //public RawImage image;
    private void Awake()
    {
        instance = this;
        c = gameObject.GetComponent<Camera>();
    }
    private void Start()
    {
        
    }
    public void UpdateCameraPosition(float x, float y)
    {

        this.c.gameObject.transform.position = new Vector3(x, y, -3);
    }
  
}
