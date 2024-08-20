using System.Linq;
using MikroFramework.Architecture;
using UnityEngine;

public struct OnAllMushroomReplaceTrait {
    public IMushroomTrait trait;
    public MushroomData source;
}

public struct OnAllMushroomUpdateProperties {
    public float capHeight;
    public float capWidth;
    public float stemHeight;
    public float stemWidth;
    //public MushroomProperty<float> growthSpeed;
    public Vector2 oscillation;
    public float oscillationSpeed;
    public Color capColor;
    public Color stemColor;
    public Color capColor0;
    public Color stemColor0;
    public Color capColor1;
    public Color stemColor1;
    public float sporeRange;
    public int extraSellPrice;
}
public class BetterBreed : MushroomTrait, ICanSendEvent {
    private bool inEffective = true;
    public override void OnStartApply(MushroomData data) {
        data.capWidth.Value++;
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override void OnMushroomPlantOnFarm(MushroomData data) {
        base.OnMushroomPlantOnFarm(data);
        if (inEffective) {
            var traits = data.GetTraits();
            foreach (IMushroomTrait trait in traits) {
                this.SendEvent<OnAllMushroomReplaceTrait>(new OnAllMushroomReplaceTrait() {
                    trait = trait.GetCopy(),
                    source = data
                });
            }
            
            ShroomPart[] parts = data.Parts.Keys.ToArray();
            foreach (ShroomPart part in parts) {
                this.SendEvent<OnAllMushroomChangeParts>(new OnAllMushroomChangeParts() {
                    part = part,
                    prefab = data.Parts[part]
                });
            }
            
            this.SendEvent<OnAllMushroomUpdateProperties>(new OnAllMushroomUpdateProperties() {
                capHeight = data.capHeight.Value,
                capWidth = data.capWidth.Value,
                stemHeight = data.stemHeight.Value,
                stemWidth = data.stemWidth.Value,
                oscillation = data.oscillation.Value,
                oscillationSpeed = data.oscillationSpeed.Value,
                capColor = data.capColor.Value,
                stemColor = data.stemColor.Value,
                capColor0 = data.capColor0.Value,
                stemColor0 = data.stemColor0.Value,
                capColor1 = data.capColor1.Value,
                stemColor1 = data.stemColor1.Value,
                sporeRange = data.sporeRange.Value,
                extraSellPrice = data.extraSellPrice.Value
            });
        }
    }


    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        BetterBreed copy = new BetterBreed();
        copy.inEffective = false;
        return copy;
    }

    public override string GetTraitName() {
        return "Better Breed";
    }

    public override string GetTraitValueDescription() {
        return "<b>This trait is gained only through the shop</b>\n" +
               "After this mushroom is planted, all other existing mushrooms will become this mushroom.";
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }

    public override int GetVisualPartGroupIdx() {
        return 39;
    }
}
