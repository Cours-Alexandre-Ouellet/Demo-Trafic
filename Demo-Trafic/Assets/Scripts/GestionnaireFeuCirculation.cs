using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionnaireFeuCirculation : MonoBehaviour
{
    private const int NOMBRE_ETATS = 3;
    public float[] dureesFeux;

    private FeuCirculation[] feux;
    private bool[] changerEtat;

    // 0 => Vert // 1 => Jaune // 2 => Rouge
    // Position 0 => etat des pairs -- position 1 => etat des impairs
    private int[] etatFeux;

    private void Start()
    {
        feux = transform.GetComponentsInChildren<FeuCirculation>();
        changerEtat = new bool[2];
        etatFeux = new int[2] { 2, 1 };

        ChangerEtat(0);
        ChangerEtat(1);
    }

    private IEnumerator AttenteLumiere(float tempsAttente, int parite)
    {
        yield return new WaitForSeconds(tempsAttente);
        changerEtat[parite] = true;
    }

     // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < changerEtat.Length; i++)
        {
            if(changerEtat[i])
            {
                ChangerEtat(i);
            }
        }
    }

    private void ChangerEtat(int parite)
    {
        changerEtat[parite] = false;
        int nouvelEtat = (etatFeux[parite] + 1) % NOMBRE_ETATS;
        etatFeux[parite] = nouvelEtat;

        for(int i = parite; i < feux.Length; i+=2)
        {
            feux[i].SetEtat(nouvelEtat);
        }

        StartCoroutine(AttenteLumiere(dureesFeux[nouvelEtat], parite));
    }
}
