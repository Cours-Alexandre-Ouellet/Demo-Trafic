using System.Collections;
using UnityEngine;
using TMPro;

public class InformationVehicule : MonoBehaviour
{
    public TextMeshProUGUI titreVehicule;
    public TextMeshProUGUI tempsVie;
    public TextMeshProUGUI nomObjet;

    private bool estOuvert;
    private VoitureAutomatique voitureAffichee;
    private Coroutine coroutineAffichage;

    private void Awake()
    {
        estOuvert = false;
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
        gameObject.SetActive(true);
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

        gameObject.SetActive(false);
        estOuvert = false;
    }
}
