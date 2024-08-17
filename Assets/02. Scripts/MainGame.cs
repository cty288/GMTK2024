using System.Collections;
using System.Collections.Generic;
using MikroFramework.Architecture;
using UnityEngine;

public class MainGame : Architecture<MainGame>
{
    protected override void Init() {
        this.RegisterModel<GameTimeModel>(new GameTimeModel());
    }
}
