using MikroFramework.BindableProperty;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Action = Antlr.Runtime.Misc.Action;
using Random = UnityEngine.Random;

public enum MushroomPropertyTag {
    Cap,
    Stem,
    Ring,
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
public class MushroomProperty<T> : IMushroomProperty {
    private T BaseValue { get; set; }
    
    private T MinValue { get; set; }

    public BindableProperty<T> RealValue { get; }
    


    public HashSet<MushroomPropertyTag> Tags { get; }

    public T ChildAdditions { get; set; }

    public bool CompareMin { get; set; } = true;
    
    //a == b
    public static implicit operator T(MushroomProperty<T> property) {
        return property.RealValue.Value;
    }

    public bool Locked {
        get => RealValue.Locked;
        set => RealValue.Locked = value;
    }
    public T Value {
        get => RealValue.Value;
        set
        {
            if (CompareMin && value is IComparable comparable) {
                if (comparable.CompareTo(MinValue) < 0) {
                    RealValue.Value = MinValue;
                    return;
                }
            }
            RealValue.Value = value;
        }
    }




    public MushroomProperty(T baseValue, T minValue, params MushroomPropertyTag[] tags) {
        BaseValue = baseValue;
        RealValue = new BindableProperty<T>(baseValue);
        this.Tags = new HashSet<MushroomPropertyTag>(tags);
        MinValue = minValue;
    }

    public override string ToString() {
        return RealValue.Value.ToString();
    }
}

public class MushroomData {
    private Dictionary<MushroomTraitCategory, MushroomProperty<float>[]> traitToPropertyMap =
        new Dictionary<MushroomTraitCategory, MushroomProperty<float>[]>();

    public Dictionary<MushroomTraitCategory, MushroomProperty<float>[]> TraitToPropertyMap => traitToPropertyMap;

    private Dictionary<MushroomTraitCategory, MushroomData> traitToParentMap =
        new Dictionary<MushroomTraitCategory, MushroomData>();
    public Dictionary<MushroomTraitCategory, MushroomData> TraitToParentMap => traitToParentMap;


    private HashSet<MushroomData> influencedBy = new HashSet<MushroomData>();

    private Dictionary<MushroomPropertyTag, List<IMushroomProperty>> properties =
        new Dictionary<MushroomPropertyTag, List<IMushroomProperty>>();

    private List<IMushroomProperty> flattenedProperties = new List<IMushroomProperty>();

    private Dictionary<Type, IMushroomTrait> traits = new Dictionary<Type, IMushroomTrait>();
    private Dictionary<Type, Action<IMushroomTrait>> traitAddCallbacks = new Dictionary<Type, Action<IMushroomTrait>>();
    private Dictionary<MushroomTraitCategory, IMushroomTrait> traitCategoryToTrait = new Dictionary<MushroomTraitCategory, IMushroomTrait>();


    private Action<IMushroomTrait> onTraitAdd;

    public MushroomProperty<float> capHeight;
    public MushroomProperty<float> capWidth;
    public MushroomProperty<float> stemHeight;
    public MushroomProperty<float> stemWidth;
    //public MushroomProperty<float> growthSpeed;
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
    public MushroomProperty<int> extraSellPrice;
    public MushroomProperty<int> sellPriceLocker;
    public MushroomProperty<int> baseSellPrice;
    public MushroomProperty<float> sellPriceMultiplier; 
    
    private Action onUpdateColor;

    private int neighborCount = 0;
    public Dictionary<ShroomPart, MushroomPart> Parts { get; set; }
    public int NeighborCount {
        get => neighborCount;
        set => neighborCount = value;
    }

    public BindableProperty<int> GrowthDay { get; } = new BindableProperty<int>(1);
    private bool isOnFarm = false;

    public bool IsOnFarm => isOnFarm;

    public int GetStage() {
        return GetStage(GrowthDay.Value);
    }

    public int GetStage(int day) {
        if (day <= 2) {
            return 1;
        } else if (day <= 4) {
            return 2;
        } else if (day <= 5) {
            return 3;
        }

        return 4;
    }

