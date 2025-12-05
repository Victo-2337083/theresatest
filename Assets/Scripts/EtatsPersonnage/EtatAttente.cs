using UnityEngine;

namespace EtatPersonnages
{
    /// <summary>
    /// Le personnage est immobile et attend qu'une action se produise
    /// </summary>
    public class EtatAttente : EtatPersonnage
    {
        /// <summary>
        /// Attent de recevoir un signal de déplacement ou d'attaque
        /// </summary>
        /// <param name="personnage">Le personnage qui est dans l'état</param>
        /// <returns>L'état de la prochaine frame</returns>
        public override EtatPersonnage ExecuterEtat(Personnage personnage)
        {
            if (personnage.EnDeplacement)
            {
                return new EtatDeplacement();
            }
            if(personnage.AttaqueEnCours)
            {
                return new EtatAttaque();
            }

            return this;
        }
    }
}
