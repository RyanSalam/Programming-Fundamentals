using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObserver : Observer
{
    public override void OnNotify(GameObject obj, NotificationType type)
    {
        switch (type)
        {
            case NotificationType.damaged:

                GetComponent<Goblin>().TargetPlayer();

                break;

            case NotificationType.death:

                obj.GetComponent<Subject>().ClearObservers();

                foreach(var subject in FindObjectsOfType<Subject>())
                {
                    subject.RemoveObserver(this);
                }

                break;

            case NotificationType.respawn:              

                foreach (var subject in FindObjectsOfType<Subject>())
                { 
                    subject.RegisterObserver(this);
                }

                Debug.Log("check");

                break;
        }
    }
}