    public MushroomData() : this(1, 1, 1, 1, new Vector2(1, 1), 1, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, false, 1, 0) {
        AddBasicProperties();
    }

    public void AddInfluencedBy(MushroomData parent) {
        influencedBy.Add(parent);
    }

    public void OnPlantToFarm() {
        this.isOnFarm = true;
        foreach (IMushroomTrait trait in traits.Values) {
            trait.OnMushroomPlantOnFarm(this);
        }
    }

    public void SendUpdateColorEvent() {
        onUpdateColor?.Invoke();
    }
    
    public void RegisterOnUpdateColor(Action callback) {
        onUpdateColor += callback;
    }
    
    public void UnregisterOnUpdateColor(Action callback) {
        onUpdateColor -= callback;
    }
    
    private void OnGrowthDayChanged(int oldDay, int newDay) {
        int oldStage = GetStage(oldDay);
        int newStage = GetStage(newDay);


        if (newStage == 1) {
            OnStage1();
        } else if (newStage == 2) {
            if (newDay == 3) {
                OnStage2Start();
            }
            OnStage2();
        } else if (newStage == 3) {
            OnStage3();
        }
    }

    private void OnStage3() {

    }

    private void OnStage2() {
        //growth & mutation
        //for each empty trait slot, mutate a property
        if (HasTrait<SuperShy>() && neighborCount > 0) {
            return;
        }
        foreach (var trait in traits.Values) {
            trait.OnStage2Grow(this);
        }
        
        MushroomTraitCategory[] categories = traitCategoryToTrait.Keys.ToArray();

        foreach (MushroomTraitCategory category in categories) {
            if (traitCategoryToTrait[category] == null) {
                if (Random.value <= 0) {
                    MushroomProperty<float>[] properties = traitToPropertyMap[category];
                    foreach (var property in properties) {
                        property.Value = Mathf.Max(property.Value + Random.Range(-1f, 1f), 0.5f);
                    }


                    Debug.Log("Mutated property " + category);
                } else {
                    var trait = TraitPool.GetRandomTrait(category);
                    if (Random.value < 0.1f && isOnFarm) {
                        var specialTrait = TraitPool.GetRandomMutationOnlyTrait(category);
                        if (specialTrait != null) {
                            trait = specialTrait;
                        }
                    }
                    
                    if (trait != null) {
                        AddTrait(trait);
                        Debug.Log("Mutation Added trait " + trait.GetTraitName());
                    }
                }
            }
        }
    }
    
    public void OnDestroy() {
        foreach (var trait in traits.Values) {
            trait.OnEnd(this);
        }
        traits.Clear();
        influencedBy.Clear();
        traitToParentMap.Clear();

        
    }

