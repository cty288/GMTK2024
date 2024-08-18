using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMushroomTrait{
	public HashSet<MushroomPropertyTag> TargetTags { get; }
	public bool SelectTrait(IMushroomProperty property);

	public void AddInfluencedProperty(IMushroomProperty property);
	
	public void OnStartApplyToProperty(IMushroomProperty property);
	
	//public bool IsGlobalOnly { get; }
	
	/// <summary>
	/// Is the trait independent of properties?
	/// </summary>
	public bool IsIndependent { get; }
	
	public int GetVisualPartGroupIdx();
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

	//public abstract bool IsGlobalOnly { get; }
	public abstract bool IsIndependent { get; }
	public virtual int GetVisualPartGroupIdx() {
		return -1;
	}


	public MushroomTrait(){
		if (GetTargetTags() == null){
			return;
		}
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

public abstract class IndependentMushroomTrait: MushroomTrait<int>{
	public override bool IsIndependent { get; } = true;

	protected override void OnStartApplyToProperty(MushroomProperty<int> property) {
		
	}

	public override MushroomPropertyTag[] GetTargetTags() {
		return null;
	}

	public override string GetTraitValueDescription() {
		return null;
	}
}
