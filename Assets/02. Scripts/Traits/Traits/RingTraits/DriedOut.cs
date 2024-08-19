public class DriedOut : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.Value--;
        data.capWidth.Value += 2;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new DriedOut();
    }

    public override string GetTraitName() {
        return "Dried Out";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 17;
    }
}
