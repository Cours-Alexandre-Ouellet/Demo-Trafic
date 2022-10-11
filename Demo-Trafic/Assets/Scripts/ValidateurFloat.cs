using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ValidateurFloat : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public Color couleurErreur = Color.red;
    public Color couleurNormale;
    public GenerateurVehicule generateur;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        couleurNormale = textMesh.color;
    }

    public void ValiderFloat(string valeur)
    {
        if(float.TryParse(valeur, out float nombre))
        {
            textMesh.color = couleurNormale;
            generateur.SetTempsAttente(nombre);
        }
        else
        {
            /*if(textMesh.text.Length > 0)
            {
                textMesh.text = textMesh.text.Substring(0, textMesh.text.Length - 1);
            }*/
            textMesh.color = couleurErreur;
        }
    }
}
