public class Hyperthyroidism : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.oscillation.Value *= 1.2f;
        data.oscillationSpeed.Value /= 3f;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Hyperthyroidism();
    }

    public override string GetTraitName() {
        return "Hyperthyroidism";
    }

    public override string GetTraitValueDescription() {
        return "SHAKES FASTER!";
    }

    public override int GetVisualPartGroupIdx() {
        return 32;
    }
}
