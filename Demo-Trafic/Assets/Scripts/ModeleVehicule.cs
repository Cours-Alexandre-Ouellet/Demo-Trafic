using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "modele_vehicule",
    menuName = "Simulation")]
public class ModeleVehicule : ScriptableObject
{
    public string nomVoiture;

    public Material exterieurVoiture;

    public float vitesseMaximale;

    public bool arretBus;

    public void ConvertirJSON()
    {
        string monFichier = JsonUtility.ToJson(this);
        File.WriteAllText(Application.streamingAssetsPath, nomVoiture);
    }

}
