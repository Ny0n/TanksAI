using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class ObservableList<T>
{
    [SerializeField] private List<T> _list;

    public ObservableList()
    {
        _list = new List<T>();
    }

    public void CopyTo(Array array, int index)
    {
    }

    public int Count => _list.Count;
    public bool IsSynchronized { get; }
    public object SyncRoot { get; }
    public bool IsReadOnly => false;

    public event Action ValueChanged; // observable event

    public void NotifyChange() => ValueChanged?.Invoke();

    public T this[int index] {
        get => _list[index];
        set
        {
            _list[index] = value;
            ValueChanged?.Invoke();
        }
    }

    public void Add([CanBeNull] T item)
    {
        _list.Add(item);
        ValueChanged?.Invoke();
    }
    
    public void AddRange(IEnumerable<T> collection)
    {
        _list.AddRange(collection);
        ValueChanged?.Invoke();
    }

    public int Add(object value)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        _list.Clear();
        ValueChanged?.Invoke();
    }

    public bool Contains(object value)
    {
        throw new NotImplementedException();
    }

    public int IndexOf(object value)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, object value)
    {
        throw new NotImplementedException();
    }

    public void Remove(object value)
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        ValueChanged?.Invoke();
    }
    
    public void InsertRange(int index, IEnumerable<T> collection)
    {
        _list.InsertRange(index, collection);
        ValueChanged?.Invoke();
    }

    public bool Remove([CanBeNull] T item)
    {
        bool result = _list.Remove(item);
        ValueChanged?.Invoke();
        return result;
    }

    public int RemoveAll(Predicate<T> match)
    {
        int result = _list.RemoveAll(match);
        ValueChanged?.Invoke();
        return result;
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        ValueChanged?.Invoke();
    }

    public bool IsFixedSize { get; }

    public void RemoveRange(int index, int count)
    {
        _list.RemoveRange(index, count);
        ValueChanged?.Invoke();
    }

    public void Reverse()
    {
        _list.Reverse();
        ValueChanged?.Invoke();
    }

    public void Reverse(int index, int count)
    {
        _list.Reverse(index, count);
        ValueChanged?.Invoke();
    }

    public void Sort()
    {
        _list.Sort();
        ValueChanged?.Invoke();
    }

    public void Sort(IComparer<T> comparer)
    {
        _list.Sort(comparer);
        ValueChanged?.Invoke();
    }

    public void Sort(int index, int count, IComparer<T> comparer)
    {
        _list.Sort(index, count, comparer);
        ValueChanged?.Invoke();
    }

    public void Sort(Comparison<T> comparison)
    {
        _list.Sort(comparison);
        ValueChanged?.Invoke();
    }

    public void TrimExcess()
    {
        _list.TrimExcess();
        ValueChanged?.Invoke();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
