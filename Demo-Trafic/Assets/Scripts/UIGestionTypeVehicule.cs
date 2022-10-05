using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIGestionTypeVehicule : MonoBehaviour
{
    public VoitureAutomatique prototype;
    public GenerateurVehicule generateur;

    private void Start()
    {
        PermettreType(GetComponent<Toggle>().isOn);
    }

    public void PermettreType(bool permis)
    {
        if(permis)
        {
            generateur.Prototypes.Add(prototype);
        }
        else
        {
            if(generateur.Prototypes.Contains(prototype))
            {
                generateur.Prototypes.Remove(prototype);
            }
        }
    }
}
