using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observable<T> : MonoBehaviour
{
    private List<Observer<T>> observers = new List<Observer<T>>();

    public void Register(Observer<T> observer)
    {
        observers.Add(observer);
    }

    public void Unregister(Observer<T> observer)
    {
        observers.Remove(observer);
    }

    public void OnChange(ref T val)
    {
        foreach(Observer<T> observer in observers)
        {
            observer.Observed(ref val);
        }
    }
}
