using System.Collections;
using UnityEngine;
using TMPro;

public class InformationVehicule : MonoBehaviour
{
    public TextMeshPro titreVehicule;
    public TextMeshPro tempsVie;
    public TextMeshPro nomObjet;

    private bool estOuvert;
    private VoitureAutomatique voitureAffichee;
    private Coroutine coroutineAffichage;

    private void Awake()
    {
        estOuvert = false;
    }

    public void AfficherVehicule(VoitureAutomatique voiture)
    {
        voitureAffichee = voiture;
        if(coroutineAffichage is not null)
        {
            StopCoroutine(coroutineAffichage);
        }

        titreVehicule.text = voiture.NomType;
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
        return string.Format("{0.##}", Time.time - voitureAffichee.TempsCreation);
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
