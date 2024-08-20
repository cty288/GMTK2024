using UnityEngine;

public class UltraRare : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        var colors = data.GetProperties<Color>(MushroomPropertyTag.Color);
        foreach (var capColor in colors) {
            capColor.Value *= Color.yellow;
        }
    }

    public override void OnStage2Grow(MushroomData data) {
        data.extraSellPrice.Value += 3;
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new UltraRare();
    }

    public override string GetTraitName() {
        return "Ultra Rare";
    }

    public override string GetTraitValueDescription() {
        return "Gain extra money when the mushroom is sold.";
    }

    public override int GetVisualPartGroupIdx() {
        return 33;
    }


}
