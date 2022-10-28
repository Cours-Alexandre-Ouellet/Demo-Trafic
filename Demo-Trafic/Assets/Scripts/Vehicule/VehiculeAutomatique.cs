using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PathFollower))]
public class VehiculeAutomatique : MonoBehaviour, IPointerClickHandler
{
    public PathFollower Suiveur { get; private set; }

    public float vitesseMaximale;

    public bool ArretBus { get; private set; }

    private StateMachine<VehiculeAutomatique> iaVehicule;

    public bool PeutAvancer { get; set; }

    public float rayonRoue;

    public event System.Action<VehiculeAutomatique> DestructionVehicule;

    private Animator[] animateursRoue;

    public virtual string NomType { get; private set; }

    public float TempsCreation { get; private set; }

    public float TempsAttente { get; set; }

    public float TempsImpatience { get; private set; }

    protected void Awake()
    {
        iaVehicule = new StateMachine<VehiculeAutomatique>(this);
        Suiveur = GetComponent<PathFollower>();
        Suiveur.SetForward(Vector3.right);
        PeutAvancer = true;
        animateursRoue = GetComponentsInChildren<Animator>();
        TempsCreation = Time.time;
        TempsImpatience = 3.0f;
        TempsAttente = 0.0f;
    }

    public void Initialiser(ModeleVehicule modele)
    {
        Material[] mat = GetComponent<MeshRenderer>().materials;
        mat[0] = modele.exterieurVoiture;
        GetComponent<MeshRenderer>().materials = mat;  
       
        vitesseMaximale = modele.vitesseMaximale;
        NomType = modele.nomVoiture;
    }

    protected void Start()
    {
        iaVehicule.Start(new EtatAvancer());
    }

    void Update()
    {
        if(TempsAttente > TempsImpatience)
        {
            Klaxonner();
            TempsAttente = 0.0f;
        }
    }

    private void Klaxonner()
    {

    }


    public void SetVitesse(float vitesse)
    {
        foreach(Animator animateur in animateursRoue)
        {
            animateur.SetFloat("Vitesse", vitesse / (Mathf.PI * rayonRoue));
        }

        Suiveur.Speed = vitesse;
    }

    public void AffecterChemin(Path chemin)
    {
        Suiveur.SetPath(chemin);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InformationVehicule.Instance.AfficherVehicule(this);
    }

    public void OnDrawGizmos()
    {
        // Dessine la boite de vérification
        Vector3 source = transform.position + -1.0f * transform.right * (GetComponent<MeshFilter>().sharedMesh.bounds.extents.x + 0.75f);
        source.y = GetComponent<MeshFilter>().sharedMesh.bounds.extents.y;

        Vector3 etendue = GetComponent<MeshFilter>().sharedMesh.bounds.extents;
        etendue.x = 0.75f * 0.66f;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(source, etendue * 2);
        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);


        // if (Physics.CheckBox(source, etendue, vehicule.transform.rotation, LayerMask.GetMask("Voiture")))
    }
}