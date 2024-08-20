public class Antisocial : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Antisocial();
    }

    public override string GetTraitName() {
        return "Antisocial";
    }

    public override string GetTraitValueDescription() {
        return "No effects.";
    }

}