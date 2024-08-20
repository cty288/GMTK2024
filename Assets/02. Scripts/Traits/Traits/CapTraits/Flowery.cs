using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowery : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.capWidth.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new Flowery();
    }

    public override string GetTraitName() {
        return "Flowery";
    }

    public override string GetTraitValueDescription() {
        return "Cap width increases";
    }

    public override int GetVisualPartGroupIdx() {
        return 4;
    }
}
