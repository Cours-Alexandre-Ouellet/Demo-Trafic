using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class VitesseSimulation
{
    public string Etiquette { get; set; }
    public float Vitesse { get; set; }
}

public class ControleurSimulation : MonoBehaviour
{
    public int indiceVitesseSimulation = 2;

    public TMP_Dropdown listeVitesses;

    private VitesseSimulation[] vitessesSimulation = new VitesseSimulation[4]
    {
        new VitesseSimulation {Etiquette = "Pause", Vitesse = 0f},
        new VitesseSimulation {Etiquette = "Ralentie (0.5x)", Vitesse = 0.5f},
        new VitesseSimulation {Etiquette = "Normale (1x)", Vitesse = 1f},
        new VitesseSimulation {Etiquette = "Rapide (2x)", Vitesse = 2f},
    };

    private void Start()
    {
        listeVitesses.AddOptions(
            vitessesSimulation.Select(x => new TMP_Dropdown.OptionData(x.Etiquette)).ToList());
        listeVitesses.value = indiceVitesseSimulation;
    }

    public void ChangementVitesse(int option)
    {
        indiceVitesseSimulation = option;
        Time.timeScale = vitessesSimulation[indiceVitesseSimulation].Vitesse;
    }
}
