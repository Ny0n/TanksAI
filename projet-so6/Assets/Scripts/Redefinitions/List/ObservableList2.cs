using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class ObservableList2<T> : List<T>
{
    public event System.Action ValueChanged; // observable event

    public void NotifyChange()
    {
        Debug.Log("NotifyChange");
        ValueChanged?.Invoke();
    }

    public new T this[int index] {
        get => base[index];
        set
        {
            base[index] = value;
            ValueChanged?.Invoke();
        }
    }

    public new void Add([CanBeNull] T item)
    {
        base.Add(item);
        ValueChanged?.Invoke();
    }
    
    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        ValueChanged?.Invoke();
    }

    public new void Clear()
    {
        base.Clear();
        ValueChanged?.Invoke();
    }
    
    public new void Insert(int index, [CanBeNull] T item)
    {
        base.Insert(index, item);
        ValueChanged?.Invoke();
    }
    
    public new void InsertRange(int index, IEnumerable<T> collection)
    {
        base.InsertRange(index, collection);
        ValueChanged?.Invoke();
    }

    public new bool Remove([CanBeNull] T item)
    {
        bool result = base.Remove(item);
        ValueChanged?.Invoke();
        return result;
    }

    public new int RemoveAll(Predicate<T> match)
    {
        int result = base.RemoveAll(match);
        ValueChanged?.Invoke();
        return result;
    }

    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);
        ValueChanged?.Invoke();
    }

    public new void RemoveRange(int index, int count)
    {
        base.RemoveRange(index, count);
        ValueChanged?.Invoke();
    }

    public new void Reverse()
    {
        base.Reverse();
        ValueChanged?.Invoke();
    }

    public new void Reverse(int index, int count)
    {
        base.Reverse(index, count);
        ValueChanged?.Invoke();
    }

    public new void Sort()
    {
        base.Sort();
        ValueChanged?.Invoke();
    }

    public new void Sort(IComparer<T> comparer)
    {
        base.Sort(comparer);
        ValueChanged?.Invoke();
    }

    public new void Sort(int index, int count, IComparer<T> comparer)
    {
        base.Sort(index, count, comparer);
        ValueChanged?.Invoke();
    }

    public new void Sort(Comparison<T> comparison)
    {
        base.Sort(comparison);
        ValueChanged?.Invoke();
    }

    public new void TrimExcess()
    {
        base.TrimExcess();
        ValueChanged?.Invoke();
    }
}
