using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voiture : MonoBehaviour
{
    // position
    // vitesse
    public float vitesse;
    // touche pour faire avancer

    private void Update()
    {
       transform.position += transform.right * vitesse * Time.deltaTime * Input.GetAxis("Vertical");
    }
}
