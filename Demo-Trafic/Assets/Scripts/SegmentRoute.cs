using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SegmentRoute : ISupportChemin
{
    public LineRenderer ligneBord1;
    public LineRenderer ligneBord2;
    public LineRenderer ligneMediane;

    private const float DECALAGE_BORD = 0.2f;
    private const float DECALAGE_CHEMIN = 1.5f;
    private const float TAILLE_INITIALE = 3f;
    private const float ELEVATION_LIGNE = 0.05f;

    public Intersection[] intersectionsConnectees;

    private bool estOrienteeX;
    private Vector3 directionDecalage;
    private Vector3 directionLongueur;
    private float longueur;
    private float largeur;

    public bool ConnecteDeuxExtremites => intersectionsConnectees.Length == 2;

    public void Awake()
    {
        pointsSortie = new Vector3[2];
        pointsArrive = new Vector3[2];

        estOrienteeX = transform.localScale.x > transform.localScale.z;
        directionDecalage = (estOrienteeX) ? Vector3.forward : Vector3.right;
        directionLongueur = (estOrienteeX) ? Vector3.right : Vector3.forward;
        longueur = TAILLE_INITIALE * (estOrienteeX ? transform.localScale.x : transform.localScale.z);
        largeur = TAILLE_INITIALE * (estOrienteeX ? transform.localScale.z : transform.localScale.x);

        CreerPointAncrage(directionDecalage, directionLongueur, longueur, estOrienteeX);
    }

    private void Start()
    {
        CreerLignes(estOrienteeX, directionDecalage, directionLongueur, longueur, largeur);
        foreach (Intersection intersection in intersectionsConnectees)
        {
            intersection.AjouterSegment(this);
        }
    }

    private void CreerLignes(bool estOrienteeX, Vector3 directionDecalage, Vector3 directionLongueur, float longueur, float largeur)
    {
        Vector3[] positions = new Vector3[2]
        {
            transform.position - directionLongueur * longueur * 0.5f + Vector3.up * ELEVATION_LIGNE,
            transform.position + directionLongueur * longueur * 0.5f + Vector3.up * ELEVATION_LIGNE
        };

        ligneMediane.SetPositions(positions);

        // Rotation autour de l'axe des x, donc on doit ajuster l'échelle en y selon la valeur en z pour le bon résultat
        ligneMediane.transform.localScale = estOrienteeX ? new Vector3(1.0f / transform.localScale.x, 1.0f, 1.0f) : new Vector3(1.0f, 1.0f / transform.localScale.z, 1.0f);

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += directionDecalage * (0.5f * largeur - DECALAGE_BORD);
        }
        ligneBord1.SetPositions(positions);

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= directionDecalage * 2.0f * (0.5f * largeur - DECALAGE_BORD);
        }
        ligneBord2.SetPositions(positions);
    }

    public override (Path, ISupportChemin) SelectionnerChemin()
    {
        for (int i = 0; i < pointsSortie.Length; i++)
        {
            if (VoisinPossedeCheminDebutant(pointsSortie[i]))
            {
                return GenererChemin(i);
            }
        }

        return (null, null);
    }

    public override (Path, ISupportChemin) SelectionnerChemin(Vector3 positionArrivee)
    {
        return GenererChemin(positionArrivee == pointsArrive[0] ? 0 : 1);
    }

    private bool VoisinPossedeCheminDebutant(Vector3 point)
    {
        foreach(Intersection voisin in intersectionsConnectees)
        {
            if(voisin.PossedeCheminDebutant(point))
            {
                return true;
            }
        }

        return false;
    }

    private (Path, ISupportChemin) GenererChemin(int indice)
    {
        Path chemin = new Path(Vector3.zero, $"{gameObject.name}");
        Vector3 direction = pointsSortie[indice] - pointsArrive[indice];

        chemin[0] = pointsArrive[indice];
        chemin[1] = pointsArrive[indice] + direction * 0.33f;
        chemin[2] = pointsSortie[indice] - direction * 0.33f;
        chemin[3] = pointsSortie[indice];

        ISupportChemin supportSuivant = null;
        if(intersectionsConnectees[0].PossedeCheminDebutant(pointsSortie[indice]))
        {
            supportSuivant = intersectionsConnectees[0];
        }
        else if(intersectionsConnectees.Length >= 2 && intersectionsConnectees[1].PossedeCheminDebutant(pointsSortie[indice]))
        {
            supportSuivant = intersectionsConnectees[1];
        }

        return (chemin, supportSuivant);
    }

    

    private void CreerPointAncrage(Vector3 directionDecalage, Vector3 directionLongueur, float longueur, bool estOrienteX)
    {
        Vector3[] positions = new Vector3[2]
        {
            transform.position - directionLongueur * longueur * 0.5f,
            transform.position + directionLongueur * longueur * 0.5f
        };

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += directionDecalage * DECALAGE_CHEMIN;
        }
        pointsArrive[0] = positions[estOrienteX ? 1 : 0];
        pointsSortie[0] = positions[estOrienteX ? 0 : 1];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= directionDecalage * 2.0f * DECALAGE_CHEMIN;
        }
        pointsArrive[1] = positions[estOrienteX ? 0 : 1];
        pointsSortie[1] = positions[estOrienteX ? 1 : 0];
        
    }
}
