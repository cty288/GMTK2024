using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leadership : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new Leadership();
    }

    public override string GetTraitName() {
        return "Leadership";
    }

    public override string GetTraitValueDescription() {
        return "Cap width decreases";
    }

    public override int GetVisualPartGroupIdx() {
        return 35;
    }
}
