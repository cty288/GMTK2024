using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMushroomTrait{
	//public HashSet<MushroomPropertyTag> TargetTags { get; }
	//public bool SelectTrait(IMushroomProperty property);

	//public void AddInfluencedProperty(IMushroomProperty property);
	
	public void OnStartApply(MushroomData data);
	
	//public bool IsGlobalOnly { get; }
	
	/// <summary>
	/// Is the trait independent of properties?
	/// </summary>

	public int GetVisualPartGroupIdx();
}


public abstract class MushroomTrait : IMushroomTrait {
	
	public abstract void OnStartApply(MushroomData data);

	public virtual int GetVisualPartGroupIdx() {
		return -1;
	}
	
	public MushroomTrait(){

	}
	

	public abstract string GetTraitName();
	
	public abstract string GetTraitValueDescription();

	public override string ToString() {
		if(GetTraitValueDescription() == null){
			return GetTraitName();
		}
		return GetTraitName() + " : " + GetTraitValueDescription();
	}
}


