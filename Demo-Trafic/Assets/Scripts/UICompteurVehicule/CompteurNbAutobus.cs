using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompteurNbAutobus : Compteur
{
    protected override void OnVehiculeCree(VoitureAutomatique voiture)
    {
        if(voiture.GetType() == typeof(Autobus))
        {
            nombre++;
            AfficherNombre(nombre);
        }
    }
}
