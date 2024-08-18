using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryRed : MushroomTrait{

	//public override bool IsGlobalOnly { get; } = false;
	public override void OnStartApply(MushroomData data) {
		var capColors = data.GetProperties<Color>(MushroomPropertyTag.Color, MushroomPropertyTag.Cap);
		foreach (var capColor in capColors) {
			capColor.RealValue.Value += new Color(0.5f, 0, 0);
		}
	}

	public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.CapLength;
	public override IMushroomTrait GetCopy() {
		return new VeryRed();
	}


	public override string GetTraitName() {
		return "Very Red";
	}

	public override string GetTraitValueDescription() {
		return null;
	}
}

public class VeryBlue : MushroomTrait {
	public override void OnStartApply(MushroomData data) {
		var colors = data.GetProperties<Color>(MushroomPropertyTag.Color, MushroomPropertyTag.Stem);
		foreach (var capColor in colors) {
			capColor.RealValue.Value = new Color(0, 0, 1f);
		}
	}

	public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.CapLength;
	public override IMushroomTrait GetCopy() {
		return new VeryBlue();
	}

	public override string GetTraitName() {
		return "Very Blue";
	}

	public override string GetTraitValueDescription() {
		return null;
	}
}

public class VeryShy : MushroomTrait {
	public override IMushroomTrait GetCopy() {
		return new VeryShy();
	}

	public override string GetTraitName() {
		return "Very Shy";
	}

	public override string GetTraitValueDescription() {
		return null;
	}

	public override void OnStartApply(MushroomData data) {
		
	}


	public override int GetVisualPartGroupIdx() {
		return 0;
	}

	public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.CapWidth;
}
