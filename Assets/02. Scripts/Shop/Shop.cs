using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
//i hate people
public class Shop : MonoBehaviour
{
    
    public RectTransform shopPanel;
    public GameObject shopUI;
    public TextMeshProUGUI[] description = new TextMeshProUGUI[3];
    private bool inAnimation = false;

    private MushroomData[] mushroomData = new MushroomData[3];


    public TextMeshProUGUI[] babyShroomTrait = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] mommyShroomTrait = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] daddyShroomTrait = new TextMeshProUGUI[3];
    void Start()
    {
        
        shopUI.SetActive(false);
        shopPanel.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            //EnableShopUI();
            UpdateShopUI();
        }
    }
    void UpdateShopUI()
    {
        for(int i = 0; i < 3; i++)
        {

            var go = MushroomGenerator.GenerateRandomMushroom(1, 2, new Vector3(100, 100, 100));
            mushroomData[i] = go.GetComponent<Mushroom>().GetMushroomData();
            var traits = mushroomData[i].GetTraits();
            for(int j = 0; j< traits.Count; j++)
            {
                switch (i)
                {
                    case 0:
                        
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
            }
        }
    }
    void EnableShopUI()
    {
        InitShopItems();
        shopUI.SetActive(true);

        inAnimation = true;
        shopPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
       
    }

    public void DisableShopUI()
    {
        shopPanel.DOScale(Vector3.zero, 0.5f).OnComplete(() => { shopUI.gameObject.SetActive(false); inAnimation = false; });
    }

    private void InitShopItems()
    {
        for(int i = 0; i < 3; i++)
        {
            var go = MushroomGenerator.GenerateRandomMushroom(1, 2, new Vector3(100, 100, 100));
            mushroomData[i] = go.GetComponent<Mushroom>().GetMushroomData();
            description[i].text = mushroomData[i].ToString();
        }
    }
}
