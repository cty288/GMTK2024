public class DriedOut : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.RealValue.Value--;
        data.capWidth.RealValue.Value += 2;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
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
