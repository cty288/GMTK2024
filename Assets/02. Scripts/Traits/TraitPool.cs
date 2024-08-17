using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    static TraitPool() {
        RegisterTrait(() => new VeryBlue(), TraitFlags.Good);
        RegisterTrait(() => new VeryRed(), TraitFlags.Bad | TraitFlags.Good);
    }

    public static void RegisterTrait(Func<IMushroomTrait> traitGetter, TraitFlags flags = TraitFlags.None) {
        traitGetters.Add(traitGetter);
        foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
            if(flags.HasFlag(flag)) {
                if(!traitsByFlags.ContainsKey(flag)) {
                    traitsByFlags[flag] = new List<Func<IMushroomTrait>>();
                }
                traitsByFlags[flag].Add(traitGetter);
            }
        }
    }
    
    public static void Shuffle<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    
    public static List<IMushroomTrait> GetRandomTraits(int count = 1, TraitFlags flags = TraitFlags.None, bool isOR = true) {
        List<Func<IMushroomTrait>> result = new List<Func<IMushroomTrait>>();
        if(flags == TraitFlags.None) {
            result = traitGetters;
            List<IMushroomTrait> traits = new List<IMushroomTrait>();
            Shuffle(result);
            for (int i = 0; i < count; i++) {
                traits.Add(result[i]());
            }
            return traits;
        }
        
        HashSet<Func<IMushroomTrait>> candidates = new HashSet<Func<IMushroomTrait>>();
        if(isOR) {
            foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
                if(flags.HasFlag(flag) && traitsByFlags.ContainsKey(flag)) {
                    candidates.UnionWith(traitsByFlags[flag]);
                }
            }
        } else {
            candidates.UnionWith(traitGetters);
            foreach (TraitFlags flag in Enum.GetValues(typeof(TraitFlags))) {
                if(flags.HasFlag(flag) && traitsByFlags.ContainsKey(flag)) {
                    candidates.IntersectWith(traitsByFlags[flag]);
                }
            }
        }
        if(candidates.Count == 0) {
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
