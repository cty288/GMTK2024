
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MikroFramework.Architecture;
using MikroFramework.Event;
using NHibernate.Util;
using UnityEngine;
using UnityEngine.Rendering;

public class Mushroom : AbstractMikroController<MainGame> {
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
    public MushroomData GetMushroomData()
    {
        return data;
    }
    
    public Dictionary<ShroomPart, MushroomPart> Parts { get;  private set; }

    private void Awake() {
        this.GetModel<GameTimeModel>().Day.RegisterOnValueChanged(OnDayChange).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnDayChange(int arg1, int arg2) {
        if(data != null) {
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

        //  Debug.Log("Has very shy trait: " + data.HasTrait<VeryShy>());
    }

    private void OnGrowthDayChange(int oldDay, int newDay) {
        int newStage = data.GetStage(newDay);
        if (newStage == 1) {
            seedGO.SetActive(true);
            growthGO.SetActive(false);
        } else if(data.GetStage(oldDay) != data.GetStage(newDay)) {
            Debug.Log("Mushroom has grown to stage " + newStage);
            if (newStage == 2) {
                seedGO.SetActive(false);
                growthGO.SetActive(true);
            }else if (newStage == 3) {
                
            }
            else { //die
                DestroySelf();
            }
            RegenerateCollider();
        }
    }

    public void RegenerateCollider()
    {
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

    public async void DestroySelf() {
        audioSource.clip = destroySFX;
        audioSource.Play();

        await UniTask.WaitUntil(() => !audioSource.isPlaying);
        oscillationSequence.Kill();
        Destroy(gameObject);
    }

    private void OnDestroy() {
        data.UnregisterOnTraitAdd<VeryShy>(OnVeryShyAdded);
    }
}
