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
    private bool sellMode = false;
    public PlayerCurrency currency;
    [SerializeField]  TextMeshProUGUI[] babyShroomTrait = new TextMeshProUGUI[3];
    [SerializeField]  TextMeshProUGUI[] mommyShroomTrait = new TextMeshProUGUI[3];
    [SerializeField]  TextMeshProUGUI[] daddyShroomTrait = new TextMeshProUGUI[3];
    [SerializeField] Texture2D cursor;
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
    public void PurchaseMushroom(int a)
    {
        var data = mushroomData[a];
        int mushroomPrice = data.GetBuyPrice();
        if(currency.CanAfford(mushroomPrice))
        {
            currency.Modify(-mushroomPrice);
        }
        Debug.Log("wow you bought a fucking shroom");
    }
    void UpdateShopUI()
    {
        foreach(var e in babyShroomTrait)
        {
            e.gameObject.SetActive(false);
        }
        foreach (var e in mommyShroomTrait)
        {
            e.gameObject.SetActive(false);
        }
        foreach (var e in daddyShroomTrait)
        {
            e.gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {

            var go = MushroomGenerator.GenerateRandomMushroom(1, 2, new Vector3(100, 100, 100));
            mushroomData[i] = go.GetComponent<Mushroom>().GetMushroomData();
            var traits = mushroomData[i].GetTraits();
            for(int j = 0; j< traits.Count; j++)
            {
                switch (i)
                {
                    case 0:
                        babyShroomTrait[j].gameObject.SetActive(true);
                        babyShroomTrait[j].text = traits[j].GetTraitName();
                        break;
                    case 1:
                        mommyShroomTrait[j].gameObject.SetActive(true);
                        mommyShroomTrait[j].text = traits[j].GetTraitName();
                        break;
                    case 2:
                        daddyShroomTrait[j].gameObject.SetActive(true);
                        daddyShroomTrait[j].text = traits[j].GetTraitName();
                        break;
                }
            }
        }
    }
    public void ToggleSellMode()
    {
        if (!sellMode)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            sellMode = true;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            sellMode = false;
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
