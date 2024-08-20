public class Extravagant : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
       
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.ChildAdditions += 2;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Extravagant();
    }

    public override string GetTraitName() {
        return "Extravagant";
    }

    public override string GetTraitValueDescription() {
        return "The children of the mushroom will gain extra cap length.";
    }

    public override int GetVisualPartGroupIdx() {
        return 19;
    }
}
