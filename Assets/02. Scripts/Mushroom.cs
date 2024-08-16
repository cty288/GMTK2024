using DG.Tweening;
using UnityEngine;

public class Mushroom : MonoBehaviour {
    [SerializeField] private GameObject renderGO;
    [SerializeField] private SpriteRenderer capSprite;
    [SerializeField] private SpriteRenderer stemSprite;
    [SerializeField] private Collider2D _collider;

    private MushroomData data;

    private bool isSelected;

    private void Start() {
        InitializeMushroom();

        DOTween.Sequence()
            .Append(renderGO.transform.DOScale(data.oscillation, data.oscillationSpeed).SetEase(Ease.InOutSine))
            .Append(renderGO.transform.DOScale(Vector2.one, data.oscillationSpeed))
            .SetLoops(-1, LoopType.Restart);
    }

    private void InitializeMushroom() {
        data = MushroomDataHelper.GetRandomMushroomData();

        capSprite.size = new Vector2(data.capWidth, data.capHeight);
        stemSprite.size = new Vector2(data.stemWidth, data.stemHeight);
        capSprite.color = data.capColor;
        stemSprite.color = data.stemColor;
    }

    private void OnMouseEnter() {
        MushroomDataPanel.Instance.SetPanelDisplay(data);
    }

    private void OnMouseExit() {
        MushroomDataPanel.Instance.ResetPanelDisplay();
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
