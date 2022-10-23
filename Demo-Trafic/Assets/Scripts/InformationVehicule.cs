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
    private VoitureAutomatique voitureAffichee;
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

    public void AfficherVehicule(VoitureAutomatique voiture)
    {
        if(!estOuvert)
        {
            Ouvrir();
        }

        voitureAffichee = voiture;
        if(coroutineAffichage is not null)
        {
            StopCoroutine(coroutineAffichage);
        }

        titreVehicule.text = voiture.NomType;
        nomObjet.text = voiture.gameObject.name;
        
        coroutineAffichage = StartCoroutine(MettreAJourTempsVie());
        voiture.DestructionVehicule += OnVehiculeDestruction;
    }

    private void OnVehiculeDestruction(VoitureAutomatique voiture)
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
