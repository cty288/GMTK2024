public class SpringTrait : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
        data.stemHeight.Value += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new SpringTrait();
    }

    public override string GetTraitName() {
        return "Spring";
    }

    public override string GetTraitValueDescription() {
        return "Cap length decreases, stem length increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 26;
    }
}
