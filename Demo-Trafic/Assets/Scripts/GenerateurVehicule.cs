using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateurVehicule : MonoBehaviour
{
    public static GenerateurVehicule Instance { get; private set; }

    [Header("Affichage")]
    public TextMeshProUGUI compteurVehicules;
    public TextMeshProUGUI compteurAutobus;
    private int nombreVehiculesCrees;
    private int nombreAutobusCrees;

    public List<VehiculeAutomatique> Prototypes { get; private set; }

    [Header("Chemin")]
    public GameObject conteneurChemins;                 // Objet contenant tous les chemins possibles
    private List<SegmentRoute> segmentsRouteInitiaux;   // Segments de route dont une des extrémités est disponible

    [Header("Paramètres génération")]
    public float chanceGeneration;                      // Probabilite de génération
    public float tempsAttente;                          // Nombre de frames d'attente entre deux générations

    private float tempsDepuisDerniereGeneration;        // Nombre de frames depuis la dernière génération

    public delegate void EvenementCreation(VehiculeAutomatique voiture);
    public event Action<VehiculeAutomatique> CreerVehicule;

    private ModeleVehicule[] modeles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tempsDepuisDerniereGeneration = 0;
        nombreVehiculesCrees = 0;
        nombreAutobusCrees = 0;
        Prototypes = new List<VehiculeAutomatique>();
        segmentsRouteInitiaux = new List<SegmentRoute>();
        modeles = Resources.LoadAll<ModeleVehicule>("DonneesVehicule");
    }

    private void Start()
    {
        foreach (SegmentRoute segmentRoute in conteneurChemins.GetComponentsInChildren<SegmentRoute>())
        {
            if (!segmentRoute.ConnecteDeuxExtremites)
            {
                segmentsRouteInitiaux.Add(segmentRoute);
            }
        }
    }

    private void Update()
    {
        if (tempsDepuisDerniereGeneration > tempsAttente)
        {
            if (UnityEngine.Random.value < chanceGeneration)
            {
                GenererVehicule();
                tempsDepuisDerniereGeneration = 0;
            }
        }
        tempsDepuisDerniereGeneration += Time.deltaTime;
    }

    public void GenererVehicule()
    {
        if (Prototypes.Count == 0)
            return;

        VehiculeAutomatique protoypeChoisi = Prototypes[UnityEngine.Random.Range(0, Prototypes.Count)];
        ConstructeurChemin cnstrChemin = new ConstructeurChemin();
        Path chemin = cnstrChemin.ConstruireChemin(segmentsRouteInitiaux[UnityEngine.Random.Range(0, segmentsRouteInitiaux.Count)]);

        if (PeutGenerer(protoypeChoisi.GetComponent<MeshFilter>().sharedMesh.bounds.extents, chemin.Start,
            (Vector3.one).normalized))
        {
            VehiculeAutomatique generee = Instantiate(protoypeChoisi, transform);
            generee.Initialiser(modeles[UnityEngine.Random.Range(0, modeles.Length)]);
            generee.AffecterChemin(chemin);

            CreerVehicule?.Invoke(generee);
        }
    }

    public void SetChanceGeneration(float chance)
    {
        chanceGeneration = chance;
    }

    public void SetTempsAttente(float tempsAttente)
    {
        this.tempsAttente = tempsAttente;
    }

    private bool PeutGenerer(Vector3 extendsLibre, Vector3 position, Vector3 direction)
    {
        return !Physics.CheckBox(position + Vector3.up * (extendsLibre.y + 0.1f), extendsLibre,
            Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.right, direction, Vector3.up), 0f),
            LayerMask.GetMask("Voiture"));
    }
}
