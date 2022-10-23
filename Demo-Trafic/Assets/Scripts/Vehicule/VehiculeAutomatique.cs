using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class VehiculeAutomatique : MonoBehaviour
{
    public PathFollower Suiveur { get; private set; }

    public float vitesseMaximale;

    private StateMachine<VehiculeAutomatique> iaVehicule;

    public bool PeutAvancer { get; set; }

    public float rayonRoue;

    public event System.Action<VehiculeAutomatique> DestructionVehicule;

    private Animator[] animateursRoue;

    protected void Awake()
    {
        iaVehicule = new StateMachine<VehiculeAutomatique>(this);
        Suiveur = GetComponent<PathFollower>();
        Suiveur.SetForward(Vector3.right);
        PeutAvancer = true;
        animateursRoue = GetComponentsInChildren<Animator>();
    }

    protected void Start()
    {
        iaVehicule.Start(new EtatAvancer());
    }

    public void SetVitesse(float vitesse)
    {
        foreach (Animator animateur in animateursRoue)
        {
            animateur.SetFloat("Vitesse", vitesse / (Mathf.PI * rayonRoue));
        }

        Suiveur.Speed = vitesse;
    }

    public void AffecterChemin(Path chemin)
    {
        Suiveur.SetPath(chemin);
    }
}