using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitialisationControles : MonoBehaviour
{
    public Slider probabilite;
    public TMP_InputField tempsAttente;

    private void Start()
    {
        probabilite.value = GenerateurVehicule.Instance.chanceGeneration;
        tempsAttente.text = $"{GenerateurVehicule.Instance.tempsAttente}";
    }
}
