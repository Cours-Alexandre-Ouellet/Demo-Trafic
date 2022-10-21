using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autobus : VoitureAutomatique
{
    public bool zoneArretDetectee;
    public float tempsArret = 3f;
    public ZoneArretBus dernierArret;

    public override string NomType => "Autobus";

    private new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        zoneArretDetectee = false;
        dernierArret = null;
    }

    // Update is called once per frame
    void Update()
    {
        Avancer();
    }

    public void EntrerZoneArret(ZoneArretBus arret)
    {
        if(arret.Equals(dernierArret))
        {
            return;
        }

        dernierArret = arret;
        zoneArretDetectee = true;
        StartCoroutine(DelaiArret());
    }

    protected override bool PeutAvancer()
    {
        return !zoneArretDetectee && base.PeutAvancer();
    }

    public IEnumerator DelaiArret()
    {
        float tempsEcoule = 0f;
        while(tempsEcoule < tempsArret)
        {
            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        zoneArretDetectee = false;
    }
}
