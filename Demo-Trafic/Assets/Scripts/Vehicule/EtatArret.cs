public class EtatArret : IState<VehiculeAutomatique>
{
    public AbstractCommand<VehiculeAutomatique>[] OnStateEnter(VehiculeAutomatique vehicule)
    {
        return new AbstractCommand<VehiculeAutomatique>[] { new CommandeChangerVitesse(0.0f) };            
    }

    public AbstractCommand<VehiculeAutomatique>[] OnStateStay(VehiculeAutomatique vehicule, out IState<VehiculeAutomatique> etatSuivant)
    {
        if(vehicule.PeutAvancer)
        {
            etatSuivant = new EtatAvancer();
        }
        else
        {
            etatSuivant = this;
        }

        return null;
    }

    public AbstractCommand<VehiculeAutomatique>[] OnStateExit(VehiculeAutomatique vehicule)
    {
        return null;
    }
}
