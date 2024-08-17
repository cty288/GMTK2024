using System.Collections.Generic;
using MikroFramework.BindableProperty;
using NHibernate.Util;
using UnityEngine;

public enum MushroomPropertyTag {
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
    private Dictionary<ShroomPart, Dictionary<MushroomPropertyTag, List<IMushroomProperty>>> properties =
        new Dictionary<ShroomPart, Dictionary<MushroomPropertyTag, List<IMushroomProperty>>>();

    private Dictionary<ShroomPart, List<IMushroomProperty>> flattenedProperties =
        new Dictionary<ShroomPart, List<IMushroomProperty>>();
    
    private List<IMushroomTrait> traits = new List<IMushroomTrait>();

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
    
    
    public BindableProperty<int> GrowthDay { get; } = new BindableProperty<int>(2);

    public int GetStage() {
        if (GrowthDay <= 2) {
            return 1;
        }else if (GrowthDay <= 4) {
            return 2;
        }

        return 3;
    }
    
    public MushroomData() :  this(1, 1, 1, 1, 1, new Vector2(1, 1), 1, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, false, 1) {
        
        AddBasicProperties();
    }
    
    public MushroomData(float capHeight, float capWidth, float stemHeight, float stemWidth, float growthSpeed, Vector2 oscillation, float oscillationSpeed, Color capColor, Color stemColor, Color capColor0, Color stemColor0, Color capColor1, Color stemColor1, bool isPoisonous, float sporeRange) {
        this.capHeight = new MushroomProperty<float>(capHeight, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.capWidth = new MushroomProperty<float>(capWidth, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.stemHeight = new MushroomProperty<float>(stemHeight, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.stemWidth = new MushroomProperty<float>(stemWidth, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.oscillation = new MushroomProperty<Vector2>(oscillation, MushroomPropertyTag.Oscillation);
        this.oscillationSpeed = new MushroomProperty<float>(oscillationSpeed, MushroomPropertyTag.OscillationSpeed);
        this.capColor = new MushroomProperty<Color>(capColor, MushroomPropertyTag.Color);
        this.stemColor = new MushroomProperty<Color>(stemColor, MushroomPropertyTag.Color);
        this.capColor0 = new MushroomProperty<Color>(capColor0, MushroomPropertyTag.Color);
        this.stemColor0 = new MushroomProperty<Color>(stemColor0, MushroomPropertyTag.Color);
        this.capColor1 = new MushroomProperty<Color>(capColor1, MushroomPropertyTag.Color);
        this.stemColor1 = new MushroomProperty<Color>(stemColor1, MushroomPropertyTag.Color);
        this.isPoisonous = new MushroomProperty<bool>(isPoisonous, MushroomPropertyTag.Poisonous);
        this.sporeRange = new MushroomProperty<float>(sporeRange, MushroomPropertyTag.SporeRange);
        this.growthSpeed = new MushroomProperty<float>(growthSpeed, MushroomPropertyTag.Growth);
        
        AddBasicProperties();
    }

    private void AddBasicProperties() {
        AddProperty(ShroomPart.Cap, capHeight);
        AddProperty(ShroomPart.Cap, capWidth);
        AddProperty(ShroomPart.Cap, capColor);
        AddProperty(ShroomPart.Cap, capColor0);
        AddProperty(ShroomPart.Cap, capColor1);
        AddProperty(ShroomPart.Stem, stemHeight);
        AddProperty(ShroomPart.Stem, stemWidth);
        AddProperty(ShroomPart.Stem, stemColor);
        AddProperty(ShroomPart.Stem, stemColor0);
        AddProperty(ShroomPart.Stem, stemColor1);
        AddProperty(ShroomPart.Global, isPoisonous);
        AddProperty(ShroomPart.Global, sporeRange);
        AddProperty(ShroomPart.Global, oscillation);
        AddProperty(ShroomPart.Global, oscillationSpeed);
    }

    public int GetSellPrice() { //TODO: later
        return 2;
    }
    
    public int GetBuyPrice() { //TODO: later
        return 1;
    }
    
    public void AddProperty(ShroomPart part, IMushroomProperty property) {
        if (!properties.ContainsKey(part)) {
            properties.Add(part, new Dictionary<MushroomPropertyTag, List<IMushroomProperty>>());
        }

        property.Tags.ForEach(tag => {
            if (!properties[part].ContainsKey(tag)) {
                properties[part].Add(tag, new List<IMushroomProperty>());
            }
            
            if(properties[part][tag].Count > 0 && properties[part][tag][0].GetType() != property.GetType()) {
                throw new System.Exception("Property type mismatch for tag " + tag + " in part " + part + " : " + property.GetType() + " != " + properties[part][tag][0].GetType() + "");
            }
            
            properties[part][tag].Add(property);
        });
        
        if (!flattenedProperties.ContainsKey(part)) {
            flattenedProperties.Add(part, new List<IMushroomProperty>());
        }
        
        flattenedProperties[part].Add(property);
    }
    
    public List<MushroomProperty<T>> GetProperties<T>(ShroomPart part, MushroomPropertyTag tag) {
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
    }
    
    public void AddTrait<T>(ShroomPart part, MushroomTrait<T> trait) {
        if (!flattenedProperties.ContainsKey(part)) {
            return;
        }
        foreach (var property in flattenedProperties[part]) {
            if(property is MushroomProperty<T> tProperty) {
                if (trait.SelectTrait(tProperty)) {
                    trait.AddInfluencedProperty(tProperty);
                    trait.OnStartApplyToProperty(tProperty);
                }
            }
        }
        traits.Add(trait);
    }
    
    public void AddTraitToParts<T>(ShroomPart[] parts, MushroomTrait<T> trait) {
        parts.ForEach(part => AddTrait(part, trait));
    }
    
    public void AddTraitToAllParts<T>(MushroomTrait<T> trait) {
        foreach (var part in flattenedProperties.Keys) {
            AddTrait(part, trait);
        }
    }
    
    public List<IMushroomTrait> GetTraits() {
        return traits;
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
    
    public static MushroomData GetRandomMushroomData() {
        /*return new MushroomData {
            capHeight = Random.Range(0.3f, 1.8f),
            capWidth = Random.Range(0.3f, 1.8f),
            stemHeight = Random.Range(0.3f, 1.8f),
            stemWidth = Random.Range(0.3f, 1.8f),
            oscillation = new Vector2(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f)),
            oscillationSpeed = Random.Range(0.3f, 0.9f),
            capColor = new Color(Random.value, Random.value, Random.value),
            capColor0 = new Color(Random.value, Random.value, Random.value),
            capColor1 = new Color(Random.value, Random.value, Random.value),
            stemColor = new Color(Random.value, Random.value, Random.value),
            stemColor0 = new Color(Random.value, Random.value, Random.value),
            stemColor1 = new Color(Random.value, Random.value, Random.value),

            isPoisonous = Random.value > 0.5f,
            sporeRange = Random.Range(0.8f, 1.6f)
        };*/

        return new MushroomData(
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