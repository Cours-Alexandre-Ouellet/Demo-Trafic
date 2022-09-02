using UnityEngine;

/// <summary>
/// Orientation d'un chemin
/// </summary>
public enum Orientation
{
    AXE_X, AXE_Z
}

/// <summary>
/// G�re la cr�ation d'une route
/// </summary>
public class Route : MonoBehaviour
{
    private const float ELEVATION_ROUTE = 0.01f;
    private const float LARGEUR_LIGNE = 0.2f;

    [Header("Cr�ation des routes")]
    public GameObject prototypeRoute;       // Mod�le de la route
    public GameObject[] segmentsRoute;      // Liste des segments cr��s 
    public float tailleSegment = 3f;        // Taille des segments de route


    [Header("Coordonn�es du chemin")]
    public Vector3 debut;                   // Coordonn�es du d�but du chemin
    public Vector3 fin;                     // Coordonn�es de la fin du chemin
    public Orientation orientation;         // Orientation donn�e du chemin

    // Variables internes
    private float demiTailleSegment;        // Moiti� de la taille d'un segment

    [Header("Lignes")]
    public Material ligneDiscontinue;
    // Composant  LineRenderer

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
        CreerLigne();
    }

    /// <summary>
    /// Cr�e tous les segments qui constitueront le chemin
    /// </summary>
    private void GenererChemin()
    {
        // Param�tres du chemin � cr�er
        float nombreSegments = (orientation == Orientation.AXE_X ? fin.x - debut.x : fin.z - debut.z) / tailleSegment;
        int nombreSegmentsComplets = Mathf.FloorToInt(nombreSegments);
        bool ajoutSegmentPartiel = !Mathf.Approximately(nombreSegments, nombreSegmentsComplets);
        int nombreSegmentsTotal = nombreSegmentsComplets + (ajoutSegmentPartiel ? 1 : 0);

        segmentsRoute = new GameObject[nombreSegmentsTotal];

        // Cr�ation des segments r�guliers
        for(int i = 0; i < nombreSegmentsComplets; i++)
        {
            CreerSegment(i, PositionSegment(i), prototypeRoute.transform.localScale);
        }

        // Cr�ation du segment partiel
        if(ajoutSegmentPartiel)
        {
            Vector3 positionSegment = PositionSegment(nombreSegmentsComplets);
            Vector3 echelleSegment = fin - positionSegment + demiTailleSegment * (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward);
            echelleSegment /= tailleSegment;

            echelleSegment.y = 1.0f;
            if(orientation == Orientation.AXE_X)
            {
                echelleSegment.z = prototypeRoute.transform.localScale.z;
                positionSegment.x -= tailleSegment * (1.0f - echelleSegment.x) * 0.5f;
            }
            else
            {
                echelleSegment.x = prototypeRoute.transform.localScale.x;
                positionSegment.z -= tailleSegment * (1.0f - echelleSegment.z) * 0.5f;
            }

            CreerSegment(nombreSegmentsComplets, positionSegment, echelleSegment);
        }

    }

    /// <summary>
    /// D�termine la position de d�part du segment.
    /// </summary>
    /// <param name="indiceSegment">Le num�ro du segment pour lequel obtenir la position.</param>
    /// <returns>La position du segment.</returns>
    private Vector3 PositionSegment(int indiceSegment)
    {
        return debut + tailleSegment * indiceSegment *
            (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward) +
            demiTailleSegment * (orientation == Orientation.AXE_X ? Vector3.right : Vector3.forward);
    }

    /// <summary>
    /// Cr�e un nouveau segment et l'ins�re dans le tableau des r�f�rences de segments.
    /// </summary>
    /// <param name="indiceSegment">L'indice du segment � cr�er.</param>
    /// <param name="position">La position du segment cr��.</param>
    /// <param name="echelle">Le facteur de mise � l'�chelle du segment cr��.</param>
    private void CreerSegment(int indiceSegment, Vector3 position, Vector3 echelle)
    {
        GameObject segmentRoute = Instantiate(prototypeRoute, transform);
        segmentRoute.transform.position = position;
        segmentRoute.transform.localScale = echelle;
        segmentsRoute[indiceSegment] = segmentRoute;
    }

    private void CreerLigne()
    {
        GameObject ligneCentrale = new GameObject("Ligne discontinue");
        ligneCentrale.transform.SetParent(transform);
        ligneCentrale.transform.Rotate(Vector3.right, 90f);

        LineRenderer ligne = ligneCentrale.AddComponent<LineRenderer>();

        ligne.SetPositions(new Vector3[2]
        {
            debut + ELEVATION_ROUTE * Vector3.up,
            fin + ELEVATION_ROUTE * Vector3.up
        });
        ligne.material = ligneDiscontinue;
        ligne.startWidth = LARGEUR_LIGNE;
        ligne.endWidth = LARGEUR_LIGNE;
        ligne.alignment = LineAlignment.TransformZ;
        ligne.textureMode = LineTextureMode.Tile;
    }


}
