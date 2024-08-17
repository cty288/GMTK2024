using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryRed : MushroomTrait<Color> {
	public override MushroomPropertyTag[] GetTargetTags() {
		return new MushroomPropertyTag[] {MushroomPropertyTag.Color, MushroomPropertyTag.Stem};
	}

	protected override void OnStartApplyToProperty(MushroomProperty<Color> property) {
		property.RealValue.Value += new Color(0.5f, 0, 0);
	}
	
	
	//public override bool IsGlobalOnly { get; } = false;
	public override bool IsIndependent { get; } = false;

	public override string GetTraitName() {
		return "Very Red";
	}

	public override string GetTraitValueDescription() {
		return null;
	}
}

public class VeryBlue : MushroomTrait<Color> {
	public override MushroomPropertyTag[] GetTargetTags() {
		return new MushroomPropertyTag[] {MushroomPropertyTag.Color, MushroomPropertyTag.Cap};
	}

	//public override bool IsGlobalOnly { get; } = false;

	protected override void OnStartApplyToProperty(MushroomProperty<Color> property) {
		property.RealValue.Value = new Color(0f, 0, 1f);
	}

	public override string GetTraitName() {
		return "Very Blue";
	}
	public override bool IsIndependent { get; } = false;
	public override string GetTraitValueDescription() {
		return null;
	}
}

public class VeryShy : IndependentMushroomTrait {

	public override string GetTraitName() {
		return "Very Shy";
	}

	public override int GetVisualPartGroupIdx() {
		return 0;
	}
}
