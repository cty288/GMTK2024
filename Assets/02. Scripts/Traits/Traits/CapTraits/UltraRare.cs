using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraRare : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        var colors = data.GetProperties<Color>(MushroomPropertyTag.Color);
        foreach (var capColor in colors) {
            capColor.Value *= Color.yellow;
            ;
        }
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        if(newStage != 2) return;
        data.extraSellPrice.Value += 3;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new UltraRare();
    }

    public override string GetTraitName() {
        return "Ultra Rare";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return -1;
    }
}
