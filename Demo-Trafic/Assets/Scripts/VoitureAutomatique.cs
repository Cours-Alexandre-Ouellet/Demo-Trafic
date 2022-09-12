using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Voiture qui déplace automatiquement vers une destination.
/// La voiture ne peut pas tourner.
/// </summary>
public class VoitureAutomatique : Voiture
{
    private const float DELTA_DISTANCE = 0.001f;     // Rayon autour de la destination pour arrêter le mouvement 

    public Vector3 destination;                     // La destination
    private Vector3 direction;                      // Vecteur de direction normalisé
    private float distanceRestante;                 // Distance restant à parcourir

    private void Start()
    {
        // Assigne la destination souhaitée
        SetDestination(destination);
    }

    // Update is called once per frame
    private void Update()
    {
        // Distance parcourue durant le frame
        float vitesseFrame = vitesse * Time.deltaTime;

        // S'il reste de la distance à pacourir
        if(distanceRestante > DELTA_DISTANCE)
        {
            Vector3 deplacement = direction * vitesseFrame;
            float distanceParcourue = deplacement.magnitude;

            // On va trop loin, alors on téléporte à destination
            if(distanceParcourue > distanceRestante)
            {
                transform.position = destination;
                distanceRestante = 0.0f;
            }
            else            // Avancement normal
            {
                transform.position += deplacement;
                distanceRestante -= distanceParcourue;
            }
        }

        
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
}
