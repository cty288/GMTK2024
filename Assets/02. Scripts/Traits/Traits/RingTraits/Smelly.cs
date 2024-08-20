public class Smelly : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Smelly();
    }

    public override string GetTraitName() {
        return "Smelly";
    }

    public override string GetTraitValueDescription() {
        return "Cap length decreases.";
    }
    public override int GetVisualPartGroupIdx() {
        return 48;
    }
}
