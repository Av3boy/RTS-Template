using Assets.Scripts.Enums;

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<KeyValuePair<ActorType, GameObject>> ToActorKeyValuePairs(this Object[] objects, ActorType actorType)
            => objects.Select(obj => new KeyValuePair<ActorType, GameObject>(actorType, obj.GameObject()));

        public static Dictionary<TKey, TValue> KeyValuePairsToDictionary<TKey, TValue>(params IEnumerable<KeyValuePair<TKey, TValue>>[] keyValuePairs)
        {
            var result = keyValuePairs.Aggregate((dict1, dict2) => dict1.Concat(dict2));
            return result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
