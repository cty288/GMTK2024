public class PetrifiedStem : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.stemHeight.Locked = true;
    }

    public override void OnStage2Grow(MushroomData data) {
        
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new PetrifiedStem();
    }

    public override string GetTraitName() {
        return "Petrified Stem";
    }

    public override string GetTraitValueDescription() {
        return "Stem length will no longer increase.";
    }

    public override int GetVisualPartGroupIdx() {
        return 11;
    }
}
