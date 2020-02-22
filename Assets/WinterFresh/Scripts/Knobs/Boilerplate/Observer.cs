using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer<T> : MonoBehaviour
{
    protected void Awake() 
    {
        RegisterWith(FindObjectOfType<Observable<T>>());
    }

    protected void RegisterWith(Observable<T> observable)
    {
        observable.Register(this);
    }

    protected void UnregisterWith(Observable<T> observable)
    {
        observable.Unregister(this);
    }

    public abstract void Observed(ref T val);
}
