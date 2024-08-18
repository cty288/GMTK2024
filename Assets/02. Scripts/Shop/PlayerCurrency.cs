using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerCurrency : MonoBehaviour
{
	public int Amount;

	public TextMeshProUGUI Text;

	private float displayCurrency;

	public event Action<PlayerCurrency, int> Modified;

	private void OnEnable()
	{
		Text.text = "$" + Amount;
	}

	public bool CanAfford(int amount)
	{
		return Amount >= amount;
	}

	public void Modify(int delta)
	{
		Amount += delta;
		this.Modified?.Invoke(this, delta);
	}

	private void Update()
	{
		displayCurrency = Mathf.Lerp(displayCurrency, Amount, Time.deltaTime * 5f);
		Text.text = "$" + Mathf.RoundToInt(displayCurrency);

        if (Input.GetKeyDown(KeyCode.G))
        {
			Modify(-10);
        }
	}
}
