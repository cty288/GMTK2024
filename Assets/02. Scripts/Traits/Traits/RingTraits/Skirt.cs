public class Skirt : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capWidth.Value++;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Skirt();
    }

    public override string GetTraitName() {
        return "Skirt";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 21;
    }
}
