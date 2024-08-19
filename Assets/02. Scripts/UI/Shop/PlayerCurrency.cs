using System;
using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour {
    public int Amount;

    public TextMeshProUGUI Text;

    private float displayCurrency;

    public event Action<PlayerCurrency, int> Modified;

    private void Start()
    {
        MushroomEntityManager.Instance.OnEndGame += HideUI;
    }

    private void OnEnable() {
        Text.text = "$" + Amount;
    }

    public bool CanAfford(int amount) {
        return Amount >= amount;
    }

    public void Modify(int delta) {
        Amount += delta;
        this.Modified?.Invoke(this, delta);
    }
    
    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void Update() {
        displayCurrency = Mathf.Lerp(displayCurrency, Amount, Time.deltaTime * 5f);
        Text.text = "$" + Mathf.RoundToInt(displayCurrency);

        if (Input.GetKeyDown(KeyCode.G)) {
            Modify(-10);
        }
    }
}