    private void OnStage2Start() {
        // for each mappedProperty, get the property from a parent that did not give a trait slot related to the mappedProperty
        // for each other property, get a random parent's trait or mix a few parent's traits together

        //step0: color inheritance
        MushroomPropertyTag[][] tagGroups = new MushroomPropertyTag[][] {
            new MushroomPropertyTag[] {MushroomPropertyTag.Cap, MushroomPropertyTag.Color},
            new MushroomPropertyTag[] {MushroomPropertyTag.Stem, MushroomPropertyTag.Color},

        };

        foreach (var tagGroup in tagGroups) {
            if (Random.value > 0.5f) {
                continue;
            }
            
            if (influencedBy.Count == 0) {
                break;
            }
            //MushroomData parent = influencedBy.ToList()[Random.Range(0, influencedBy.Count)];
            var selfColor = GetProperties<Color>(tagGroup).ToList();
            //var parentColor = parent.GetProperties<Color>(tagGroup).ToList();
            for (int i = 0; i < selfColor.Count; i++) {
                Color result = new Color(0, 0, 0);
                foreach (var parent in influencedBy) {
                    var parentColor = parent.GetProperties<Color>(tagGroup).ToList();
                    result += parentColor[i].Value;
                }
                result /= influencedBy.Count;
                result *= Random.Range(0.8f, 1.2f);
                result.a = 1;
                selfColor[i].Value = result;
            }
        }

        //step1: add traits
        foreach (MushroomTraitCategory category in traitToParentMap.Keys) {
            if (traitToParentMap[category] != null) {
                var parent = traitToParentMap[category];
                IMushroomTrait parentTrait = parent.GetTraits().Find(trait => trait.Category == category);
                if (parentTrait != null) {
                    AddTrait(parentTrait.GetCopy());
                    Debug.Log("Added trait " + parentTrait.GetTraitName());
                }
            }
        }

        //step2: inherit properties
        foreach (MushroomTraitCategory category in traitToPropertyMap.Keys) {
            MushroomData excludedParent = traitToParentMap[category];
            HashSet<MushroomData> targetParents = new HashSet<MushroomData>(influencedBy);
            if (excludedParent != null) {
                targetParents.Remove(excludedParent);
            }
            if (targetParents.Count == 0) {
                continue;
            }
            MushroomData parent = targetParents.ToList()[Random.Range(0, targetParents.Count)];

            MushroomProperty<float>[] selfProperties = traitToPropertyMap[category];
            MushroomProperty<float>[] parentProperties = parent.traitToPropertyMap[category];

            for (int i = 0; i < selfProperties.Length; i++) {
                selfProperties[i].Value = parentProperties[i].Value + parentProperties[i].ChildAdditions;
            }

            Debug.Log($"Inherited property {category} from parent {parent.GetHashCode()}");
        }

        //step3: stemwidth and spore range
        int parentCount = influencedBy.Count;
        stemWidth.Value = Mathf.Clamp(stemWidth.Value + parentCount * 0.3f, stemWidth.Value, 3f);
        sporeRange.Value = Mathf.Clamp(sporeRange.Value + parentCount * 0.5f, sporeRange.Value, 6f);

        //step4: clean up
        influencedBy.Clear();
        traitToParentMap.Clear();


    }

    private void OnStage1() {

    }

