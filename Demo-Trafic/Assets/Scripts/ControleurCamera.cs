using UnityEngine;

public class ControleurCamera : MonoBehaviour
{
    [SerializeField]
    private float vitesse;

    [SerializeField]
    private float vitesseRotation;

    [SerializeField]
    private float vitesseZoom;

    public void FixedUpdate()
    {
        GererDeplacement();
        GererRotation();
        GererZoom();
    }

    private void GererDeplacement()
    {
        Vector3 devantPlat = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 droitPlat = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        if(Input.GetKey(KeyCode.W))
        {
            transform.position += devantPlat * vitesse * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position -= devantPlat * vitesse * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position -= droitPlat * vitesse * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += droitPlat * vitesse * Time.deltaTime;
        }
    }

    private void GererRotation()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.down, vitesseRotation * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, vitesseRotation * Time.deltaTime, Space.World);
        }
    }

    private void GererZoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position += transform.forward * vitesseZoom * Time.deltaTime;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position -= transform.forward * vitesseZoom * Time.deltaTime;
        }
    }

    

    
}
