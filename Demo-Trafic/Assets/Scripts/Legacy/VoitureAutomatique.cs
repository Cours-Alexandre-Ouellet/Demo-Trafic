using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Voiture qui déplace automatiquement vers une destination.
/// La voiture ne peut pas tourner.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class VoitureAutomatique : Voiture
{
    private const float DELTA_DISTANCE = 0.001f;     // Rayon autour de la destination pour arrêter le mouvement 
    private const float DELTA_HAUTEUR = 0.5f;

    public Vector3 destination;                     // La destination
    private Vector3 direction;                      // Vecteur de direction normalisé
    private float distanceRestante;                 // Distance restant à parcourir

    public event Action<VoitureAutomatique> DestructionVehicule;

    protected void Start()
    {
        // Assigne la destination souhaitée
        SetDestination(destination);
    }

    // Update is called once per frame
    private void Update()
    {
        Avancer();
    }

    protected void Avancer()
    {
        // Distance parcourue durant le frame
        float vitesseFrame = vitesse * Time.deltaTime;

        // S'il reste de la distance à pacourir
        if(distanceRestante > DELTA_DISTANCE && PeutAvancer())
        {
            Vector3 deplacement = direction * vitesseFrame;
            float distanceParcourue = deplacement.magnitude;

            // On va trop loin, alors on téléporte à destination
            if(distanceParcourue > distanceRestante)
            {
                transform.position = destination;
                distanceRestante = 0.0f;

                DestructionVehicule?.Invoke(this);
                Destroy(gameObject);
            }
            else            // Avancement normal
            {
                transform.position += deplacement;
                distanceRestante -= distanceParcourue;
            }
        }
    }

    protected virtual bool PeutAvancer()
    {
        const float ZONE_VERIF = 1.5f;
        Vector3 source = transform.position + transform.right * (GetComponent<MeshFilter>().mesh.bounds.extents.x + ZONE_VERIF);
        source.y = 0.5f;
        Vector3 etendue = GetComponent<MeshFilter>().mesh.bounds.extents;
        etendue.x = ZONE_VERIF * 0.33f;

        return !Physics.CheckBox(source, etendue, transform.rotation, LayerMask.GetMask("Voiture"));
    }

    /// <summary>
    /// Assigne une nouvelle destination et effectue les calculs pour permettre le déplacement du véhicule.
    /// </summary>
    /// <param name="destination">La destination à atteindre.</param>
    private void SetDestination(Vector3 destination)
    {
        Vector3 vecteurDeplacement = destination - transform.position;

        this.destination = destination;
        direction = vecteurDeplacement.normalized;
        distanceRestante = vecteurDeplacement.magnitude;
    }

    public void AffecterChemin(Chemin chemin)
    {
        transform.position = chemin.Depart;
        transform.position += Vector3.up * (DELTA_HAUTEUR + GetComponent<MeshFilter>().mesh.bounds.extents.y);

        SetDestination(chemin.Arrivee);

        // Ajustement en fonction de la direction
        transform.Rotate(0f, Vector3.SignedAngle(transform.right, direction, Vector3.up), 0f);

        
    }
}
