using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère une barre de progression
/// </summary>
public class BarreProgression : MonoBehaviour
{
    [SerializeField, Tooltip("L'image qui doit être remplie selon la progression.")]
    private Image barre;

    /// <summary>
    /// Modifie la progression affichée de la barre.
    /// </summary>
    /// <param name="progression">Le pourcentage de progression.</param>
    public void SetProgression(float progression)
    {
        // Restreint la valeur entre 0 et 1
        progression = Mathf.Clamp01(progression);
        
        // Affiche le bon niveau de progression
        barre.fillAmount = progression;
    }
}
