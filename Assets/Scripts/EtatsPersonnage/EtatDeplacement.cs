using UnityEngine;

namespace EtatPersonnages
{
    /// <summary>
    /// Etat d'un personnage qui est entrain de se déplacer
    /// </summary>
    public class EtatDeplacement : EtatPersonnage
    {
        /// <inheritdoc/>
        public override void EntrerEtat(Personnage personnage)
        {
            base.EntrerEtat(personnage);
            personnage.DemarrerDeplacement();
        }

        /// <summary>
        /// Le personnage se déplace selon les contrôles lus
        /// </summary>
        /// <param name="personnage">Le personnage a déplacer</param>
        /// <returns></returns>
        public override EtatPersonnage ExecuterEtat(Personnage personnage)
        {
            personnage.Deplacer();

            if(!personnage.EnDeplacement)
            {
                return new EtatAttente();
            }

            return this;
        }

        /// <inheritdoc/>
        public override void SortirEtat(Personnage personnage)
        {
            base.SortirEtat(personnage);
            personnage.ArreterDeplacement();
        }
    }
}
