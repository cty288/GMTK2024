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
            prefab = MushroomPartManager.Instance.partsSO.volva[
                Random.Range(0, MushroomPartManager.Instance.partsSO.volva.Length)]
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
        return null;
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }
}
