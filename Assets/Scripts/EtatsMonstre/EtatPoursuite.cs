using UnityEngine;

namespace EtatsMonstre
{

    /// <summary>
    /// Le monstre poursuit le personnage. Si le personnage s'éloigne trop, alors le monstre revient en mode patrouille, 
    /// s'il est assez proche pour attaquer, alors il attaque.
    /// </summary>
    public class EtatPoursuite : EtatMonstre
    {
        /// <inheritdoc/>
        public override void EntrerEtat(Monstre monstre)
        {
            base.EntrerEtat(monstre);
            monstre.DemarrerMarche();
        }

        /// <summary>
        /// Trouve périodiquement la position du personnage. Si l'on est assez proche, alors on tombe en mode attaque.
        /// </summary>
        /// <param name="monstre">Le monstre dont on gère le comportement</param>
        /// <returns>L'état à exécuter à la prochaine frame.</returns>
        public override EtatMonstre ExecuterEtat(Monstre monstre)
        {
            Vector3 positionPersonnage = ControleurJeu.Instance.Personnage.transform.position;
            monstre.AssignerDestination(positionPersonnage);

            // Engager attaque
            float distanceAuPersonnage = Vector3.Distance(positionPersonnage, monstre.transform.position);
            if (distanceAuPersonnage < monstre.RayonAttaque)
            {
                return new EtatAttaque();
            }
            if (distanceAuPersonnage > monstre.RayonPoursuite)
            {
                return new EtatPatrouille();
            }

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