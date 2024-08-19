using System;
using System.Collections.Generic;
using System.Linq;

[System.Flags]
public enum TraitFlags {
    None = 0,
    Good = 1 << 1,
    Bad = 1 << 2,
}

public static class TraitPool {
    private static List<Func<IMushroomTrait>> traitGetters = new List<Func<IMushroomTrait>>();

    private static Dictionary<TraitFlags, List<Func<IMushroomTrait>>> traitsByFlags =
        new Dictionary<TraitFlags, List<Func<IMushroomTrait>>>();

    private static Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>> traitsByCategory =
        new Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>>();

    private static Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>>
        mutationOnlyTraits = new Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>>();
    
    
    private static Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>>
        shopOnlyTraits = new Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>>();
     

    static TraitPool() {
        //Caps
        RegisterTrait(() => new BigCap());
        RegisterTrait(() => new SplitedTrait());
        RegisterTrait(() => new PetrifiedCap());
        RegisterTrait(() => new FishyScale());
        RegisterTrait(() => new Flowery());
        RegisterTrait(() => new AlienBanana());
        RegisterTrait(() => new OnionCap());
        RegisterTrait(() => new StandardCap());
        RegisterTrait(() => new TooBlue());
        RegisterTrait(() => new UltraRare());
        RegisterTrait(() => new HighHat());
        RegisterTrait(() => new GravityPull());
        RegisterTrait(() => new AMouth());
        RegisterTrait(() => new TooTasty());
        RegisterTrait(() => new Leadership());
       

        //Stems
        RegisterTrait(() => new BigFoot());
        RegisterTrait(() => new Gourd());
        RegisterTrait(() => new PetrifiedStem());
        RegisterTrait(() => new StandardStem());
        RegisterTrait(() => new StraightEdge());
        RegisterTrait(() => new Strangled());
        RegisterTrait(() => new Thorny());
        RegisterTrait(() => new TreeRoot());
        RegisterTrait(() => new SuperShy());
        RegisterTrait(() => new Dink());
        RegisterTrait(() => new IsItDead());
        RegisterTrait(() => new Squeezed());
        RegisterTrait(() => new SpringTrait());
        RegisterTrait(() => new StickTrait());
        RegisterTrait(() => new Poisonous());
        RegisterTrait(() => new Antisocial());

        //Rings
        RegisterTrait(() => new SuppressorRing());
        RegisterTrait(() => new StandardRing());
        RegisterTrait(() => new Extravagant());
        RegisterTrait(() => new DriedOut());
        RegisterTrait(() => new Skirt());
        RegisterTrait(() => new Tube());
        RegisterTrait(() => new Hallucinogenic());
        
        RegisterSpecialPoolTrait(() => new AHatTrait(), mutationOnlyTraits);
        RegisterSpecialPoolTrait(() => new SporesTrait(), mutationOnlyTraits);
        
        RegisterSpecialPoolTrait(() => new LazyTrait(), shopOnlyTraits);
        RegisterSpecialPoolTrait(() => new BetterBreed(), shopOnlyTraits);
    }

    private static void RegisterSpecialPoolTrait(Func<IMushroomTrait> traitGetter, Dictionary<MushroomTraitCategory, List<Func<IMushroomTrait>>> pool) {
        MushroomTraitCategory category = traitGetter().Category;
        if (!pool.ContainsKey(category)) {
            pool[category] = new List<Func<IMushroomTrait>>();
        }
        pool[category].Add(traitGetter);
    }
    public static void RegisterTrait(Func<IMushroomTrait> traitGetter, TraitFlags flags = TraitFlags.None) {
        traitGetters.Add(traitGetter);
        foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
            if (flags.HasFlag(flag)) {
                if (!traitsByFlags.ContainsKey(flag)) {
                    traitsByFlags[flag] = new List<Func<IMushroomTrait>>();
                }
                traitsByFlags[flag].Add(traitGetter);
            }
        }

        MushroomTraitCategory category = traitGetter().Category;
        if (!traitsByCategory.ContainsKey(category)) {
            traitsByCategory[category] = new List<Func<IMushroomTrait>>();
        }
        traitsByCategory[category].Add(traitGetter);
    }

    public static void Shuffle<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public static IMushroomTrait GetRandomMutationOnlyTrait(MushroomTraitCategory category) {
        if (!mutationOnlyTraits.ContainsKey(category)) {
            return null;
        }
        List<Func<IMushroomTrait>> candidates = mutationOnlyTraits[category];
        if (candidates == null) {
            return null;
        }
        Shuffle(candidates);
        return candidates[0]();
    }
    
    public static IMushroomTrait GetRandomShopOnlyTrait() {
        List<Func<IMushroomTrait>> candidates = shopOnlyTraits.Values.SelectMany(x => x).ToList();
        
        Shuffle(candidates);
        return candidates[0]();
    }
    
    
    public static IMushroomTrait GetRandomTrait(MushroomTraitCategory category) {
        if (!traitsByCategory.ContainsKey(category)) {
            return null;
        }
        List<Func<IMushroomTrait>> candidates = traitsByCategory[category];
        if (candidates == null) {
            return null;
        }
        Shuffle(candidates);
        return candidates[0]();
    }

    public static List<IMushroomTrait> GetRandomTraits(int count = 1, TraitFlags flags = TraitFlags.None, bool isOR = true) {
        List<Func<IMushroomTrait>> result = new List<Func<IMushroomTrait>>();
        if (flags == TraitFlags.None) {
            result = traitGetters;
            List<IMushroomTrait> traits = new List<IMushroomTrait>();
            Shuffle(result);
            for (int i = 0; i < count; i++) {
                traits.Add(result[i]());
            }
            return traits;
        }

        HashSet<Func<IMushroomTrait>> candidates = new HashSet<Func<IMushroomTrait>>();
        if (isOR) {
            foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
                if (flags.HasFlag(flag) && traitsByFlags.ContainsKey(flag)) {
                    candidates.UnionWith(traitsByFlags[flag]);
                }
            }
        } else {
            candidates.UnionWith(traitGetters);
            foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
                if (flags.HasFlag(flag) && traitsByFlags.ContainsKey(flag)) {
                    candidates.IntersectWith(traitsByFlags[flag]);
                }
            }
        }
        if (candidates.Count == 0) {
            return new List<IMushroomTrait>();
        }

        result.AddRange(candidates);
        Shuffle(result);
        List<IMushroomTrait> resultTraits = new List<IMushroomTrait>();
        for (int i = 0; i < count; i++) {
            resultTraits.Add(result[i]());
        }
        return resultTraits;
    }


}
