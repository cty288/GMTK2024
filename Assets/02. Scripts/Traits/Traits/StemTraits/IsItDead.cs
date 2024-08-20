public class IsItDead : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        data.stemHeight.Value--;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new IsItDead();
    }

    public override string GetTraitName() {
        return "Is It Dead?";
    }

    public override string GetTraitValueDescription() {
        return "Stem length decreases. The mushroom doesn't want to move.";
    }
    
    
    public override int GetVisualPartGroupIdx() {
        return 42;
    }
    
}
