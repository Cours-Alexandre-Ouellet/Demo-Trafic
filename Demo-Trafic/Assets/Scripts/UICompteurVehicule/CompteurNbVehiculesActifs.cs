using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurNbVehiculesActifs : Compteur
{
    protected override void OnVehiculeCree(VehiculeAutomatique voiture)
    {
        nombre++;
        AfficherNombre(nombre);

        voiture.DestructionVehicule += OnVehiculeDetruit;
    }

    void OnVehiculeDetruit(VehiculeAutomatique voiture)
    {
        nombre--;
        AfficherNombre(nombre);
        voiture.DestructionVehicule -= OnVehiculeDetruit;
    }
}
