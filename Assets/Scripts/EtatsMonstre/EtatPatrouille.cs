using UnityEngine;

namespace EtatsMonstre
{

    /// <summary>
    /// État dans lequel un monstre patrouille une zone. Lorsqu'il atteint le point de patrouille, alors 
    /// il choisit une nouvelle destination
    /// </summary>
    public class EtatPatrouille : EtatMonstre
    {
        /// <inheritdoc/>
        public override void EntrerEtat(Monstre monstre)
        {
            base.EntrerEtat(monstre);
            monstre.DemarrerMarche();
        }

        /// <summary>
        /// Patrouille une zone. Si durant la patrouille le personnage devient suffisamment proche, alors le monstre se met à 
        /// poursuivre
        /// </summary>
        /// <param name="monstre">Le monstre géré par l'état</param>
        /// <returns>L'état du monstre de la prochaine frame.</returns>
        public override EtatMonstre ExecuterEtat(Monstre monstre)
        {
            //Destination atteinte
            if (Vector3.Distance(monstre.transform.position, monstre.Destination) < 1.0f)
            {
                monstre.ChoisirDestination();
            }

            // Engager poursuite
            if (Vector3.Distance(ControleurJeu.Instance.Personnage.transform.position,
                monstre.transform.position) < monstre.RayonPoursuite)
            {
                return new EtatPoursuite();
            }

            // Continuer la patrouille
            return this;
        }

        /// <inheritdoc/>
        public override void SortirEtat(Monstre monstre)
        {
            base.SortirEtat(monstre);
            monstre.ArreterMarche();
        }
    }
}