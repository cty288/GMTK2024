using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LargestSize : MonoBehaviour
{
    public float Amount;

    public TextMeshProUGUI Text;

    private float displayCurrency;

    public event Action<LargestSize, float> Modified;

    private void OnEnable() {
        Text.text = "Biggest Mushroom: " + Amount + " size";
    }

    public void Modify(float value) {
        Amount = value;
        Text.text = "Biggest Mushroom: " + Math.Round(Amount, 2) + " size";
        this.Modified?.Invoke(this, value);
    }
}
