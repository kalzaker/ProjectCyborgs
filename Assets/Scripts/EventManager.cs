using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent playerDied = new UnityEvent();

    public static UnityEvent weaponDropped = new UnityEvent();

    public static UnityEvent<Weapon> weaponPickedUp = new UnityEvent<Weapon>();

    public static UnityEvent<int> shotFired = new UnityEvent<int>();
}