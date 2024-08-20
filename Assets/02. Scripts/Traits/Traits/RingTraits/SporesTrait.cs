using System.Collections;
using System.Collections.Generic;
using MikroFramework.Architecture;
using UnityEngine;


public class SporesTrait : MushroomTrait , ICanSendEvent{
	public override void OnStartApply(MushroomData data) {
		
	}

	public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
	public override IMushroomTrait GetCopy() {
		return new SporesTrait();
	}

	public override string GetTraitName() {
		return "SPORES";
	}

	public override string GetTraitValueDescription() {
		return "<b>This trait is gained only through mutation</b>" +
		              "\n When the mushroom is planted, all existing mushrooms gain extra cap width and they will have a same pattern.";
		;
	}

	public override void OnMushroomPlantOnFarm(MushroomData data) {
		base.OnMushroomPlantOnFarm(data);
		this.SendEvent<OnAllMushroomAddProperty>(new OnAllMushroomAddProperty() {
			tags = new MushroomPropertyTag[] {MushroomPropertyTag.Cap, MushroomPropertyTag.Width},
			value = 1
		});
		this.SendEvent<OnAllMushroomChangeParts>(new OnAllMushroomChangeParts() {
			part = ShroomPart.Pattern,
			prefab = MushroomPartManager.Instance.partsSO.pattern[1]
		});
	}

	public IArchitecture GetArchitecture() {
		return MainGame.Interface;
	}

	public override int GetVisualPartGroupIdx() {
		return 38;
	}
}
