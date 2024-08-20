public class StrongNeck : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value++;
        data.capWidth.Value++;
        data.stemWidth.Value -= 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new StrongNeck();
    }

    public override string GetTraitName() {
        return "Strong Neck";
    }

    public override string GetTraitValueDescription() {
        return "Cap size increases, stem width decreases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 28;
    }
}
