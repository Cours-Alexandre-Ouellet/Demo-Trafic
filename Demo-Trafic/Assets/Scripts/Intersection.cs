using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Intersection : ISupportChemin
{
    private Vector3 decalageArrivee = new Vector3(3f, 0f, 1.5f);
    private Vector3 decalageSortie = new Vector3(3f, 0f, -1.5f);

    private List<SegmentRoute> segmentsConnectes;

    private void Awake()
    {
        segmentsConnectes = new List<SegmentRoute>();
        CreerPointsSignificatifs();
    }

    public void AjouterSegment(SegmentRoute segment)
    {
        segmentsConnectes.Add(segment);
    }

    public override (Path, ISupportChemin) SelectionnerChemin()
    {
        throw new System.Exception("Le chemin ne doit pas demarrer dans une intersection.");
    }

    public override (Path, ISupportChemin) SelectionnerChemin(Vector3 position)
    {
        int indiceArrivee = GetIndiceArrive(position);
        int indiceSortie = Random.Range(0, pointsSortie.Length - 1);

        if (indiceSortie >= indiceArrivee)       // On s'assure de ne pas prendre le même indice
        {
            indiceSortie += 1;
        }

        return GenererChemin(indiceArrivee, indiceSortie);
    }

    private int GetIndiceArrive(Vector3 point)
    {
        for (int i = 0; i < pointsArrive.Length; i++)
        {
            if (point == pointsArrive[i])
            {
                return i;
            }
        }

        return -1;
    }

    private void CreerPointsSignificatifs()
    {
        pointsArrive = GetPositionsRoutes(transform.position + decalageArrivee);
        pointsSortie = GetPositionsRoutes(transform.position + decalageSortie);
    }

    private Vector3[] GetPositionsRoutes(Vector3 positionInitiale)
    {
        Vector3[] positions = new Vector3[4];
        Quaternion rotation90 = Quaternion.Euler(0f, 90f, 0f);
        positions[0] = positionInitiale;
        Vector3 position = positionInitiale;
        Vector3 pivot = transform.position;

        for (int i = 1; i < 4; i++)
        {
            position = (rotation90 * (position - pivot)) + pivot;
            positions[i] = position;
        }

        return positions;
    }

    private (Path, ISupportChemin) GenererChemin(int indiceArrive, int indiceSortie)
    {
        Path chemin = new Path(Vector3.zero, $"{gameObject.name}");
        chemin[0] = pointsArrive[indiceArrive];
        chemin[3] = pointsSortie[indiceSortie];

        Vector3 localPoint0 = pointsArrive[indiceArrive] - transform.position;
        Vector3 localPoint1 = pointsSortie[indiceSortie] - transform.position;

        Vector2 signes = new Vector2(Mathf.Sign(localPoint0.x * localPoint1.x), Mathf.Sign(localPoint0.z * localPoint1.z));

        if (signes.x > 0 && signes.y > 0)             // Virage à droite
        {
            CreerCheminVirageDroit(chemin, localPoint0, localPoint1);
        }
        else if (signes.x < 0 && signes.y < 0)       // Virage à gauche
        {
            CreerCheminVirageGauche(chemin, localPoint0, localPoint1);
        }
        else                                         // Tout droit
        {
            CreerCheminDroit(chemin, indiceArrive, indiceSortie);
        }

        return (chemin, TrouverSegmentDebutant(chemin.End));
    }

    private void CreerCheminDroit(Path chemin, int indiceArrive, int indiceSortie)
    {
        Vector3 arrive = pointsArrive[indiceArrive];
        Vector3 sortie = pointsSortie[indiceSortie];

        if (Mathf.Approximately(arrive.x, sortie.x))
        {
            float diff = sortie.z - arrive.z;
            chemin[1] = new Vector3(arrive.x, 0f, arrive.z + diff * 0.333f);
            chemin[2] = new Vector3(sortie.x, 0f, sortie.z - diff * 0.333f);
        }
        else
        {
            float diff = sortie.x - arrive.x;
            chemin[1] = new Vector3(arrive.x + diff * 0.333f, 0f, arrive.z);
            chemin[2] = new Vector3(sortie.x - diff * 0.333f, 0f, sortie.z);
        }
    }

    private void CreerCheminVirageDroit(Path chemin, Vector3 localPoint0, Vector3 localPoint1)
    {
        Vector3 point = new Vector3(1.5f, 0f, 1.5f);
        point.Scale(new Vector3(
            Mathf.Abs(localPoint0.x) < Mathf.Abs(localPoint1.x) ? Mathf.Sign(localPoint0.x) : Mathf.Sign(localPoint1.x),
            0.0f,
            Mathf.Abs(localPoint0.z) < Mathf.Abs(localPoint1.z) ? Mathf.Sign(localPoint0.z) : Mathf.Sign(localPoint1.z)
        ));
        point += transform.position;
        chemin[1] = point;
        chemin[2] = point;
    }

    private void CreerCheminVirageGauche(Path chemin, Vector3 localPoint0, Vector3 localPoint1)
    {
        Vector3 point = new Vector3(1.5f, 0f, 1.5f);
        point.Scale(new Vector3(
            Mathf.Abs(localPoint0.x) > Mathf.Abs(localPoint1.x) ? Mathf.Sign(localPoint0.x) : Mathf.Sign(localPoint1.x),
            0.0f,
            Mathf.Abs(localPoint0.z) > Mathf.Abs(localPoint1.z) ? Mathf.Sign(localPoint0.z) : Mathf.Sign(localPoint1.z)
        ));
        point *= -1.0f;
        point += transform.position;
        chemin[1] = point;
        chemin[2] = point;
    }

    private SegmentRoute TrouverSegmentDebutant(Vector3 pointArrive)
    {
        foreach (SegmentRoute segment in segmentsConnectes)
        {
            if (segment.PossedeCheminDebutant(pointArrive))
            {
                return segment;
            }
        }

        return null;
    }
}
