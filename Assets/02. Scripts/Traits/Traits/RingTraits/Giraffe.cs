public class Giraffe : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
        data.capWidth.Value--;
        data.stemHeight.Value++;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Giraffe();
    }

    public override string GetTraitName() {
        return "Giraffe";
    }

    public override string GetTraitValueDescription() {
        return "Cap size decreases, stem length increases.";
    }

    public override int GetVisualPartGroupIdx() {
        return 31;
    }
}
