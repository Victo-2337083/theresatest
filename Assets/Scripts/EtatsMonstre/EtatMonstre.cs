using UnityEngine;

namespace EtatsMonstre
{

    /// <summary>
    /// Gère le comportement d'un monstre
    /// </summary>
    public abstract class EtatMonstre
    {
        /// <summary>
        /// Actions à accomplir lorsqu'un monstre entre dans l'état
        /// </summary>
        /// <param name="monstre">Le monstre dont on gère le comportement</param>
        public virtual void EntrerEtat(Monstre monstre)
        {
            Debug.Log($"Monstre entre dans l'état {this.GetType().Name}");
        }

        /// <summary>
        /// Actions à accomplir lorsqu'un monstre est dans cet état. Comprends aussi la logique de 
        /// passage aux autres états.
        /// </summary>
        /// <param name="monstre">Le monstre dont on gère le comportement</param>
        /// <returns>L'état à exécuter à la prochaine frame.</returns>
        public abstract EtatMonstre ExecuterEtat(Monstre monstre);

        /// <summary>
        /// Actions à accomplir lorsqu'un monstre sort de l'état
        /// </summary>
        /// <param name="monstre">Le monstre dont on gère le comportement</param>
        public virtual void SortirEtat(Monstre monstre)
        {
            Debug.Log($"Monstre sort de l'état {this.GetType().Name}");
        }
    }
}