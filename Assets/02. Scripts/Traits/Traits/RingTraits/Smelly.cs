public class Smelly : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Smelly();
    }

    public override string GetTraitName() {
        return "Smelly";
    }

    public override string GetTraitValueDescription() {
        return "No effects.";
    }

}
