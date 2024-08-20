using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyScale : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.capHeight.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new FishyScale();
    }

    public override string GetTraitName() {
        return "Fishy Scale";
    }

    public override string GetTraitValueDescription() {
        return "Cap length increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 3;
    }
}