    public MushroomData(float capHeight, float capWidth, float stemHeight, float stemWidth, Vector2 oscillation, float oscillationSpeed, Color capColor, Color stemColor, Color capColor0, Color stemColor0, Color capColor1, Color stemColor1, bool isPoisonous, float sporeRange, int baseSellPrice) {
        this.capHeight = new MushroomProperty<float>(capHeight,0.2f, MushroomPropertyTag.Cap, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.capWidth = new MushroomProperty<float>(capWidth, 0.2f, MushroomPropertyTag.Cap, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.stemHeight = new MushroomProperty<float>(stemHeight, 0.2f, MushroomPropertyTag.Stem, MushroomPropertyTag.Height, MushroomPropertyTag.Size);
        this.stemWidth = new MushroomProperty<float>(stemWidth,0.2f,  MushroomPropertyTag.Stem, MushroomPropertyTag.Width, MushroomPropertyTag.Size);
        this.oscillation = new MushroomProperty<Vector2>(oscillation, default, MushroomPropertyTag.Oscillation);
        this.oscillationSpeed = new MushroomProperty<float>(oscillationSpeed, default, MushroomPropertyTag.OscillationSpeed);
        this.capColor = new MushroomProperty<Color>(capColor, default, MushroomPropertyTag.Cap, MushroomPropertyTag.Color);
        this.stemColor = new MushroomProperty<Color>(stemColor, default, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.capColor0 = new MushroomProperty<Color>(capColor0, default, MushroomPropertyTag.Cap, MushroomPropertyTag.Color);
        this.stemColor0 = new MushroomProperty<Color>(stemColor0, default, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.capColor1 = new MushroomProperty<Color>(capColor1, default, MushroomPropertyTag.Cap, MushroomPropertyTag.Color);
        this.stemColor1 = new MushroomProperty<Color>(stemColor1, default, MushroomPropertyTag.Stem, MushroomPropertyTag.Color);
        this.isPoisonous = new MushroomProperty<bool>(isPoisonous, default, MushroomPropertyTag.Poisonous);
        this.sporeRange = new MushroomProperty<float>(sporeRange, 0.2f,MushroomPropertyTag.SporeRange);
        this.extraSellPrice = new MushroomProperty<int>(0, 0);
        this.sellPriceMultiplier = new MushroomProperty<float>(1, 0);
        this.sellPriceLocker = new MushroomProperty<int>(-1, default);
        this.sellPriceLocker.CompareMin = false;
        
        this.baseSellPrice = new MushroomProperty<int>(baseSellPrice, 1);
        
        // this.growthSpeed = new MushroomProperty<float>(growthSpeed, MushroomPropertyTag.Growth);
        //this.stemWidth.RealValue.Value = 2;
        AddBasicProperties();
        
        GrowthDay.RegisterOnValueChanged(OnGrowthDayChanged);
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
        AddProperty(extraSellPrice);
        AddProperty(sellPriceLocker);

        traitToPropertyMap.Add(MushroomTraitCategory.Cap, new MushroomProperty<float>[] { capHeight, capWidth });
        traitToPropertyMap.Add(MushroomTraitCategory.Stem, new MushroomProperty<float>[] { stemHeight, stemWidth });
        traitToPropertyMap.Add(MushroomTraitCategory.Ring, new MushroomProperty<float>[] { });

        traitToParentMap.Add(MushroomTraitCategory.Cap, null);
        traitToParentMap.Add(MushroomTraitCategory.Stem, null);
        traitToParentMap.Add(MushroomTraitCategory.Ring, null);

        traitCategoryToTrait.Add(MushroomTraitCategory.Cap, null);
        traitCategoryToTrait.Add(MushroomTraitCategory.Stem, null);
        traitCategoryToTrait.Add(MushroomTraitCategory.Ring, null);

    }

    public int GetSellPrice() { //TODO: later



        //=================================================
        int finalPrice =
            Mathf.RoundToInt(Math.Max(0, extraSellPrice.Value + baseSellPrice.Value) * sellPriceMultiplier.Value);
        
        if (sellPriceLocker.Value >= 0) {
            finalPrice = sellPriceLocker.Value;
        }
        return finalPrice;
    }

    public int GetBuyPrice() { //TODO: later
        return Mathf.Max(1, Mathf.RoundToInt(GetSellPrice() * 1.2f));
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

    public void AddTrait<T>(IMushroomTrait trait) {
        AddTrait(trait);
    }

    public void ReplaceTrait(IMushroomTrait trait) {
        RemoveTrait(trait.Category);
        AddTrait(trait);
    }
    
    public void RemoveTrait(MushroomTraitCategory category) {
        if (traitCategoryToTrait[category] == null) {
            return;
        }
        RemoveTrait(traitCategoryToTrait[category].GetType());
    }
    
    public void RemoveTrait(Type traitType) {
        if (!traits.ContainsKey(traitType)) {
            return;
        }
        IMushroomTrait trait = traits[traitType];
        trait.OnEnd(this);
        traits.Remove(traitType);
        if (traitRemoveCallbacks.ContainsKey(traitType)) {
            traitRemoveCallbacks[traitType]((IMushroomTrait)trait);
        }
        traitCategoryToTrait[trait.Category] = null;
    }
    
    public void AddTrait(IMushroomTrait trait) {
        if (traitCategoryToTrait[trait.Category] != null) {
            Debug.LogWarning($"Trait {trait} already exists for category {trait.Category}");
            return;
        }
        trait.OnStartApply(this);
        traits.Add(trait.GetType(), trait);
        if (traitAddCallbacks.ContainsKey(trait.GetType())) {
            traitAddCallbacks[trait.GetType()](trait);
        }
        traitCategoryToTrait[trait.Category] = trait;
        if (isOnFarm) {
            trait.OnMushroomPlantOnFarm(this);
        }

    }

    private Dictionary<object, Action<IMushroomTrait>> traitRemoveCallbacks = new Dictionary<object, Action<IMushroomTrait>>();
    public void RegisterOnTraitAdd<T>(Action<T> callback, bool callIfExist = true) where T : IMushroomTrait {
        Action<IMushroomTrait> action = trait => callback((T)trait);
        traitRemoveCallbacks.Add(callback, action);

        if (!traitAddCallbacks.ContainsKey(typeof(T))) {
            traitAddCallbacks.Add(typeof(T), action);
        } else {
            traitAddCallbacks[typeof(T)] += action;
        }
        if (callIfExist) {
            if (traits.ContainsKey(typeof(T))) {
                callback((T)traits[typeof(T)]);
            }
        }
    }

    public void RegisterOnTraitAdd(Action<IMushroomTrait> callback, bool callIfExist = true) {
        if (onTraitAdd == null) {
            onTraitAdd = callback;
        } else {
            onTraitAdd += callback;
        }
        if (callIfExist) {
            foreach (var trait in traits.Values) {
                callback(trait);
            }
        }
    }

    public void UnregisterOnTraitAdd(Action<IMushroomTrait> callback) {
        onTraitAdd -= callback;
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
            trait = (T)traits[typeof(T)];
            return true;
        }

        trait = default;
        return false;
    }

    public float GetSize()
    {
        return (capHeight + capWidth + stemHeight + stemWidth) / 4.0f;
    }

    public void StopGrowth()
    {
        GrowthDay.UnRegisterOnValueChanged(OnGrowthDayChanged);
    }

    /// <summary>
    /// AND operation
    /// </summary>
    /// <param name="tags"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public HashSet<MushroomProperty<T>> GetProperties<T>(params MushroomPropertyTag[] tags) {
        //find properties that have all the tags
        HashSet<IMushroomProperty> result = new HashSet<IMushroomProperty>(flattenedProperties);
        foreach (var tag in tags) {
            if (!properties.ContainsKey(tag)) {
                return null;
            }

            result.IntersectWith(properties[tag]);
        }

        //filter out those of different types
        result.RemoveWhere(property => property.GetType() != typeof(MushroomProperty<T>));

        return new HashSet<MushroomProperty<T>>(result.Cast<MushroomProperty<T>>());
    }
}


public static class MushroomDataHelper {
    public static MushroomData GetControlMushroomData() {
        return new MushroomData();
    }


    public static MushroomData GetInitialMushroomData() {
        MushroomData data = new MushroomData(
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),

            new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1)),
            Random.Range(0.5f, 1),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            Random.value > 0.5f,
            Random.Range(3f, 4f),
            Random.Range(0, 2));

        return data;
    }

