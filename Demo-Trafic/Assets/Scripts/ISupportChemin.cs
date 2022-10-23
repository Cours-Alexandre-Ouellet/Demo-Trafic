using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public abstract class ISupportChemin : MonoBehaviour
{
    public abstract (Path, ISupportChemin) SelectionnerChemin();

    public abstract (Path, ISupportChemin) SelectionnerChemin(Vector3 positionArrivee);

    public bool PossedeCheminDebutant(Vector3 point)
    {
        foreach(Vector3 pointArrive in pointsArrive)
        {
            if(point == pointArrive)
            {
                return true;
            }
        }
        return false;
    }

    protected Vector3[] pointsSortie;

    public Vector3[] PointsSortie => pointsSortie;

    protected Vector3[] pointsArrive;

    public Vector3[] PointsArrive => pointsArrive;

    public void Accepter(ConstructeurChemin constructeur, ref List<Path> chemins)
    {
        (Path, ISupportChemin) infoChemin;

        if (chemins.Count == 0)
        {
            infoChemin = SelectionnerChemin();
        }
        else
        {
            Vector3 position = chemins[chemins.Count - 1].End;
            infoChemin = SelectionnerChemin(position);
        }

        if (infoChemin.Item1 is not null)
        {
            chemins.Add(infoChemin.Item1);
        }
        constructeur.Visiter(infoChemin.Item2, ref chemins);
    }
}
