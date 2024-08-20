using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBanana : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capWidth.ChildAdditions += 1f;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new AlienBanana();
    }

    public override string GetTraitName() {
        return "Alien Banana";
    }

    public override string GetTraitValueDescription() {
        return "The children of this mushroom will gain extra cap width.";
    }

    public override int GetVisualPartGroupIdx() {
        return 5;
    }
}
