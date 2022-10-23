using System;
using UnityEngine;


public class ZoneArret : MonoBehaviour
{
    public event Action<VehiculeAutomatique> OnArriveeVehicule;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out VehiculeAutomatique vehicule))
        {
            OnArriveeVehicule?.Invoke(vehicule);
        }
    }
}
