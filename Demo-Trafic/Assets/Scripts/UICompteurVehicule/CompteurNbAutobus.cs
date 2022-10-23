using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompteurNbAutobus : Compteur
{
    protected override void OnVehiculeCree(VehiculeAutomatique voiture)
    {
        if(voiture.GetType() == typeof(AutobusAutomatique))
        {
            nombre++;
            AfficherNombre(nombre);
        }
    }
}
