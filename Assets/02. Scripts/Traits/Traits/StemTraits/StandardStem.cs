﻿public class StandardStem : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.sellPriceLocker.Value = 2;
    }

    public override void OnEnd(MushroomData mushroomData) {
        base.OnEnd(mushroomData);
        mushroomData.sellPriceLocker.Value = -1;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new StandardStem();
    }

    public override string GetTraitName() {
        return "Standard Stem";
    }

    public override string GetTraitValueDescription() {
        return "Sell price stays at 2.";
    }

    public override int GetVisualPartGroupIdx() {
        return 15;
    }
}
