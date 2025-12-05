using UnityEngine;

namespace EtatPersonnages
{
    /// <summary>
    /// Action d'attaque du personnage
    /// </summary>
    public class EtatAttaque : EtatPersonnage
    {
        /// <inheritdoc/>
        public override void EntrerEtat(Personnage personnage)
        {
            base.EntrerEtat(personnage);
            personnage.Attaquer();
        }

        /// <summary>
        /// Réalise l'action d'attaquer une cible.
        /// </summary>
        /// <param name="personnage">Le personnage qui attaque</param>
        /// <returns>L'état du personnage à la prochaine frame</returns>
        public override EtatPersonnage ExecuterEtat(Personnage personnage)
        {
            if(personnage.AttaqueEnCours)
            {
                return this;
            }

            return new EtatAttente();
        }
    }
}
