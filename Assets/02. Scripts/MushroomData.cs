using System;
using System.Collections.Generic;
using MikroFramework.Architecture;
using MikroFramework.BindableProperty;
using MikroFramework.Event;
using NHibernate.Util;
using UnityEngine;
using Action = Antlr.Runtime.Misc.Action;
using Random = UnityEngine.Random;

public enum MushroomPropertyTag {
    Cap,
    Stem,
    Height,
    Width,
    Size,
    Growth,
    Color,
    Poisonous,
    SporeRange,
    Oscillation,
    OscillationSpeed
}
public interface IMushroomProperty {
    HashSet<MushroomPropertyTag> Tags { get; }
}
public class MushroomProperty<T> : IMushroomProperty{
    public T BaseValue { get; set; }
    
    public BindableProperty<T> RealValue { get; }
    
    public HashSet<MushroomPropertyTag> Tags { get; }

    public MushroomProperty(T baseValue, params MushroomPropertyTag[] tags) {
        BaseValue = baseValue;
        RealValue = new BindableProperty<T>(baseValue);
        this.Tags = new HashSet<MushroomPropertyTag>(tags);
    }

    public override string ToString() {
        return RealValue.Value.ToString();
    }
}

public class MushroomData {
    private Dictionary<MushroomPropertyTag, List<IMushroomProperty>> properties =
        new Dictionary<MushroomPropertyTag, List<IMushroomProperty>>();

    private List<IMushroomProperty> flattenedProperties = new List<IMushroomProperty>();

    private Dictionary<Type, IMushroomTrait> traits = new Dictionary<Type, IMushroomTrait>();
    private Dictionary<Type, Action<IMushroomTrait>> traitAddCallbacks = new Dictionary<Type, Action<IMushroomTrait>>();

    public MushroomProperty<float> capHeight;
    public MushroomProperty<float> capWidth;
    public MushroomProperty<float> stemHeight;
    public MushroomProperty<float> stemWidth;
    public MushroomProperty<float> growthSpeed;
    public MushroomProperty<Vector2> oscillation;
    public MushroomProperty<float> oscillationSpeed;
    public MushroomProperty<Color> capColor;
    public MushroomProperty<Color> stemColor;
    public MushroomProperty<Color> capColor0;
    public MushroomProperty<Color> stemColor0;
    public MushroomProperty<Color> capColor1;
    public MushroomProperty<Color> stemColor1;
    public MushroomProperty<bool> isPoisonous;
    public MushroomProperty<float> sporeRange;
    
    
    public BindableProperty<int> GrowthDay { get; } = new BindableProperty<int>(1);

    public int GetStage() {
        return GetStage(GrowthDay.Value);
    }
    
    public int GetStage(int day) {
        if (day <= 2) {
            return 1;
        }else if (day <= 4) {
            return 2;
        }else if (day <= 5) {
            return 3;
        }

        return 4;
    }
    
    public MushroomData() :  this(1, 1, 1, 1, 1, new Vector2(1, 1), 1, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, false, 1) {
        
        AddBasicProperties();
    }
    
