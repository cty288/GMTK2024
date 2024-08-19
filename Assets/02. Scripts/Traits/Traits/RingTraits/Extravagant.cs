public class Extravagant : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.ChildAdditions += 2;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
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
