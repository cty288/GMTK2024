
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MikroFramework.Event;
using NHibernate.Util;
using UnityEngine;
using UnityEngine.Rendering;

public class Mushroom : MonoBehaviour {
    [SerializeField] private GameObject renderGO;

    public GameObject RenderGo => renderGO;
    [SerializeField] private SortingGroup sortLayer;
    [SerializeField] private Collider2D _collider;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private AudioClip plantSFX;
    [SerializeField] private AudioClip destroySFX;
    [SerializeField] private bool debug = false;
    private MushroomData data;

    private Sequence oscillationSequence;
    [HideInInspector] public bool isSelected = false;

    private void Start() {
        if (debug) {
            
        }

       // InitializeMushroom(MushroomDataHelper.GetRandomMushroomData(1,2));

        oscillationSequence = DOTween.Sequence()
            .Append(renderGO.transform.DOScale(data.oscillation.RealValue.Value, data.oscillationSpeed.RealValue.Value).SetEase(Ease.InOutSine))
            .Append(renderGO.transform.DOScale(Vector2.one, data.oscillationSpeed.RealValue.Value))
            .SetLoops(-1, LoopType.Restart);
    }

    public void InitializeMushroom(MushroomData data) {
        this.data = data;
        
      
        /*
        MushroomPartManager parts = MushroomPartManager.Instance;
        MushroomGenerator.GenerateCustomMushroom(new MushroomPart[]
        {
            parts.partsSO.volva[0], parts.partsSO.stem[0], parts.partsSO.ring[0], parts.partsSO.gill[0],
            parts.partsSO.cap[0], parts.partsSO.pattern[0]
        }, data,  renderGO.transform);*/

        sortLayer.sortingOrder = (int)transform.position.y * -1000;
    }



    private void OnMouseEnter() {
        MushroomDataPanel.Instance.SetPanelDisplay(data);
    }

    private void OnMouseExit() {
        MushroomDataPanel.Instance.ResetPanelDisplay();
    }

    private void Update() {
        if (isSelected) {
            transform.position = InputManager.Instance.GetMouseWorldPosition();
            sortLayer.sortingOrder = (int)transform.position.y * -1000;
        }
    }

    public void Select() {
        audioSource.clip = pickupSFX;
        audioSource.Play();

        oscillationSequence.Pause();

        isSelected = true;
    }

    public void Deselect() {
        audioSource.clip = plantSFX;
        audioSource.Play();

        oscillationSequence.Play();

        isSelected = false;
    }

    public async void DestroySelf() {
        audioSource.clip = destroySFX;
        audioSource.Play();

        await UniTask.WaitUntil(() => !audioSource.isPlaying);
        oscillationSequence.Kill();
        Destroy(gameObject);
    }
}
