using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Voiture qui d�place automatiquement vers une destination.
/// La voiture ne peut pas tourner.
/// </summary>
public class VoitureAutomatique : Voiture
{
    private const float DELTA_DISTANCE = 0.001f;     // Rayon autour de la destination pour arr�ter le mouvement 

    public Vector3 destination;                     // La destination
    private Vector3 direction;                      // Vecteur de direction normalis�
    private float distanceRestante;                 // Distance restant � parcourir

    private void Start()
    {
        // Assigne la destination souhait�e
        SetDestination(destination);
    }

    // Update is called once per frame
    private void Update()
    {
        // Distance parcourue durant le frame
        float vitesseFrame = vitesse * Time.deltaTime;

        // S'il reste de la distance � pacourir
        if(distanceRestante > DELTA_DISTANCE)
        {
            Vector3 deplacement = direction * vitesseFrame;
            float distanceParcourue = deplacement.magnitude;

            // On va trop loin, alors on t�l�porte � destination
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
    /// Assigne une nouvelle destination et effectue les calculs pour permettre le d�placement du v�hicule.
    /// </summary>
    /// <param name="destination">La destination � atteindre.</param>
    private void SetDestination(Vector3 destination)
    {
        Vector3 vecteurDeplacement = destination - transform.position;

        this.destination = destination;
        direction = vecteurDeplacement.normalized;
        distanceRestante = vecteurDeplacement.magnitude;
    }
}
