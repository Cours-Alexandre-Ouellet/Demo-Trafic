public class EtatAvancer : IState<VehiculeAutomatique>
{
    public AbstractCommand<VehiculeAutomatique>[] OnStateEnter(VehiculeAutomatique vehicule)
    {
        vehicule.TempsAttente = 0.0f;
        return new AbstractCommand<VehiculeAutomatique>[] { new CommandeChangerVitesse(vehicule.vitesseMaximale) };
    }

    public AbstractCommand<VehiculeAutomatique>[] OnStateStay(VehiculeAutomatique vehicule, out IState<VehiculeAutomatique> etatSuivant)
    {
        if (vehicule.PeutAvancer && !vehicule.Suiveur.ReachEndOfPath)
        {
            etatSuivant = this;
            return new AbstractCommand<VehiculeAutomatique>[] { new CommandeAvancer(true) };
        }
        else
        {
            if (vehicule.Suiveur.ReachEndOfPath)
            {
                etatSuivant = this;
                return new AbstractCommand<VehiculeAutomatique>[] { new CommandeSupprimer() };
            }
            else
            {
                etatSuivant = new EtatArret();
                return null;
            }
        }
    }

    public AbstractCommand<VehiculeAutomatique>[] OnStateExit(VehiculeAutomatique objectControle)
    {
        return null;
    }
}
