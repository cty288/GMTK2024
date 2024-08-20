public class StandardRing : MushroomTrait {
    public override void OnStartApply(MushroomData data) {

    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.extraSellPrice.Value -= 1;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new StandardRing();
    }

    public override string GetTraitName() {
        return "Standard Ring";
    }

    public override string GetTraitValueDescription() {
        return "Gain extra money when the mushroom is sold.";
    }

    public override int GetVisualPartGroupIdx() {
        return 18;
    }
}
