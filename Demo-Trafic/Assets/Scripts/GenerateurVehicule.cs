using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateurVehicule : MonoBehaviour
{
    [Header("Affichage")]
    public TextMeshProUGUI compteurVehicules;
    public TextMeshProUGUI compteurAutobus;
    private int nombreVehiculesCrees;
    private int nombreAutobusCrees;

    [Header("Vehicule")]

    public VoitureAutomatique[] prototypes;

    [Header("Chemin")]
    public Chemin[] chemins;

    [Header("Paramètres génération")]
    public float chanceGeneration;                      // Probabilite de génération
    public float tempsAttente;                          // Nombre de frames d'attente entre deux générations

    private float tempsDepuisDerniereGeneration;        // Nombre de frames depuis la dernière génération

    public delegate void EvenementCreation(VoitureAutomatique voiture);
    public event Action<VoitureAutomatique> CreerVehicule;

    private void Awake()
    {
        tempsDepuisDerniereGeneration = 0;
        nombreVehiculesCrees = 0;
        nombreAutobusCrees = 0;
    }

    private void Update()
    {
        if(tempsDepuisDerniereGeneration > tempsAttente)
        {
            if(UnityEngine.Random.value < chanceGeneration)
            {
                GenererVehicule();
                tempsDepuisDerniereGeneration = 0;
            }
        }
        tempsDepuisDerniereGeneration += Time.deltaTime;
    }

    private void GenererVehicule()
    {
        VoitureAutomatique protoypeChoisi = prototypes[UnityEngine.Random.Range(0, prototypes.Length)];
        Chemin cheminChoisi = chemins[UnityEngine.Random.Range(0, chemins.Length)];

        if(PeutGenerer(protoypeChoisi.GetComponent<MeshFilter>().sharedMesh.bounds.extents, cheminChoisi.Depart, 
            (cheminChoisi.Arrivee - cheminChoisi.Depart).normalized))
        {            
            VoitureAutomatique generee = Instantiate(protoypeChoisi, transform);
            generee.AffecterChemin(cheminChoisi);

            CreerVehicule?.Invoke(generee);
        }
    }

    private bool PeutGenerer(Vector3 extendsLibre, Vector3 position, Vector3 direction)
    {
        return !Physics.CheckBox(position + Vector3.up * (extendsLibre.y + 0.1f), extendsLibre, 
            Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.right, direction, Vector3.up), 0f),
            LayerMask.GetMask("Voiture"));
    }
}
