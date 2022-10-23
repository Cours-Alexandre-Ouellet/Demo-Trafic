using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class VehiculeAutomatique : MonoBehaviour
{
    public PathFollower Suiveur { get; private set; }

    public float vitesseMaximale;

    private StateMachine<VehiculeAutomatique> iaVehicule;

    public bool PeutAvancer { get; set; }

    public event System.Action<VehiculeAutomatique> DestructionVehicule;

    protected void Awake()
    {
        iaVehicule = new StateMachine<VehiculeAutomatique>(this);
        Suiveur = GetComponent<PathFollower>();
        Suiveur.SetForward(Vector3.right);
        PeutAvancer = true;
    }

    protected void Start()
    {
        iaVehicule.Start(new EtatAvancer());
    }

    public void SetVitesse(float vitesse)
    {
        Suiveur.Speed = vitesse;
    }

    public void AffecterChemin(Path chemin)
    {
        Suiveur.SetPath(chemin);
    }
}