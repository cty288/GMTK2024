using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.capHeight.Value--;
        data.capWidth.Value += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new OnionCap();
    }

    public override string GetTraitName() {
        return "Onion Cap";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 6;
    }
}
