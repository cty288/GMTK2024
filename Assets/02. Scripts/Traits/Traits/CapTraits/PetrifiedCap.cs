using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrifiedCap : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        data.capHeight.Locked = true;
        data.capWidth.Locked = true;

        var properties = data.GetProperties<Color>(MushroomPropertyTag.Color, MushroomPropertyTag.Cap);
        foreach (var property in properties) {
            property.Value *= Color.gray;
        }
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new PetrifiedCap();
    }

    public override string GetTraitName() {
        return "Cap size no longer increases.";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 2;
    }
}
