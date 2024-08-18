using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrifiedCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        data.capHeight.RealValue.Locked = true;
        data.capWidth.RealValue.Locked = true;

        var properties = data.GetProperties<Color>(MushroomPropertyTag.Color, MushroomPropertyTag.Cap);
        foreach (var property in properties) {
            property.RealValue.Value *= Color.gray;
        }
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new PetrifiedCap();
    }

    public override string GetTraitName() {
        return "Petrified Cap";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 2;
    }
}
