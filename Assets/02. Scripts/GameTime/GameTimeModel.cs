using System.Collections;
using System.Collections.Generic;
using MikroFramework.Architecture;
using MikroFramework.BindableProperty;
using UnityEngine;

public class GameTimeModel : AbstractModel {
	public BindableProperty<int> Day { get; } = new BindableProperty<int>(1);
	protected override void OnInit() {
		
	}
}
