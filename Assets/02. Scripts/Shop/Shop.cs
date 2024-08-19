using MikroFramework.Architecture;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, ICanGetModel {
    public PlayerCurrency currency;
    private InputManager inputManager;

    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private Image mushroomGhost;
    private bool isDragging = false;
    private int selectedSlot = -1;

    private void Start() {
        inputManager = InputManager.Instance;
        mushroomGhost.gameObject.SetActive(false);

        this.GetModel<GameTimeModel>().Day.RegisterOnValueChanged(UpdateShopItems);
    }

    private void UpdateShopItems(int arg1, int arg2) {
        for (int i = 0; i < 3; i++)
            if (shopSlots[i].IsEmpty() && Random.value < 0.7f) {
                var mushroomData = MushroomGenerator.GenerateRandomMushroomData(1, 2);
                shopSlots[i].SetShopItem(mushroomData);
            }
    }

    public void ClickItem(int slot) {
        if (!shopSlots[slot].IsEmpty() && currency.Amount >= shopSlots[slot].GetPrice()) {
            selectedSlot = slot;
            isDragging = true;
            mushroomGhost.color = shopSlots[slot].GetMushroomForSale().capColor.RealValue;
            mushroomGhost.gameObject.SetActive(true);
            inputManager.OnMouseUp += BuyItem;
        }
    }

    private void BuyItem() {
        isDragging = false;
        mushroomGhost.gameObject.SetActive(false);
        inputManager.OnMouseUp -= BuyItem;

        if (inputManager.IsMouseOverUI()) {
            selectedSlot = -1;
        } else {
            currency.Modify(-shopSlots[selectedSlot].GetPrice());

            MushroomEntityManager.Instance.SpawnMushroom(shopSlots[selectedSlot].GetMushroomForSale(), inputManager.GetMouseWorldPosition());

            shopSlots[selectedSlot].ResetItem();
        }
    }

    private void Update() {
        if (isDragging) {
            Vector3 mousePos = inputManager.GetMouseScreenPosition();
            mousePos.z = 0;
            mushroomGhost.transform.position = mousePos;
        }
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }
}

