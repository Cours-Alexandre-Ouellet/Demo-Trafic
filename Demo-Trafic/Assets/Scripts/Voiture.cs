using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Voiture qui se déplace dans la simulation
/// </summary>
public class Voiture : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float vitesse;                       // Vitesse à laquelle l'on avance
    public float vitesseMeilleureTauxVirage;    // Vitesse du meilleur taux de virage
    public float meilleurTauxVirage;            // Valeur (degrés par secondes) du meilleur taux de virage

    private void Update()
    {
        float vitesseFrame = vitesse * Input.GetAxis("Vertical");

        transform.position += transform.right * vitesseFrame * Time.deltaTime;
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * VitesseAngulaire(vitesseFrame) * Time.deltaTime);
    }

    /// <summary>
    /// Calcule la vitesse angulaire à partir de la vitesse du véhicule.
    /// </summary>
    /// <param name="vitesse">Vitesse du véhicule.</param>
    /// <returns>La vitesse angulaire selon la vitesse instantanée.</returns>
    private float VitesseAngulaire(float vitesse)
    {
        float facteurDirection = vitesse < 0.0f ? -1.0f : 1.0f;
        float vitesseAbsolue = vitesse * facteurDirection;

        // Formule qui respecte la condition vitesse = 0 => vitesseAngulaire = 0
        float vitesseAngulaire = (-1.0f * meilleurTauxVirage / (vitesseMeilleureTauxVirage * vitesseMeilleureTauxVirage)) *
                    (vitesse - vitesseMeilleureTauxVirage) * (vitesseAbsolue - vitesseMeilleureTauxVirage) + meilleurTauxVirage;

        return vitesseAngulaire * facteurDirection;
    }
}
