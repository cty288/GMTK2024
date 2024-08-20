public class Poisonous : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new Poisonous();
    }

    public override string GetTraitName() {
        return "Poisonous";
    }

    public override string GetTraitValueDescription() {
        return "No effects.";
    }

}