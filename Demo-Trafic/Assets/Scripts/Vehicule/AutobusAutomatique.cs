using System.Collections;
using UnityEngine;

public class AutobusAutomatique : VehiculeAutomatique
{
    public float tempsArretMinimal;
    public float tempsArretMaximal;

    public void ArretEmbarquement()
    {
        StartCoroutine(Attendre(Random.Range(tempsArretMinimal, tempsArretMaximal)));
    }

    private IEnumerator Attendre(float temps)
    {
        yield return new WaitForSeconds(temps);
    }
}

