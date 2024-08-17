using DG.Tweening;
using UnityEngine;

public class Mushroom : MonoBehaviour {
    [SerializeField] private GameObject renderGO;
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

    private void InitializeMushroom()
    {
        data = MushroomDataHelper.GetRandomMushroomData();

        MushroomPartManager parts = MushroomPartManager.Instance;

        MushroomGenerator.GenerateCustomMushroom(new MushroomPart[]
        {
            parts.partsSO.volva[0], parts.partsSO.stem[0], parts.partsSO.ring[0], parts.partsSO.gill[0],
            parts.partsSO.cap[0],
        }, data, t: renderGO.transform);
    }

    private void OnMouseEnter() {
        MushroomDataPanel.Instance.SetPanelDisplay(data);

        
    }

    private void OnMouseExit() {
        MushroomDataPanel.Instance.ResetPanelDisplay();
    }

    private void OnMouseDown()
    {
        MushroomEntityManager.Instance.KillMushroom(this.gameObject);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
