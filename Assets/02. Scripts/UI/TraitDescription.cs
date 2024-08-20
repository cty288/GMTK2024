using TMPro;
using UnityEngine;

public class TraitDescription : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI traitName;
    [SerializeField] private TextMeshProUGUI traitDescription;
    [SerializeField] private GameObject container;

    public void SetTrait(IMushroomTrait trait) {
        traitName.text = trait.GetTraitName();
        traitDescription.text = trait.GetTraitValueDescription();
    }

    public void ShowTrait(bool show) {
        container.SetActive(show);
    }
}
