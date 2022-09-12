using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArret : MonoBehaviour
{
    private bool aArrete;

    private void OnTriggerEnter(Collider other)
    {
        aArrete = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(aArrete is false)
        {
            Debug.Log($"La voiture { other.gameObject.transform.name } a brulé l'arrêt { gameObject.transform.parent.name }");
        }
        else
        {
            Debug.Log($"La voiture { other.gameObject.transform.name } s'est correctement arrêtée à l'arrêt { gameObject.transform.parent.name }.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Voiture voiture = other.GetComponent<Voiture>();

        if(voiture is not null)
        {
            if(Mathf.Approximately(voiture.GetComponent<Rigidbody>().velocity.sqrMagnitude, 0.0f))
            {
                aArrete = true;
            }
        }
    }
}
