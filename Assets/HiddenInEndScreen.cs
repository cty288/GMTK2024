using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenInEndScreen : MonoBehaviour
{
    private void Start()
    {
        MushroomEntityManager.Instance.OnEndGame += HideUI;
    }
    
    private void HideUI()
    {
        gameObject.SetActive(false);
    }
}
