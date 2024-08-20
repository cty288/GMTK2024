public class FlatEarther : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new FlatEarther();
    }

    public override string GetTraitName() {
        return "Flat-earther";
    }

    public override string GetTraitValueDescription() {
        return "No effects.";
    }

    public override int GetVisualPartGroupIdx() {
        return 47;
    }
}
