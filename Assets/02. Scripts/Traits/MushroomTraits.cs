using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMushroomTrait{
	public HashSet<MushroomPropertyTag> TargetTags { get; }
	public bool SelectTrait(IMushroomProperty property);

	public void AddInfluencedProperty(IMushroomProperty property);
	
	public void OnStartApplyToProperty(IMushroomProperty property);
	
	public bool IsGlobalOnly { get; }
}
public abstract class MushroomTrait<T> : IMushroomTrait{
	public HashSet<MushroomPropertyTag> TargetTags { get; } = new HashSet<MushroomPropertyTag>();


	protected List<MushroomProperty<T>> influencedProperties = new List<MushroomProperty<T>>();

	public abstract MushroomPropertyTag[] GetTargetTags();

	protected virtual bool SelectTrait(MushroomProperty<T> property) { //by default, it's AND operation
		foreach (var tag in TargetTags){
			if (!property.Tags.Contains(tag)){
				 return false;
			}
		}
		return true;
	}
	
	public bool SelectTrait(IMushroomProperty property) {
		if (property is MushroomProperty<T>){
			return SelectTrait((MushroomProperty<T>)property);
		}

		return false;
	}

	public void AddInfluencedProperty(IMushroomProperty property) {
		if (property is MushroomProperty<T> p){
			AddInfluencedProperty(p);
		}
	}

	public void OnStartApplyToProperty(IMushroomProperty property) {
		if (property is MushroomProperty<T> p){
			OnStartApplyToProperty(p);
		}
	}

	public abstract bool IsGlobalOnly { get; }


	public MushroomTrait(){
		foreach (var tag in GetTargetTags()){
			TargetTags.Add(tag);
		}
	}
	
	protected abstract void OnStartApplyToProperty(MushroomProperty<T> property);
	
	protected void AddInfluencedProperty(MushroomProperty<T> property){
		influencedProperties.Add(property);
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
