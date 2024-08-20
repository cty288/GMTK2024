public class StickTrait : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capWidth.Value--;
        data.stemHeight.Value += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new StickTrait();
    }

    public override string GetTraitName() {
        return "Stick";
    }

    public override string GetTraitValueDescription() {
        return "Cap width decreases, stem length increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 27;
    }
}
