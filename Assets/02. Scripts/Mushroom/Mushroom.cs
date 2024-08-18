using Cysharp.Threading.Tasks;
using DG.Tweening;
using MikroFramework.Architecture;
using MikroFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Mushroom : AbstractMikroController<MainGame> {
    private MushroomEntityManager entityManager;

    [SerializeField] private GameObject growthGO;
    [SerializeField] private GameObject seedGO;

    public GameObject RenderGo => growthGO;
    [SerializeField] private SortingGroup sortLayer;
    [SerializeField] private Collider2D _collider;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private AudioClip plantSFX;
    [SerializeField] private AudioClip destroySFX;

    private MushroomData data;

    private Sequence oscillationSequence;
    [HideInInspector] public bool isSelected = false;
    public MushroomData GetMushroomData() {
        return data;
    }

    public Dictionary<ShroomPart, MushroomPart> Parts { get; private set; }
    private List<Mushroom> neighbors = new List<Mushroom>();

    public Action<Mushroom> OnMushroomDestroyed;

    private void Awake() {
        entityManager = MushroomEntityManager.Instance;

        this.GetModel<GameTimeModel>().Day.RegisterOnValueChanged(OnDayChange).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnDayChange(int arg1, int arg2) {
        if (data != null) {
            data.GrowthDay.Value++;
        }
    }

    public void InitializeMushroom(MushroomData data, Dictionary<ShroomPart, MushroomPart> parts) {
        this.data = data;
        Parts = parts;

        sortLayer.sortingOrder = (int)transform.position.y * -1000;
        oscillationSequence = DOTween.Sequence()
            .Append(RenderGo.transform.DOScale(data.oscillation.RealValue.Value, data.oscillationSpeed.RealValue.Value).SetEase(Ease.InOutSine))
            .Append(RenderGo.transform.DOScale(Vector2.one, data.oscillationSpeed.RealValue.Value))
            .SetLoops(-1, LoopType.Restart);

        data.RegisterOnTraitAdd<VeryShy>(OnVeryShyAdded);

        this.data.GrowthDay.RegisterWithInitValue(OnGrowthDayChange).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnGrowthDayChange(int oldDay, int newDay) {
        int newStage = data.GetStage(newDay);
        if (newStage == 1) {
            seedGO.SetActive(true);
            growthGO.SetActive(false);
        } else if (data.GetStage(oldDay) != data.GetStage(newDay)) {
            if (newStage == 2) {
                seedGO.SetActive(false);
                growthGO.SetActive(true);
            } else if (newStage == 3) {

            } else { //die
                DestroySelf();
            }
            RegenerateCollider();
        }
    }

    public void SetNeighbors(List<Mushroom> neighbors) {
        this.neighbors = neighbors;
    }

    public void AddNeighbor(Mushroom neighbor) {
        if (!neighbors.Contains(neighbor))
            this.neighbors.Add(neighbor);
    }

    public void RemoveNeighbor(Mushroom neighbor) {
        this.neighbors.Remove(neighbor);
    }

    public void ClearNeighbors() {
        neighbors.Clear();
    }

    public void RegenerateCollider() {
        ((CompositeCollider2D)_collider).GenerateGeometry();
    }

    private void OnVeryShyAdded(VeryShy e) {
        Debug.Log("This is a very shy mushroom");
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

    private List<Mushroom> FindStage3Neighbors() {
        List<Mushroom> results = new List<Mushroom>();
        foreach (Mushroom neighbor in neighbors) {
            if (neighbor.data.GetStage() == 3) {
                results.Add(neighbor);
            }
        }
        return results;
    }

    public async void DestroySelf() {
        audioSource.clip = destroySFX;
        audioSource.Play();

        await UniTask.WaitUntil(() => !audioSource.isPlaying);

        OnMushroomDestroyed?.Invoke(this);

        oscillationSequence.Kill();
        Destroy(gameObject);
    }

    private void OnDestroy() {
        data.UnregisterOnTraitAdd<VeryShy>(OnVeryShyAdded);
    }
}
