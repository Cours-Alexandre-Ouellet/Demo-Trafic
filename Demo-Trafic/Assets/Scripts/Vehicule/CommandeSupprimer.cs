using UnityEngine;

public class CommandeSupprimer : AbstractCommand<VehiculeAutomatique>
{
    public override void Execute(VehiculeAutomatique vehicule)
    {
        Object.Destroy(vehicule.gameObject);
    }
}
