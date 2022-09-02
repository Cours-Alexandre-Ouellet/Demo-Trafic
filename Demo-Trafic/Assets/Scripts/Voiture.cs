using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Voiture qui se d�place dans la simulation
/// </summary>
public class Voiture : MonoBehaviour
{
    [Header("Param�tres de d�placement")]
    public float vitesse;                       // Vitesse � laquelle l'on avance
    public float vitesseMeilleureTauxVirage;    // Vitesse du meilleur taux de virage
    public float meilleurTauxVirage;            // Valeur (degr�s par secondes) du meilleur taux de virage

    private void Update()
    {
        float vitesseFrame = vitesse * Input.GetAxis("Vertical");

        transform.position += transform.right * vitesseFrame * Time.deltaTime;
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * VitesseAngulaire(vitesseFrame) * Time.deltaTime);
    }

    /// <summary>
    /// Calcule la vitesse angulaire � partir de la vitesse du v�hicule.
    /// </summary>
    /// <param name="vitesse">Vitesse du v�hicule.</param>
    /// <returns>La vitesse angulaire selon la vitesse instantan�e.</returns>
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
