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

    public MushroomVisuals mushroomVisualParts; // A class containing references to all the parts of a mushroom for modification.

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

    /// <summary>
    /// Updates the mushroom part sizes based on the information in data
    /// </summary>
    private void ChangeMushroomSizes() {
        foreach (var stem in mushroomVisualParts.Stem) {
            stem.SetPartSize(data.stemHeight.RealValue, data.stemWidth.RealValue);
        }
        foreach (var cap in mushroomVisualParts.Cap) {
            cap.SetPartSize(data.capHeight.RealValue, data.capWidth.RealValue);
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
            Stage1();
        } else if (data.GetStage(oldDay) != data.GetStage(newDay)) {
            if (newStage == 2) {
                Stage2();
            } else if (newStage == 3) {
                Stage3();
            } else { //die
                DestroySelf();
            }
            ChangeMushroomSizes();
            RegenerateCollider();
        }
    }

    private void Stage1() {
        seedGO.SetActive(true);
        growthGO.SetActive(false);

        List<Mushroom> stage3Neighbors = FindNeighborsBasedOnStage(3);
        foreach (Mushroom parent in stage3Neighbors) {
            var traits = parent.data.GetTraits();
            TraitPool.Shuffle(traits);
            foreach (var trait in traits) {
                //TODO: 
                // try to add the trait to this mushroom
                // if a trait slot is available, add the trait, otherwise try 50% replace or try for next slot
                // keep track of which parent's mushroomData was used for which traits

            }
        }
    }

    private void Stage2() {
        seedGO.SetActive(false);
        growthGO.SetActive(true);

        // for each mappedProperty, get the property from a parent that did not give a trait slot related to the mappedProperty
        // for each other property, get a random parent's trait or mix a few parent's traits together
        // on second day, increment/decrement a few traits
    }

    private void Stage3() {
        int currChildren = FindNeighborsBasedOnStage(1).Count;
        for (int i = 0; i < 2 - currChildren; i++) {
            Vector2 spawnPos = UnityEngine.Random.insideUnitCircle * data.sporeRange.RealValue + (Vector2)transform.position;
            entityManager.SpawnMushroom(spawnPos);
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

    public void ChangeOutlineColor(Color color) {
        foreach (var outliner in mushroomVisualParts.Outliners) {
            outliner.ChangeColor(color);
        }
    }

    private void OnVeryShyAdded(VeryShy e) {
        Debug.Log("This is a very shy mushroom");
    }

    private void OnMouseEnter() {
        ChangeOutlineColor(Color.white);
        MushroomDataPanel.Instance.TurnOnPanel();
        MushroomDataPanel.Instance.SetPanelDisplay(data);
    }

    private void OnMouseExit() {
        ChangeOutlineColor(Color.black);
        MushroomDataPanel.Instance.TurnOffPanel();
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

    private List<Mushroom> FindNeighborsBasedOnStage(int stage) {
        List<Mushroom> results = new List<Mushroom>();
        foreach (Mushroom neighbor in neighbors) {
            if (neighbor.data.GetStage() == stage) {
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
