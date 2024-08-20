public class LazyTrait : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {

    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Stem;

    public override IMushroomTrait GetCopy() {
        return new LazyTrait();
    }

    public override string GetTraitName() {
        return "Lazy";
    }

    public override string GetTraitValueDescription() {
        return "<b>(Shop Only)</b>\n" +
               "The mushroom is too lazy to move.";
    }


    public override int GetVisualPartGroupIdx() {
        return 44;
    }
}
