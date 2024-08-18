using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyScale : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        if(newStage != 2) return;
        data.capHeight.RealValue.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new FishyScale();
    }

    public override string GetTraitName() {
        return "Fishy Scale";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 3;
    }
}
