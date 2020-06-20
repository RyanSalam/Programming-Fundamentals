using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObserver : Observer
{
    private void Start()
    {
        foreach(var subject in FindObjectsOfType<Subject>())
        {
            subject.RegisterObserver(this);
        }
    }

    public override void OnNotify(GameObject obj, NotificationType type)
    {
        switch (type)
        {
            case NotificationType.damaged:

                foreach (var goblin in FindObjectsOfType<Goblin>())
                {
                    Debug.Log("notify");
                    goblin.TargetPlayer();
                }

                break;
        }
    }
}
