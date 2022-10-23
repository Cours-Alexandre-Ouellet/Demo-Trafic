using UnityEngine;

public class CommandeAvancer : AbstractCommand<VehiculeAutomatique>
{
    private readonly bool verifierPourCollision;

    private const float ZONE_VERIF = 0.75f;

    public CommandeAvancer(bool verifierPourCollision)
    {
        this.verifierPourCollision = verifierPourCollision;
    }

    public override void Execute(VehiculeAutomatique vehicule)
    {
        if(verifierPourCollision)
        {
            Vector3 extendsVehicule = vehicule.GetComponent<MeshFilter>().sharedMesh.bounds.extents;
            Vector3 source = vehicule.transform.position - vehicule.transform.right * (extendsVehicule.x + ZONE_VERIF);
            source.y = extendsVehicule.y;

            Vector3 etendue = extendsVehicule;
            etendue.x = ZONE_VERIF * 0.66f;

            if (Physics.CheckBox(source, etendue, vehicule.transform.rotation, LayerMask.GetMask("Voiture")))
            {
                return;     // Ne peut pas avancer
            }
        }

        PathFollower suiveur = vehicule.Suiveur;
        (Vector3, Quaternion) infoApresMouvement = suiveur.MoveForward();
        infoApresMouvement.Item1.y = vehicule.transform.position.y;

        vehicule.transform.position = infoApresMouvement.Item1;
        vehicule.transform.rotation = infoApresMouvement.Item2;
    }
}
