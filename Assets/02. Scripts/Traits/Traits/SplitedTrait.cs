using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitedTrait : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        data.capHeight.RealValue.Value /= 2f;
        data.capWidth.RealValue.Value /= 2f;
        data.stemHeight.RealValue.Value /= 2f;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new SplitedTrait();
    }

    public override string GetTraitName() {
        return "Small Cap";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 1;
    }
}
