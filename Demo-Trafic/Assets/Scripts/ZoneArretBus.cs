using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArretBus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AutobusAutomatique autobus))
        {
            autobus.ArretEmbarquement();
        }
    }
}
