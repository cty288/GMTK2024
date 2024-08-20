public class SuppressorRing : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        data.capHeight.Locked = true;
        data.capWidth.Locked = true;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new SuppressorRing();
    }

    public override string GetTraitName() {
        return "Suppressor";
    }

    public override string GetTraitValueDescription() {
        return "Cap size will no longer increase.";
    }

    public override int GetVisualPartGroupIdx() {
        return 16;
    }
}