    public MushroomData(float capHeight, float capWidth, float stemHeight, float stemWidth, float growthSpeed, Vector2 oscillation, float oscillationSpeed, Color capColor, Color stemColor, Color capColor0, Color stemColor0, Color capColor1, Color stemColor1, bool isPoisonous, float sporeRange) {
        this.capHeight = new MushroomProperty<float>(capHeight,  MushroomPropertyTag.Cap, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.capWidth = new MushroomProperty<float>(capWidth, MushroomPropertyTag.Cap, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.stemHeight = new MushroomProperty<float>(stemHeight,  MushroomPropertyTag.Stem, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.stemWidth = new MushroomProperty<float>(stemWidth, MushroomPropertyTag.Stem, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.oscillation = new MushroomProperty<Vector2>(oscillation, MushroomPropertyTag.Oscillation);
        this.oscillationSpeed = new MushroomProperty<float>(oscillationSpeed, MushroomPropertyTag.OscillationSpeed);
        this.capColor = new MushroomProperty<Color>(capColor, MushroomPropertyTag.Cap,MushroomPropertyTag.Color);
        this.stemColor = new MushroomProperty<Color>(stemColor, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.capColor0 = new MushroomProperty<Color>(capColor0, MushroomPropertyTag.Cap,MushroomPropertyTag.Color);
        this.stemColor0 = new MushroomProperty<Color>(stemColor0, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.capColor1 = new MushroomProperty<Color>(capColor1, MushroomPropertyTag.Cap, MushroomPropertyTag.Color);
        this.stemColor1 = new MushroomProperty<Color>(stemColor1, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.isPoisonous = new MushroomProperty<bool>(isPoisonous, MushroomPropertyTag.Poisonous);
        this.sporeRange = new MushroomProperty<float>(sporeRange, MushroomPropertyTag.SporeRange);
        this.growthSpeed = new MushroomProperty<float>(growthSpeed, MushroomPropertyTag.Growth);
        
        AddBasicProperties();
    }

    private void AddBasicProperties() {
        AddProperty(capHeight);
        AddProperty(capWidth);
        AddProperty(capColor);
        AddProperty(capColor0);
        AddProperty(capColor1);
        AddProperty(stemHeight);
        AddProperty(stemWidth);
        AddProperty(stemColor);
        AddProperty(stemColor0);
        AddProperty(stemColor1);
        AddProperty(isPoisonous);
        AddProperty(sporeRange);
        AddProperty(oscillation);
        AddProperty(oscillationSpeed);
    }

    public int GetSellPrice() { //TODO: later
        return 2;
    }
    
    public int GetBuyPrice() { //TODO: later
        return 1;
    }
    
    public void AddProperty(IMushroomProperty property) {

        property.Tags.ForEach(tag => {
            if (!properties.ContainsKey(tag)) {
                properties.Add(tag, new List<IMushroomProperty>());
            }
            
            /*if(properties[tag].Count > 0 && properties[tag][0].GetType() != property.GetType()) {
                throw new System.Exception("Property type mismatch for tag " + tag + " in part " + part + " : " + property.GetType() + " != " + properties[part][tag][0].GetType() + "");
            }*/
            
            properties[tag].Add(property);
        });
        
  
        flattenedProperties.Add(property);
    }
    
    /*public List<MushroomProperty<T>> GetProperties<T>(ShroomPart part, MushroomPropertyTag tag) {
        if (!properties.ContainsKey(part) || !properties[part].ContainsKey(tag)) {
            return new List<MushroomProperty<T>>();
        }

        return properties[part][tag].ConvertAll(property => (MushroomProperty<T>) property);
    }
    
    public HashSet<MushroomProperty<T>> GetPropertiesFromAllParts<T>(MushroomPropertyTag tag) {
        HashSet<MushroomProperty<T>> result = new HashSet<MushroomProperty<T>>();
        
        foreach (var part in flattenedProperties.Keys) {
            if (properties.ContainsKey(part) && properties[part].ContainsKey(tag)) {
                result.UnionWith(properties[part][tag].ConvertAll(property => (MushroomProperty<T>) property));
            }
        }

        return result;
    }*/
    
    public void AddTrait<T>(MushroomTrait<T> trait) {
        AddTrait((IMushroomTrait) trait);
    }
    
    public void AddTrait(IMushroomTrait trait) {
        if (trait.IsIndependent) {
            traits.Add(trait.GetType(), trait);
            if (traitAddCallbacks.ContainsKey(trait.GetType())) {
                traitAddCallbacks[trait.GetType()](trait);
            }
            return;
        }
        
        foreach (var property in flattenedProperties) {
            if (trait.SelectTrait(property)) {
                trait.AddInfluencedProperty(property);
                trait.OnStartApplyToProperty(property);
            }
        }
        traits.Add(trait.GetType(), trait);
        if (traitAddCallbacks.ContainsKey(trait.GetType())) {
            traitAddCallbacks[trait.GetType()](trait);
        }
        
    }
    
    private Dictionary<object, Action<IMushroomTrait>> traitRemoveCallbacks = new Dictionary<object, Action<IMushroomTrait>>();
    public void RegisterOnTraitAdd<T>(Action<T> callback, bool callIfExist = true) where T : IMushroomTrait {
        Action<IMushroomTrait> action = trait => callback((T) trait);
        traitRemoveCallbacks.Add(callback, action);
        
        if (!traitAddCallbacks.ContainsKey(typeof(T))) {
            traitAddCallbacks.Add(typeof(T), action);
        }
        else {
            traitAddCallbacks[typeof(T)] += action;
        }
        if (callIfExist) {
            if (traits.ContainsKey(typeof(T))) {
                callback((T) traits[typeof(T)]);
            }
        }
    }
    
    public void UnregisterOnTraitAdd<T>(Action<T> callback) where T : IMushroomTrait {
        Action<IMushroomTrait> action = traitRemoveCallbacks[callback];
        traitAddCallbacks[typeof(T)] -= action;
        traitRemoveCallbacks.Remove(callback);
    }
    
    public List<IMushroomTrait> GetTraits() {
        return new List<IMushroomTrait>(traits.Values);
    }
    
    public bool HasTrait<T>() where T : IMushroomTrait {
        return traits.ContainsKey(typeof(T));
    }
    
    public bool HasTrait<T>(out T trait) where T : IMushroomTrait {
        if (traits.ContainsKey(typeof(T))) {
            trait = (T) traits[typeof(T)];
            return true;
        }

        trait = default;
        return false;
    }
}

public static class MushroomDataHelper {
    public static MushroomData GetControlMushroomData() {
        return new MushroomData();
    }
    
    
    public static MushroomData GetInitialMushroomData() {
        MushroomData data = new MushroomData(
            Random.Range(0.5f, 1),
            Random.Range(0.5f, 1),
            Random.Range(0.5f, 1),
            Random.Range(0.5f, 1),
            1,
            new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1)),
            Random.Range(0.5f, 1),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            Random.value > 0.5f,
            Random.Range(0.5f, 1));

        return data;
    }
    
    public static MushroomData GetRandomMushroomData(int initialGrowthDay, int minTraitCount, int maxTraitCount) {
        MushroomData data = new MushroomData(
            Random.Range(0.3f, 1.8f),
            Random.Range(0.3f, 1.8f),
            Random.Range(0.3f, 1.8f),
            Random.Range(0.3f, 1.8f),
            1,
            new Vector2(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f)),
            Random.Range(0.3f, 0.9f),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            Random.value > 0.5f,
            Random.Range(0.8f, 1.6f));
        data.GrowthDay.Value = initialGrowthDay;
        
        int traitCount = Random.Range(minTraitCount, maxTraitCount + 1);
        var traits = TraitPool.GetRandomTraits(traitCount);

        
        
        foreach (var trait in traits) {
            data.AddTrait(trait);
        }
        return data;

    }

    public static string ToString(MushroomData data) {
        return
                $"Cap Height: {data.capHeight}\n" +
                $"Cap Width: {data.capWidth}\n" +
                $"Stem Height: {data.stemHeight}\n" +
                $"Stem Width: {data.stemWidth}\n" +
                $"Oscillation: {data.oscillation}\n" +
                $"Oscillation Speed: {data.oscillationSpeed}\n" +
                $"Cap Color: {data.capColor}\n" +
                $"Stem Color: {data.stemColor}\n" +
                $"Is Poisonous: {data.isPoisonous}\n" +
                $"Spore Range: {data.sporeRange}";
    }
    
    
}