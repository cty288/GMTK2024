using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value++;
        data.capWidth.Value++;
        data.stemHeight.Value--;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new BigCap();
    }

    public override string GetTraitName() {
        return "Bigger Cap";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 0;
    }
}
