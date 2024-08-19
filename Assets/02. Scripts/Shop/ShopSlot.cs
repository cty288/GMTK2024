using TMPro;
using UnityEngine;
using UnityEngine.UI;
//i hate people
public class ShopSlot : MonoBehaviour {
    [SerializeField] private GameObject soldOutSign;
    [SerializeField] private GameObject shopItem;

    [SerializeField] private TextMeshProUGUI[] traitText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image shopItemImage;

    private MushroomData mushroomForSale;
    private int price;

    private void Start() {
        ResetItem();
    }

    public void SetShopItem(MushroomData mushroom) {
        mushroomForSale = mushroom;

        shopItem.SetActive(true);
        soldOutSign.SetActive(false);

        var count = 0;
        foreach (var trait in mushroomForSale.GetTraits()) {
            traitText[count++].text = trait.GetTraitName();
        }

        shopItemImage.color = mushroom.capColor.RealValue;

        price = mushroomForSale.GetBuyPrice();
        priceText.text = "$" + price;
    }

    public MushroomData GetMushroomForSale() {
        return mushroomForSale;
    }

    public int GetPrice() {
        return price;
    }

    public bool IsEmpty() {
        return mushroomForSale == null;
    }

    public void ResetItem() {
        mushroomForSale = null;
        price = 0;

        shopItem.SetActive(false);
        soldOutSign.SetActive(true);
    }
}