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

	public void OnStage2Grow(MushroomData data);
	
	public MushroomTraitCategory Category { get; }
	
	public IMushroomTrait GetCopy();
	
	public string GetTraitName();
	
	public string GetTraitValueDescription();
	
	public void OnMushroomPlantOnFarm(MushroomData data);
	void OnEnd(MushroomData mushroomData);
}


public abstract class MushroomTrait : IMushroomTrait {
	
	public abstract void OnStartApply(MushroomData data);

	public virtual int GetVisualPartGroupIdx() {
		return -1;
	}

	public virtual void OnStage2Grow(MushroomData data) {
		
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

	public virtual void OnEnd(MushroomData mushroomData) {
		
	}

	public override string ToString() {
		return GetTraitName();
	}
}


