using System.Collections.Generic;
using UnityEngine;

public class BookMenu : MonoBehaviour {
    [SerializeField] private GameObject bookMenu;
    [SerializeField] private TraitDescription[] traitDescriptions;
    [SerializeField] private GameObject previousImage;
    [SerializeField] private GameObject nextImage;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openBookSound;
    [SerializeField] private AudioClip pageFlipSound;

    private List<IMushroomTrait> traitsList;
    private int currentPageFirstIndex = 0;
    private bool isOpen = false;

    private void Start() {
        traitsList = TraitPool.GetAllTraits();
    }

    public void OpenBook() {
        audioSource.clip = openBookSound;
        audioSource.Play();

        isOpen = !isOpen;
        bookMenu.SetActive(isOpen);
        if (isOpen) {
            currentPageFirstIndex = 0;
            UpdateUI();
        }
    }

    public void PreviousPage() {
        audioSource.clip = pageFlipSound;
        audioSource.Play();

        currentPageFirstIndex -= traitDescriptions.Length;
        currentPageFirstIndex = Mathf.Clamp(currentPageFirstIndex, 0, traitsList.Count - 1);

        UpdateUI();
    }

    public void NextPage() {
        audioSource.clip = pageFlipSound;
        audioSource.Play();

        currentPageFirstIndex += traitDescriptions.Length;
        currentPageFirstIndex = Mathf.Clamp(currentPageFirstIndex, 0, traitsList.Count - 1);

        UpdateUI();
    }

    private void UpdateUI() {
        if (currentPageFirstIndex == 0) {
            previousImage.SetActive(false);
        } else {
            previousImage.SetActive(true);
        }

        if (currentPageFirstIndex + traitDescriptions.Length >= traitsList.Count) {
            nextImage.SetActive(false);
        } else {
            nextImage.SetActive(true);
        }

        foreach (var traitDescription in traitDescriptions) {
            traitDescription.ShowTrait(false);
        }

        for (int i = currentPageFirstIndex; i < traitsList.Count; i++) {
            if (i - currentPageFirstIndex > traitDescriptions.Length - 1) {
                break;
            }

            traitDescriptions[i - currentPageFirstIndex].SetTrait(traitsList[i]);
            traitDescriptions[i - currentPageFirstIndex].ShowTrait(true);
        }
    }
}
