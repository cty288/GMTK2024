using Cysharp.Threading.Tasks;
using UnityEngine;

public class Hallucinogenic : MushroomTrait {
    private bool isEnd = false;
    public override void OnStartApply(MushroomData data) {
       
        ChangeColor(data).Forget();
    }

    public override void OnStage2Grow(MushroomData data) {
        base.OnStage2Grow(data);
    }

    public override void OnEnd(MushroomData mushroomData) {
        base.OnEnd(mushroomData);
        isEnd = true;
    }

    private async UniTask ChangeColor(MushroomData data) {
        while (!isEnd) {
            await UniTask.WaitForSeconds(0.2f);
            if (data == null) {
                break;
            }

            var colors = data.GetProperties<Color>(MushroomPropertyTag.Color);
            foreach (var c in colors) {
                c.Value = new Color(Random.value, Random.value, Random.value);
            }
            data.SendUpdateColorEvent();
        }
    }

    public override MushroomTraitCategory Category { get; } = MushroomTraitCategory.Ring;
    public override IMushroomTrait GetCopy() {
        return new Hallucinogenic();
    }

    public override string GetTraitName() {
        return "Hallucinogenic";
    }

    public override string GetTraitValueDescription() {
        return null;
    }

    public override int GetVisualPartGroupIdx() {
        return 37;
    }
}
