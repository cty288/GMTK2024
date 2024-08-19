public class Tube : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.Value++;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Tube();
    }

    public override string GetTraitName() {
        return "Tube";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 20;
    }
}
