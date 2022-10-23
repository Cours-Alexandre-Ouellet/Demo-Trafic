using System.Collections.Generic;
using UnityEngine;

public class ConstructeurChemin
{
    public Path ConstruireChemin(ISupportChemin supportInitial)
    {
        List<Path> chemins = new List<Path>();
        Visiter(supportInitial, ref chemins);

        return Fusionner(chemins);
    }

    public void Visiter(ISupportChemin supportChemin, ref List<Path> chemins)
    {
        if (supportChemin is not null)
        {
            supportChemin.Accepter(this, ref chemins);
        }
    }

    private Path Fusionner(List<Path> chemins)
    {
        if (chemins.Count == 0)
        {
            return null;
        }

        Path chemin = new Path(Vector3.zero, "Chemin");
        chemin.AutoSetControlPoints = false;

        // Premier segment
        for (int i = 0; i < 4; i++)
        {
            chemin[i] = chemins[0][i];
        }

        for (int i = 1; i < chemins.Count; i++)
        {
            chemin.AddSegment(chemins[i][3]);
            chemin[i * 3 + 1] = chemins[i][1];
            chemin[i * 3 + 2] = chemins[i][2];
        }

        return chemin;
    }
}

