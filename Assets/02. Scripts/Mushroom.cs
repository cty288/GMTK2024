using DG.Tweening;
using MikroFramework.Event;
using NHibernate.Util;
using UnityEngine;
using UnityEngine.Rendering;

public class Mushroom : MonoBehaviour {
    [SerializeField] private GameObject renderGO;
    [SerializeField] private SortingGroup sortLayer;
    [SerializeField] private Collider2D _collider;

    private MushroomData data;

    private bool isSelected;

    private void Start() {
        InitializeMushroom();

        DOTween.Sequence()
            .Append(renderGO.transform.DOScale(data.oscillation.RealValue.Value, data.oscillationSpeed.RealValue.Value).SetEase(Ease.InOutSine))
            .Append(renderGO.transform.DOScale(Vector2.one, data.oscillationSpeed.RealValue))
            .SetLoops(-1, LoopType.Restart);
    }

    private void InitializeMushroom()
    {
        data = MushroomDataHelper.GetRandomMushroomData();
        data.AddTraitToAllParts(new VeryRed());
        data.AddTrait(ShroomPart.Cap, new VeryBlue());
        
        

        MushroomPartManager parts = MushroomPartManager.Instance;

        MushroomGenerator.GenerateCustomMushroom(new MushroomPart[]
        {
            parts.partsSO.volva[0], parts.partsSO.stem[0], parts.partsSO.ring[0], parts.partsSO.gill[0],
            parts.partsSO.cap[0], parts.partsSO.pattern[0]
        }, data, t: renderGO.transform);

        sortLayer.sortingOrder = (int) transform.position.y * -1000;
        

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
