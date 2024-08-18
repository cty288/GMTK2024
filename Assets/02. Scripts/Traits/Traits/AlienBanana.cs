using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBanana : MushroomTrait
{
    public override void OnStartApply(MushroomData data) {
        data.capWidth.ChildAdditions += 2f;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new AlienBanana();
    }

    public override string GetTraitName() {
        return "Alien Banana";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 5;
    }
}
