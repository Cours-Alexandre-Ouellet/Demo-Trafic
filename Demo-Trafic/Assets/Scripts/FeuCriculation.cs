using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuCriculation : MonoBehaviour
{
    private const int NOMBRE_ETATS = 3;
    private const string PROPRIETE_EMISSION = "_EmissionColor";

    public float[] dureesFeux;
    public float intensite;
    public MeshRenderer[] lumieres;

    // 0 => Vert // 1 => Jaune // 2 => Rouge
    private int etatFeu;
    private bool changerEtat;

    private void Awake()
    {
        changerEtat = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        etatFeu = 0;
        SetEtat(etatFeu);
    }

    private void Update()
    {
        if(changerEtat)
        {
            SetEtat((etatFeu + 1) % NOMBRE_ETATS);
        }
    }

    private void SetEtat(int etatFeu)
    {
        changerEtat = false;
        this.etatFeu = etatFeu;
        foreach(MeshRenderer lumiere in lumieres)
        {
            lumiere.material.SetColor(PROPRIETE_EMISSION, Color.black);
        }

        lumieres[etatFeu].material.SetColor(PROPRIETE_EMISSION, 
                lumieres[etatFeu].material.color * intensite);

        StartCoroutine(AttenteLumiere(dureesFeux[etatFeu]));
    }

    private IEnumerator AttenteLumiere(float tempsAttente)
    {
        yield return new WaitForSeconds(tempsAttente);
        changerEtat = true;
    }
}
