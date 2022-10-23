using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArretFeu : MonoBehaviour
{
    public FeuCirculation feu;

    private void OnTriggerEnter(Collider other)
    {
        if(!feu.PermetCirculation && other.TryGetComponent(out VehiculeAutomatique vehicule))
        {
            vehicule.PeutAvancer = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (feu.PermetCirculation && other.TryGetComponent(out VehiculeAutomatique vehicule))
        {
            vehicule.PeutAvancer = true;
        }
    }
}
