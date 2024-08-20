using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
        data.capWidth.Value--;
        data.stemWidth.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new GravityPull();
    }

    public override string GetTraitName() {
        return "Gravity Pull";
    }

    public override string GetTraitValueDescription() {
        return "Cap size decreases, stem width increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 23;
    }
}
