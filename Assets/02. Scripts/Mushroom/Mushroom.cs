using Cysharp.Threading.Tasks;
using DG.Tweening;
using MikroFramework.Architecture;
using MikroFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


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
    private Vector2 originalPosition;

    public MushroomData GetMushroomData() {
        return data;
    }

    public Dictionary<ShroomPart, MushroomPart> Parts { get; private set; }
    //private List<Mushroom> neighbors = new List<Mushroom>();

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
        foreach (var ring in mushroomVisualParts.Ring) {
            float avgWidth = (data.capWidth.RealValue + data.stemWidth.RealValue) / 2;
            ring.SetPartSize(avgWidth, avgWidth);
        }
        foreach (var cap in mushroomVisualParts.Cap) {
            cap.SetPartSize(data.capHeight.RealValue, data.capWidth.RealValue);
        }
    }

    private void ChangeMushroomColor() {
        foreach (var stem in mushroomVisualParts.Stem) {
            stem.SetPartColor(data.stemColor.RealValue, ColorElement.Primary);
            stem.SetPartColor(data.stemColor0.RealValue, ColorElement.Secondary);
            stem.SetPartColor(data.stemColor1.RealValue, ColorElement.Tertiary);
        }
        foreach (var ring in mushroomVisualParts.Ring) {
            ring.SetPartColor(data.stemColor.RealValue, ColorElement.Primary);
            ring.SetPartColor(data.stemColor0.RealValue, ColorElement.Secondary);
            ring.SetPartColor(data.stemColor1.RealValue, ColorElement.Tertiary);
        }
        foreach (var cap in mushroomVisualParts.Cap) {
            cap.SetPartColor(data.capColor.RealValue, ColorElement.Primary);
            cap.SetPartColor(data.capColor0.RealValue, ColorElement.Secondary);
            cap.SetPartColor(data.capColor1.RealValue, ColorElement.Tertiary);
        }
    }

    public void RegenerateMushroomVisuals() {
        MushroomGenerator.RegenerateMushroomVisuals(data, this);
    }

    public void ReinitializeMushroom(Dictionary<ShroomPart, MushroomPart> parts) {
        Parts = parts;
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
            OnStage1();
        } else if (newStage == 2) {
            if (newDay == 3) {
                OnStage2Start();
            }
            OnStage2();
        } else if (newStage == 3) {
            OnStage3();
        } else {
            DestroySelf();
        }

        /*this.Delay(0.1f, () => {
            if (this) {

            }

        });*/
        UpdateVisual().Forget();
    }

    private async UniTask UpdateVisual() {
        await UniTask.NextFrame();
        if (!this) return;
        RegenerateMushroomVisuals();
        ChangeMushroomSizes();
        ChangeMushroomColor();
        RegenerateCollider();
    }

    private void OnStage2() {
        //growth & mutation -> I wrote the code in data
        seedGO.SetActive(false);
        growthGO.SetActive(true);
    }
    private void OnStage2Start() {
        // for each mappedProperty, get the property from a parent that did not give a trait slot related to the mappedProperty
        // for each other property, get a random parent's trait or mix a few parent's traits together
        // I wrote the code in data
    }

    private List<MushroomData> GetStage1Neighbors() {

        var allMushrooms = entityManager.GetAllMushrooms();
        List<MushroomData> neighbors = new List<MushroomData>();

        foreach (Mushroom m1 in allMushrooms) {
            if (m1 != this) {
                float distance = Vector2.Distance(m1.transform.position, transform.position);
                if (distance <= this.GetMushroomData().sporeRange.RealValue && m1.GetMushroomData().GetStage() == 1) {
                    neighbors.Add(m1.GetMushroomData());
                }
            }
        }
        return neighbors;
    }


    private void OnStage1() {
        seedGO.SetActive(true);
        growthGO.SetActive(false);

    }


    private void OnStage3() {
        //parent pass traits to 2 children
        List<MushroomData> stage1Neighbors = GetStage1Neighbors();

        int currChildren = stage1Neighbors.Count;
        for (int i = 0; i < 2 - currChildren; i++) {
            Vector2 spawnPos = UnityEngine.Random.insideUnitCircle * data.sporeRange.RealValue + (Vector2)transform.position;
            Mushroom spawnedMushroom = entityManager.SpawnMushroom(spawnPos, 0, 0);
            stage1Neighbors.Add(spawnedMushroom.GetMushroomData());
        }

        TraitPool.Shuffle(stage1Neighbors);
        for (int i = 0; i < Math.Min(stage1Neighbors.Count, 2); i++) {
            bool res = PassTrait(stage1Neighbors[i]);
            if (res) {
                Debug.Log($"Mushroom {data.GetHashCode()} passed trait to {stage1Neighbors[i].GetHashCode()}");
            }
        }
    }

    /// <summary>
    /// Make the child inherit some traits from the parent (recording the parent)
    /// </summary>
    /// <param name="child"></param>
    private Boolean PassTrait(MushroomData child) {
        MushroomData data = this.GetMushroomData();
        child.AddInfluencedBy(data);

        int categoriesCount = Enum.GetValues(typeof(MushroomTraitCategory)).Length;
        int startIdx = UnityEngine.Random.Range(0, categoriesCount);
        int remainCount = categoriesCount;

        while (remainCount > 0) {
            MushroomTraitCategory category = (MushroomTraitCategory)startIdx;
            if (child.TraitToParentMap[category] == null) {
                child.TraitToParentMap[category] = data;
                return true;
            } else if (Random.value < 0.5f) {
                child.TraitToParentMap[category] = data;
                return true;
            }

            startIdx = (startIdx + 1) % categoriesCount;
            remainCount--;
        }

        return false;
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

        VFXManager.Instance.PlayPickup(transform.position);

        isSelected = true;
        originalPosition = transform.position;
    }

    public void Deselect() {
        if (InputManager.Instance.IsMouseOverUI()) {
            transform.position = originalPosition;
        } else {
            audioSource.clip = plantSFX;
            audioSource.Play();

            VFXManager.Instance.PlayPlace(transform.position);
        }

        oscillationSequence.Play();
        isSelected = false;
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
