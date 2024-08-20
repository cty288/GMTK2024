using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MouseOverChangeImage : MonoBehaviour {
    [SerializeField] private Sprite normalImage;
    [SerializeField] private Sprite mouseOverImage;

    private Image image;

    private void Start() {
        image = GetComponent<Image>();
    }

    public void SetHoverImage() {
        image.sprite = mouseOverImage;
    }

    public void SetNormalImage() {
        image.sprite = normalImage;
    }
}
