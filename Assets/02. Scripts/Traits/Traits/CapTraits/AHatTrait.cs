using System.Collections;
using System.Collections.Generic;
using MikroFramework.Architecture;
using UnityEngine;

public struct OnAllMushroomAddProperty {
	public MushroomPropertyTag[] tags;
	public float value;
}

public struct OnAllMushroomChangeParts {
	public ShroomPart part;
	public MushroomPart prefab;
}
public class AHatTrait : MushroomTrait , ICanSendEvent{
	public override void OnStartApply(MushroomData data) {
		
	}

	public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
	public override IMushroomTrait GetCopy() {
		return new AHatTrait();
	}

	public override string GetTraitName() {
		return "A Hat";
	}

	public override string GetTraitValueDescription() {
		return "<b>This trait is gained only through mutation</b>" +
		       "\n When the mushroom is planted, all existing mushrooms gain extra cap length and they will have a same pattern.";
	}

	public override void OnMushroomPlantOnFarm(MushroomData data) {
		base.OnMushroomPlantOnFarm(data);
		this.SendEvent<OnAllMushroomAddProperty>(new OnAllMushroomAddProperty() {
			tags = new MushroomPropertyTag[] {MushroomPropertyTag.Cap, MushroomPropertyTag.Height},
			value = 1
		});
		this.SendEvent<OnAllMushroomChangeParts>(new OnAllMushroomChangeParts() {
			part = ShroomPart.Pattern,
			//index = 3
			prefab = MushroomPartManager.Instance.partsSO.pattern[4]
		});
	}

	public IArchitecture GetArchitecture() {
		return MainGame.Interface;
	}
	
	~AHatTrait() {
		Debug.Log("AHatTrait is destroyed");
	}

	public override int GetVisualPartGroupIdx() {
		return 36;
	}
}
