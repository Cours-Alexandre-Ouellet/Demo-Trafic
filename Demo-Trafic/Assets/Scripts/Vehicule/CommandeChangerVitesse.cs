public class CommandeChangerVitesse : AbstractCommand<VehiculeAutomatique>
{
    private readonly float vitesse;

    public CommandeChangerVitesse(float vitesse)
    {
        this.vitesse = vitesse;
    }

    public override void Execute(VehiculeAutomatique voiture)
    {
        voiture.SetVitesse(vitesse);
    }
}

