using UnityEngine;

namespace EtatPersonnages
{

    /// <summary>
    /// Gère le comportement d'un personnage
    /// </summary>
    public abstract class EtatPersonnage
    {
        /// <summary>
        /// Actions à accomplir lorsqu'un personnage entre dans l'état
        /// </summary>
        /// <param name="personnage">Le personnage dont on gère le comportement</param>
        public virtual void EntrerEtat(Personnage personnage)
        {
            Debug.Log($"Personnage entre dans l'état {this.GetType().Name}");
        }

        /// <summary>
        /// Actions à accomplir lorsqu'un personnage est dans cet état. Comprends aussi la logique de 
        /// passage aux autres états.
        /// </summary>
        /// <param name="personnage">Le personnage dont on gère le comportement</param>
        /// <returns>L'état à exécuter à la prochaine frame.</returns>
        public abstract EtatPersonnage ExecuterEtat(Personnage personnage);

        /// <summary>
        /// Actions à accomplir lorsqu'un personnage sort de l'état
        /// </summary>
        /// <param name="personnage">Le personnage dont on gère le comportement</param>
        public virtual void SortirEtat(Personnage personnage)
        {
            Debug.Log($"Personnage sort de l'état {this.GetType().Name}");
        }
    }
}