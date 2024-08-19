using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowery : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        if(newStage != 2) return;
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
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
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 4;
    }
}
