using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurNbVehiculesActifs : Compteur
{
    protected override void OnVehiculeCree(VoitureAutomatique voiture)
    {
        nombre++;
        AfficherNombre(nombre);

        voiture.DestructionVehicule += OnVehiculeDetruit;
    }

    void OnVehiculeDetruit(VoitureAutomatique voiture)
    {
        nombre--;
        AfficherNombre(nombre);
        voiture.DestructionVehicule -= OnVehiculeDetruit;
    }
}
