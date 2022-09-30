using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurNbVehicules : Compteur
{
    protected override void OnVehiculeCree(VoitureAutomatique voiture)
    {
        nombre++;
        AfficherNombre(nombre);
    }
}
