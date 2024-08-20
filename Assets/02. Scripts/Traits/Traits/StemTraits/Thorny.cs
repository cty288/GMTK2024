public class Thorny : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.capHeight.Value++;
        data.capWidth.Value++;
        data.stemHeight.Value -= 3;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Thorny();
    }

    public override string GetTraitName() {
        return "Thorny";
    }

    public override string GetTraitValueDescription() {
        return "Cap size increases, stem length decreases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 9;
    }
}
