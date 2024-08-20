public class BigFoot : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
        data.capWidth.Value--;
        data.stemHeight.Value += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new BigFoot();
    }

    public override string GetTraitName() {
        return "Big Foot";
    }

    public override string GetTraitValueDescription() {
        return "Cap size decreases, stem length increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 8;
    }
}
