using System;
using System.Collections;
using System.Collections.Generic;

public class MyList<T> : IEnumerable<T>
{
    private T[] items;
    private int count;

    public MyList()
    {
        items = new T[4]; // Initial capacity
        count = 0;
    }

    public int Count => count;

    public void Add(T item)
    {
        EnsureCapacity();
        items[count] = item;
        count++;
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= count)
            return false;

        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        count--;
        return true;
    }

    public bool Remove(T item)
    {
        int index = Array.IndexOf(items, item, 0, count);
        if (index >= 0)
        {
            return RemoveAt(index);
        }
        return false;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            return items[index];
        }
        set
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            items[index] = value;
        }
    }

    private void EnsureCapacity()
    {
        if (count == items.Length)
        {
            int newCapacity = items.Length * 2;
            Array.Resize(ref items, newCapacity);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            yield return items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
