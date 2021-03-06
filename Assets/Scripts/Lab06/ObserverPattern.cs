﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify(GameObject obj, NotificationType type);
}

public abstract class Subject : MonoBehaviour
{
    private List<Observer> observers = new List<Observer>();

    public void RegisterObserver(Observer observer)
    {
        if (observers.Contains(observer))
            return;

        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    public void ClearObservers()
    {
        observers.Clear();
    }

    public void Notify(GameObject obj, NotificationType type)
    {
        foreach(var observer in observers)
        {
            observer.OnNotify(obj, type);
        }
    }
}

public enum NotificationType { damaged, death, respawn}