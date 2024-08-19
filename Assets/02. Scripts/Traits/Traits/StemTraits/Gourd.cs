﻿public class Gourd : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.stemHeight.ChildAdditions += 2f;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Gourd();
    }

    public override string GetTraitName() {
        return "Gourd";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 14;
    }
}