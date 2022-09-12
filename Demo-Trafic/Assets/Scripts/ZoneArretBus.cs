using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArretBus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Autobus autobus = other.GetComponent<Autobus>();

        if(autobus is not null)
        {
            autobus.EntrerZoneArret(this);
        }
    }
}
