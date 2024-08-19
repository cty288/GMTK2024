public class Skirt : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capWidth.Value++;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Skirt();
    }

    public override string GetTraitName() {
        return "Skirt";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 21;
    }
}
