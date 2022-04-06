using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [System.Serializable]
    public class Blackboard
    {
        public TeamsListSO TeamsList;
        
        private Dictionary<string, object> _dictionary;
        public Dictionary<string, object> Dico => _dictionary;

        public Blackboard()
        {
            _dictionary = new Dictionary<string, object>();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            SetValue("targetPos", null);
            // ...
        }

        public bool HasValue(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void SetValue(string key, object value)
        {
            if (_dictionary.ContainsKey(key))
                _dictionary[key] = value;
            else
                _dictionary.Add(key, value);
        }
        
        public T GetValue<T>(string key)
        {
            if (_dictionary.ContainsKey(key))
                return (T) _dictionary[key];
            
            return default;
        }
        
        public object GetValueAsObject(string key)
        {
            return GetValue<object>(key);
        }
    }
}
