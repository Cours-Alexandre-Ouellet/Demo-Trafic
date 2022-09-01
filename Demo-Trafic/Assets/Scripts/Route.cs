using UnityEngine;

/// <summary>
/// Orientation d'un chemin
/// </summary>
public enum Orientation
{
    AXE_X, AXE_Z
}

/// <summary>
/// Gère la création d'une route
/// </summary>
public class Route : MonoBehaviour
{
    [Header("Création des routes")]
    public GameObject prototypeRoute;       // Modèle de la route
    public GameObject[] segmentsRoute;      // Liste des segments créés 
    public float tailleSegment = 3f;        // Taille des segments de route

    [Header("Coordonnées du chemin")]
    public Vector3 debut;                   // Coordonnées du début du chemin
    public Vector3 fin;                     // Coordonnées de la fin du chemin
    public Orientation orientation;         // Orientation donnée du chemin

    // Variables internes
    private float demiTailleSegment;        // Moitié de la taille d'un segment

    private void Awake()
    {
        demiTailleSegment = 0.5f * tailleSegment;
    }

    void Start()
    {
        // Validation que debut < fin
        if(orientation == Orientation.AXE_X && debut.x > fin.x ||
            orientation == Orientation.AXE_Z && debut.z > fin.z)
        {
            debut += fin;
            fin = debut - fin;
            debut -= fin;
        }

        GenererChemin();
    }

    /// <summary>
    /// Crée tous les segments qui constitueront le chemin
    /// </summary>
    private void GenererChemin ()
    {
        // Paramètres du chemin à créer
        float nombreSegments = (orientation == Orientation.AXE_X ? fin.x - debut.x : fin.z - debut.z) / tailleSegment;
        int nombreSegmentsComplets = Mathf.FloorToInt(nombreSegments);
        bool ajoutSegmentPartiel = !Mathf.Approximately(nombreSegments, nombreSegmentsComplets);
        int nombreSegmentsTotal = nombreSegmentsComplets + (ajoutSegmentPartiel ? 1 : 0);

        segmentsRoute = new GameObject[nombreSegmentsTotal];

        // Création des segments réguliers
        for(int i = 0; i < nombreSegmentsComplets; i++)
        {
            CreerSegment(i, PositionSegment(i), Vector3.one);
        }

        // Création du segment partiel
        if(ajoutSegmentPartiel)
        {
            Vector3 positionSegment = PositionSegment(nombreSegmentsComplets);
            Vector3 echelleSegment = fin - positionSegment + demiTailleSegment * (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward);
            echelleSegment /= tailleSegment;

            echelleSegment.y = 1.0f;
            if(orientation == Orientation.AXE_X)
            {
                echelleSegment.z = 1.0f;
                positionSegment.x -= tailleSegment * (1.0f - echelleSegment.x) * 0.5f;
            }
            else
            {
                echelleSegment.x = 1.0f;
                positionSegment.z -= tailleSegment * (1.0f - echelleSegment.z) * 0.5f;
            }

            CreerSegment(nombreSegmentsComplets, positionSegment, echelleSegment);
        }

    }

    /// <summary>
    /// Détermine la position de départ du segment.
    /// </summary>
    /// <param name="indiceSegment">Le numéro du segment pour lequel obtenir la position.</param>
    /// <returns>La position du segment.</returns>
    private Vector3 PositionSegment(int indiceSegment)
    {
        return debut + tailleSegment * indiceSegment * 
            (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward) +
            demiTailleSegment * (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward);
    }

    /// <summary>
    /// Crée un nouveau segment et l'insère dans le tableau des références de segments.
    /// </summary>
    /// <param name="indiceSegment">L'indice du segment à créer.</param>
    /// <param name="position">La position du segment créé.</param>
    /// <param name="echelle">Le facteur de mise à l'échelle du segment créé.</param>
    private void CreerSegment(int indiceSegment, Vector3 position, Vector3 echelle)
    {
        GameObject segmentRoute = Instantiate(prototypeRoute, transform);
        segmentRoute.transform.position = position;
        segmentRoute.transform.localScale = echelle;
        segmentsRoute[indiceSegment] = segmentRoute;
    }   
}
