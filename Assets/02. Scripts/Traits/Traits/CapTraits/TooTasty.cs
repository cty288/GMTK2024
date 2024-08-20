using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooTasty : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }
    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);

    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new TooTasty();
    }

    public override string GetTraitName() {
        return "Too Tasty";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 34;
    }
}
