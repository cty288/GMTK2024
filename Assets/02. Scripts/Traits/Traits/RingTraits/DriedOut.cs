public class DriedOut : MushroomTrait {
    public override void OnStartApply(MushroomData data) {

    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
        data.capWidth.Value += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new DriedOut();
    }

    public override string GetTraitName() {
        return "Dried Out";
    }

    public override string GetTraitValueDescription() {
        return "Cap length decreases, cap width increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 17;
    }
}
