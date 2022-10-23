using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class InformationVehicule : MonoBehaviour
{
    public TextMeshProUGUI titreVehicule;
    public TextMeshProUGUI tempsVie;
    public TextMeshProUGUI nomObjet;

    private bool estOuvert;
    private VehiculeAutomatique voitureAffichee;
    private Coroutine coroutineAffichage;

    private Animator animator;

    public static InformationVehicule Instance { get; private set; }

    private void Awake()
    {
        if(Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        estOuvert = false;
        animator = GetComponent<Animator>();
    }

    public void AfficherVehicule(VehiculeAutomatique vehicule)
    {
        if(!estOuvert)
        {
            Ouvrir();
        }

        voitureAffichee = vehicule;
        if(coroutineAffichage is not null)
        {
            StopCoroutine(coroutineAffichage);
        }

        titreVehicule.text = vehicule.NomType;
        nomObjet.text = vehicule.gameObject.name;
        
        coroutineAffichage = StartCoroutine(MettreAJourTempsVie());
        vehicule.DestructionVehicule += OnVehiculeDestruction;
    }

    private void OnVehiculeDestruction(VehiculeAutomatique voiture)
    {
        if(coroutineAffichage is not null)
        {
            StopCoroutine(coroutineAffichage);
            coroutineAffichage = null;
        }
        voiture.DestructionVehicule -= OnVehiculeDestruction;
    }

    private IEnumerator MettreAJourTempsVie()
    {
        while(true)
        {
            tempsVie.text = CalculerTempsVie();
            yield return new WaitForSeconds(0.25f);
        }

    }

    private string CalculerTempsVie()
    {
        return (Time.time - voitureAffichee.TempsCreation).ToString("0.00");
    }

    public void Ouvrir()
    {
        animator.SetTrigger("Ouvrir");
        estOuvert = true;
    }

    public void Fermer()
    {
        voitureAffichee = null;
        if(coroutineAffichage is not null)
        {
            StopCoroutine(coroutineAffichage);
            coroutineAffichage = null;
        }

        animator.SetTrigger("Fermer");
        estOuvert = false;
    }
}
