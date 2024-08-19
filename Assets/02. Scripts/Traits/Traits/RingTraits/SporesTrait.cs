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
		return null;
	}

	public override void OnMushroomPlantOnFarm(MushroomData data) {
		base.OnMushroomPlantOnFarm(data);
		this.SendEvent<OnAllMushroomAddProperty>(new OnAllMushroomAddProperty() {
			tags = new MushroomPropertyTag[] {MushroomPropertyTag.Cap, MushroomPropertyTag.Width},
			value = 1
		});
		this.SendEvent<OnAllMushroomChangeParts>(new OnAllMushroomChangeParts() {
			part = ShroomPart.Pattern,
			prefab = MushroomPartManager.Instance.partsSO.pattern[2]
		});
	}

	public IArchitecture GetArchitecture() {
		return MainGame.Interface;
	}

}
