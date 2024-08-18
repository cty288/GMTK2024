public class Thorny : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        if (newStage != 2) return;
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        data.capHeight.RealValue.Value++;
        data.capWidth.RealValue.Value++;
        data.stemHeight.RealValue.Value -= 3;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Thorny();
    }

    public override string GetTraitName() {
        return "Thorny";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 7;
    }
}
