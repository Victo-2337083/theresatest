using UnityEngine;


namespace EtatsMonstre
{
    /// <summary>
    /// Le monstre attaque le personnage. Si le personnage s'éloigne trop, alors le monstre revient en mode poursuite.
    /// </summary>
    public class EtatAttaque : EtatMonstre
    {
        /// <summary>
        /// Lamce une attaque vers personnage. S'il est assez loin, alors on tombe en mode poursuite.
        /// </summary>
        /// <param name="monstre">Le monstre dont on gère le comportement</param>
        /// <returns>L'état à exécuter à la prochaine frame.</returns>
        public override EtatMonstre ExecuterEtat(Monstre monstre)
        {
            monstre.DemarrerAttaque();

            // Personnage se sauve - retourner en poursuite
            Vector3 positionPersonnage = ControleurJeu.Instance.Personnage.transform.position;
            float distanceAuPersonnage = Vector3.Distance(positionPersonnage, monstre.transform.position);
            if (distanceAuPersonnage > monstre.RayonAttaque)
            {
                return new EtatPoursuite();
            }

            return this;

        }
    }
}