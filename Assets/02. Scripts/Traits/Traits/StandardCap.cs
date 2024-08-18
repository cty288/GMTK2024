using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        if(newStage != 2) return;
        data.extraSellPrice.RealValue.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new StandardCap();
    }

    public override string GetTraitName() {
        return "Standard Cap";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 7;
    }
}
