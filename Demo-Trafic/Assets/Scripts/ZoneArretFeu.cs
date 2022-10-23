using UnityEngine;

public class ZoneArretFeu : MonoBehaviour
{
    public FeuCirculation feu;

    public Vector3 direction;               // Direction dans laquelle avancer pour être soumis à l'arrêt

    private void OnTriggerEnter(Collider other)
    {
        if (!feu.PermetCirculation && other.TryGetComponent(out VehiculeAutomatique vehicule))
        {
            vehicule.PeutAvancer = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (feu.PermetCirculation && other.TryGetComponent(out VehiculeAutomatique vehicule))
        {
            vehicule.PeutAvancer = true;
        }
    }
}
