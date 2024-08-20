public class Contraband : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
       
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.sellPriceMultiplier.Value *= 2f;
        data.capHeight.Value--;
        data.capWidth.Value--;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Contraband();
    }

    public override string GetTraitName() {
        return "Contraband";
    }

    public override string GetTraitValueDescription() {
        return "Cap size decreases. Sell price doubled.";
    }

    public override int GetVisualPartGroupIdx() {
        return 46;
    }
}
