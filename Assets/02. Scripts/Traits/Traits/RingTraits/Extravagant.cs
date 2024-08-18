public class Extravagant : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.ChildAdditions += 2;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Extravagant();
    }

    public override string GetTraitName() {
        return "Extravagant";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 19;
    }
}
