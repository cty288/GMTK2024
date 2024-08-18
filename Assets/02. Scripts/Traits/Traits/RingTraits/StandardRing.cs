public class StandardRing : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.extraSellPrice.RealValue.Value -= 1;
    }

    public override void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
        base.OnNewDay(data, oldDay, newDay, oldStage, newStage);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new StandardRing();
    }

    public override string GetTraitName() {
        return "Standard Ring";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 18;
    }
}
