public class Squeezed : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.stemHeight.Value--;
        data.stemWidth.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Squeezed();
    }

    public override string GetTraitName() {
        return "Squeezed";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 25;
    }
}
