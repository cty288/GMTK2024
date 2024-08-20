public class StandardCap : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.extraSellPrice.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new StandardCap();
    }

    public override string GetTraitName() {
        return "Standard Cap";
    }

    public override string GetTraitValueDescription() {
        return "Gain extra money when you sell this mushroom.";
    }

    public override int GetVisualPartGroupIdx() {
        return 7;
    }
}
