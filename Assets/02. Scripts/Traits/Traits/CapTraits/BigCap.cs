using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        if(newStage != 2) return;
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        data.capHeight.RealValue.Value++;
        data.capWidth.RealValue.Value++;
        data.stemHeight.RealValue.Value--;
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
