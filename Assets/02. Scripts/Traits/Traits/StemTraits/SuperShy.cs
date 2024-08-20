public class SuperShy : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {

    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new SuperShy();
    }

    public override string GetTraitName() {
        return "Super Shy";
    }

    public override string GetTraitValueDescription() {
        return "The mushroom doesn't grow if there are other mushrooms nearby.";
    }
    
}