    public static MushroomData GetRandomMushroomData(int initialGrowthDay, int minTraitCount, int maxTraitCount) {
        MushroomData data = new MushroomData(
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),
            Random.Range(1f, 1.2f),
            new Vector2(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f)),
            Random.Range(0.3f, 0.9f),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            new Color(Random.value, Random.value, Random.value),
            Random.value > 0.5f,
            Random.Range(3f, 4f),
            Random.Range(0, 4));
        data.GrowthDay.Value = initialGrowthDay;

        int traitCount = Random.Range(minTraitCount, maxTraitCount + 1);
        var traits = TraitPool.GetRandomTraits(traitCount);

        foreach (var trait in traits) {
            data.AddTrait(trait);
        }
        return data;

    }

    public static MushroomData CopyMushroomData(MushroomData data)
    {
        var mushroom = new MushroomData(data.capHeight, data.capWidth, data.stemHeight, data.stemWidth, data.oscillation, data.oscillationSpeed, data.capColor, data.stemColor, data.capColor0, data.stemColor0, data.capColor1, data.stemColor1, data.isPoisonous, data.sporeRange, data.baseSellPrice);

        mushroom.GrowthDay.Value = data.GrowthDay;
        
        // Prevent mushroom from instantly destroying itself.
        if (mushroom.GetStage() > 3)
        {
            mushroom.GrowthDay.Value = 5;
        }

        var traits = data.GetTraits();
        foreach (var trait in traits)
        {
            mushroom.AddTrait(trait.GetCopy());
        }

        mushroom.Parts = MushroomGenerator.RegetParts(mushroom, data.Parts);
        
        mushroom.StopGrowth();

        return mushroom;
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
            $"Spore Range: {data.sporeRange}\n" +
            $"Sell Price: {data.GetSellPrice()}\n";
    }


}