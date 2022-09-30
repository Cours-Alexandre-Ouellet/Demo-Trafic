using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurVehicule : MonoBehaviour
{
    public VoitureAutomatique[] prototypes;

    [Header("Chemin")]
    public Chemin[] chemins;

    [Header("Paramètres génération")]
    public float chanceGeneration;                      // Probabilite de génération
    public float tempsAttente;                          // Nombre de frames d'attente entre deux générations

    private float tempsDepuisDerniereGeneration;        // Nombre de frames depuis la dernière génération

    private void Awake()
    {
        tempsDepuisDerniereGeneration = 0;
    }

    private void Update()
    {
        if(tempsDepuisDerniereGeneration > tempsAttente)
        {
            if(Random.value < chanceGeneration)
            {
                GenererVehicule();
                tempsDepuisDerniereGeneration = 0;
            }
        }
        tempsDepuisDerniereGeneration += Time.deltaTime;
    }

    private void GenererVehicule()
    {
        VoitureAutomatique protoypeChoisi = prototypes[Random.Range(0, prototypes.Length)];
        Chemin cheminChoisi = chemins[Random.Range(0, chemins.Length)];

        if(PeutGenerer(protoypeChoisi.GetComponent<MeshFilter>().sharedMesh.bounds.extents, cheminChoisi.Depart, 
            (cheminChoisi.Arrivee - cheminChoisi.Depart).normalized))
        {
            VoitureAutomatique generee = Instantiate(protoypeChoisi, transform);
            generee.AffecterChemin(cheminChoisi);
        }
    }

    private bool PeutGenerer(Vector3 extendsLibre, Vector3 position, Vector3 direction)
    {
        pos = position + Vector3.up * (extendsLibre.y + 0.1f);
        ext = extendsLibre;
        angle = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.right, direction, Vector3.up), 0f);

        return !Physics.CheckBox(position + Vector3.up * (extendsLibre.y + 0.1f), extendsLibre, 
            Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.right, direction, Vector3.up), 0f),
            LayerMask.GetMask("Voiture"));
    }

    private Vector3 ext, pos;
    private Quaternion angle;

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(pos, angle, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, ext * 2f);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
