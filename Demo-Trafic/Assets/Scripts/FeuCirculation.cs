using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuCirculation : MonoBehaviour
{
    private const string PROPRIETE_EMISSION = "_EmissionColor";

    public float intensite;
    public MeshRenderer[] lumieres;   

    public bool PermetCirculation { get; private set; }

    public void SetEtat(int etatFeu)
    {
        foreach(MeshRenderer lumiere in lumieres)
        {
            lumiere.material.SetColor(PROPRIETE_EMISSION, Color.black);
        }

        lumieres[etatFeu].material.SetColor(PROPRIETE_EMISSION, 
                lumieres[etatFeu].material.color * intensite);

        PermetCirculation = etatFeu == 0;       // Le 0 représente un feu vert
    }
}
