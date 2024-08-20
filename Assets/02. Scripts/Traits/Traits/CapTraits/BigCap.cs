using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCap : MushroomTrait {
    private float[] parms = new[] {1f, 1f, -1f};
    
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value += parms[0];
        data.capWidth.Value += parms[1];
        data.stemHeight.Value += parms[2];
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new BigCap();
    }

    public override string GetTraitName() {
        return "Bigger Cap";
    }

    public override string GetTraitValueDescription() {
        return "Cap size increases, stem length decreases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 0;
    }
}
