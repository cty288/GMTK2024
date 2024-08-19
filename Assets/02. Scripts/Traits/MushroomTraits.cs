using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomTraitCategory {
	Cap,
	Stem,
	Ring
}
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

	public void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage);
	
	public MushroomTraitCategory Category { get; }
	
	public IMushroomTrait GetCopy();
	
	public string GetTraitName();
	
	public string GetTraitValueDescription();
	
	public void OnMushroomPlantOnFarm(MushroomData data);
}


public abstract class MushroomTrait : IMushroomTrait {
	
	public abstract void OnStartApply(MushroomData data);

	public virtual int GetVisualPartGroupIdx() {
		return -1;
	}

	public virtual void OnNewDay(MushroomData data, int oldDay, int newDay, int oldStage, int newStage) {
		
	}

	public abstract MushroomTraitCategory Category { get; }
	public abstract IMushroomTrait GetCopy();



	public MushroomTrait(){

	}
	

	public abstract string GetTraitName();
	
	public abstract string GetTraitValueDescription();
	public virtual void OnMushroomPlantOnFarm(MushroomData data) {
		Debug.Log($"{ToString()} Plant on farm");
	}

	public override string ToString() {
		if(GetTraitValueDescription() == null){
			return GetTraitName();
		}
		return GetTraitName() + " : " + GetTraitValueDescription();
	}
}


