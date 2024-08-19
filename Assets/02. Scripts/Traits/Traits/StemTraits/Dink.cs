public class Dink : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {

    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Dink();
    }

    public override string GetTraitName() {
        return "Dink";
    }

    public override string GetTraitValueDescription() {
        return null;
    }
    
}
