public class PetrifiedStem : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        if (newStage != 2) return;
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
        data.stemHeight.RealValue.Locked = true;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new PetrifiedStem();
    }

    public override string GetTraitName() {
        return "Petrified Stem";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 3;
    }
}
