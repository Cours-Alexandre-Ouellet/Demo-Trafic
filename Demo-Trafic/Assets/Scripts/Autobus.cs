using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autobus : VoitureAutomatique
{
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Avancer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    protected override bool PeutAvancer()
    {
        
    }
}
