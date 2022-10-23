using System;
using UnityEngine;

[Serializable]
public class Chemin
{
    public Vector3 depart;
    public Vector3 arrivee;

    public Vector3 Depart
    {
        get { return depart; }
        set { depart = value; }
    }

    public Vector3 Arrivee
    {
        get { return arrivee; }
        set { arrivee = value; }
    }
}
