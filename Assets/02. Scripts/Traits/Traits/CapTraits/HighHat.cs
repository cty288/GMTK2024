using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighHat : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value += 2;
        data.capWidth.Value--;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new HighHat();
    }

    public override string GetTraitName() {
        return "High Hat";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 22;
    }
}
