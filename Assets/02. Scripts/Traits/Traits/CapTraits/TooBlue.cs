using UnityEngine;

public class TooBlue : MushroomTrait {
    public override void OnStartApply(MushroomData data) {
        var colors = data.GetProperties<Color>(MushroomPropertyTag.Color);
        foreach (var capColor in colors) {
            capColor.Value = new Color(0, 0, 1f);
        }
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new TooBlue();
    }

    public override string GetTraitName() {
        return "Too Blue";
    }

	public override string GetTraitValueDescription() {
        return "The mushroom will become <color=blue>BLUE</color> (duh).";
	}
    public override int GetVisualPartGroupIdx() {
        return 45;
    }
}