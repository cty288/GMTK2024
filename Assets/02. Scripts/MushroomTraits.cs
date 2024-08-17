using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMushroomTrait{
	public HashSet<MushroomPropertyTag> TargetTags { get; } 
}
public abstract class MushroomTrait<T> : IMushroomTrait{
	public HashSet<MushroomPropertyTag> TargetTags { get; } = new HashSet<MushroomPropertyTag>();

	protected List<MushroomProperty<T>> influencedProperties = new List<MushroomProperty<T>>();

	public abstract MushroomPropertyTag[] GetTargetTags();

	public virtual bool SelectTrait(MushroomProperty<T> property) { //by default, it's AND operation
		foreach (var tag in TargetTags){
			if (!property.Tags.Contains(tag)){
				 return false;
			}
		}
		return true;
	}
	
	
	public MushroomTrait(){
		foreach (var tag in GetTargetTags()){
			TargetTags.Add(tag);
		}
	}
	
	public abstract void OnStartApplyToProperty(MushroomProperty<T> property);
	
	public void AddInfluencedProperty(MushroomProperty<T> property){
		influencedProperties.Add(property);
		//OnStartApplyToProperty(property);
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
