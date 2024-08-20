using System.Collections;
using System.Collections.Generic;
using MikroFramework.AudioKit;
using UnityEngine;

public class AMouth : MushroomTrait {
    private AudioSource audio;
    public override void OnStartApply(MushroomData data) {
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
        data.capHeight.Value--;
    }

    public override void OnMushroomPlantOnFarm(MushroomData data) {
        base.OnMushroomPlantOnFarm(data);

        audio = AudioSystem.Singleton.Play2DSound("pvz", 0.2f, true);
    }

    public override void OnEnd(MushroomData mushroomData) {
        base.OnEnd(mushroomData);
        if (audio != null) {
            AudioSystem.Singleton.StopSound(audio);
        }
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Cap;
    public override IMushroomTrait GetCopy() {
        return new AMouth();
    }

    public override string GetTraitName() {
        return "A Mouth?";
    }

    public override string GetTraitValueDescription() {
        return "Nobody can stop the mushroom from singing (except zombies and shovels).";
    }

    public override int GetVisualPartGroupIdx() {
        return 24;
    }
}
