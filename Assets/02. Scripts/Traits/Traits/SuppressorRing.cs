using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppressorRing : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        data.capHeight.RealValue.Locked = true;
        data.capWidth.RealValue.Locked = true;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new SuppressorRing();
    }

    public override string GetTraitName() {
        return "Suppressor";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 8;
    }
}
