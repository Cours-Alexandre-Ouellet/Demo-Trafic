using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestionnaireArret : MonoBehaviour
{
    public ZoneArret[] arrets;

    private const float TEMPS_ENTRE_PASSAGE = 3f;

    private Stack<VehiculeAutomatique> vehiculesAttente;

    private bool executionCoroutine;

    private void Awake()
    {
        vehiculesAttente = new Stack<VehiculeAutomatique>();
        executionCoroutine = false;
    }

    private void Start()
    {
        foreach (ZoneArret zone in arrets)
        {
            zone.OnArriveeVehicule += AjouterVehicule;
        }
    }

    private void AjouterVehicule(VehiculeAutomatique vehicule)
    {
        vehiculesAttente.Push(vehicule);
        vehicule.PeutAvancer = false;

        if(!executionCoroutine)
        {
            StartCoroutine(LibererArret());
        }
    }

    private IEnumerator LibererArret()
    {
        executionCoroutine = true;

        while(vehiculesAttente.Count > 0)
        {
            yield return new WaitForSeconds(TEMPS_ENTRE_PASSAGE);
            vehiculesAttente.Pop().PeutAvancer = true;
        }

        executionCoroutine = false;
    }
}
