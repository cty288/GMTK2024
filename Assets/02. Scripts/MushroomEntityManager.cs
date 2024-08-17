using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEntityManager : MonoBehaviour
{
    //Might use this script to keep control of all the mushroom spawned via container
    //we shall see if this script is necessary later on. if not i will merge this with input manager or game manager
    private bool deleteMode = false;

    public static MushroomEntityManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(this);
        }

        
    }
    public void KillMushroom(GameObject shroom)
    {
        if(shroom != null && deleteMode)
        {

            GameObject.Destroy(shroom);
        }
    }
    
    public void TurnOnDeleteMode()
    {
        deleteMode = true;
    }
}
