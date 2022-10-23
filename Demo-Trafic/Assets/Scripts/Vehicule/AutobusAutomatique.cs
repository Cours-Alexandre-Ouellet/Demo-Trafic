using System.Collections;
using UnityEngine;

public class AutobusAutomatique : VehiculeAutomatique
{
    public float tempsArretMinimal;
    public float tempsArretMaximal;

    public override string NomType => "Autobus";

    public void ArretEmbarquement()
    {
        PeutAvancer = false;
        StartCoroutine(TempsAttente(Random.Range(tempsArretMinimal, tempsArretMaximal)));
    }

    private IEnumerator TempsAttente(float tempsAttente)
    {
        yield return new WaitForSeconds(tempsAttente);
        PeutAvancer = true;
    }
}

