using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shop : MonoBehaviour
{
    
    public RectTransform shopPanel;
    public GameObject shopUI;
    private bool inAnimation = false;
  
    void Start()
    {
        
        shopUI.SetActive(false);
        shopPanel.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A) && inAnimation == false)
        {
            EnableShopUI();
        }
    }

    void EnableShopUI()
    {
        
        shopUI.SetActive(true);

        inAnimation = true;
        shopPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
       
    }

    public void DisableShopUI()
    {
        shopPanel.DOScale(Vector3.zero, 0.5f).OnComplete(() => { shopUI.gameObject.SetActive(false); inAnimation = false; });
    }
}
