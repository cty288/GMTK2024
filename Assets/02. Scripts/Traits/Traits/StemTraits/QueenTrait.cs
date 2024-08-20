using MikroFramework.Architecture;
using UnityEngine;

public class QueenTrait : MushroomTrait, ICanSendEvent {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override void OnMushroomPlantOnFarm(MushroomData data) {
        base.OnMushroomPlantOnFarm(data);
        this.SendEvent<OnAllMushroomAddProperty>(new OnAllMushroomAddProperty() {
            tags = new MushroomPropertyTag[] {MushroomPropertyTag.Stem, MushroomPropertyTag.Height},
            value = 1
        });
        this.SendEvent<OnAllMushroomChangeParts>(new OnAllMushroomChangeParts() {
            part = ShroomPart.Volvae,
            prefab = MushroomPartManager.Instance.partsSO.volva[6]
        });
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new QueenTrait();
    }

    public override string GetTraitName() {
        return "Queen";
    }

    public override string GetTraitValueDescription() {
        return "<b>This trait is gained only through mutation</b>" +
               "\n When the mushroom is planted, all existing mushrooms gain extra stem length and they will have a same volva.";
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }
    
    
    public override int GetVisualPartGroupIdx() {
        return 43;
    }
}
