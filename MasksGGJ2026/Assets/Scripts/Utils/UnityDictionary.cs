using System.Collections.Generic;
using UnityEngine;

namespace Utils
{

    [System.Serializable]
    public class UnityDictionary<T, Y>
    {
        public List<DictionaryValue<T, Y>> values = new();

        public Y GetValue(T key)
        {
            DictionaryValue<T, Y> value = values.Find(i => i.key.Equals(key));
            if(value != null)
            {
                return value.value;
            }
            else
            {
                // Debug.LogWarning($"This attribute \"{key}\" does not exist.");
                return default;
            }
        }

        public void UpdateValue(T key, Y value)
        {
            int index = values.FindIndex(i => i.key.Equals(key));
            if(index >= 0)
            {
                values[index].value = value;
            }
            else
            {
                values.Add(new(key, value));
            }
        }

        public bool Contains(T key)
        {
            return values.FindIndex(i => i.value.Equals(key)) >= 0;
        }
    }

    [System.Serializable]
    public class DictionaryValue<T, Y>
    {
        public DictionaryValue(T key, Y value)
        {
            this.key = key;
            this.value = value;
        }

        public T key;
        public Y value;
    }
}
