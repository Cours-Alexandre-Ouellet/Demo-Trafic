using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class Compteur : MonoBehaviour
{
    protected int nombre;
    private TextMeshProUGUI etiquette;
    public GenerateurVehicule generateur;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        nombre = 0;
        etiquette = GetComponent<TextMeshProUGUI>();
        generateur.CreerVehicule += OnVehiculeCree;
    }

    protected virtual void OnDestroy()
    {
        generateur.CreerVehicule -= OnVehiculeCree;
    }

    protected void AfficherNombre(int nombre)
    {
        etiquette.text = $"{nombre}";
    }

    protected abstract void OnVehiculeCree(VehiculeAutomatique voiture);
}
