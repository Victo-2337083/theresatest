using UnityEngine;

/// <summary>
/// Gestionnaire de l'environnement du jeu
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    // Instance unique du contrôleur de jeu
    private static ControleurJeu instance;

    /// <summary>
    /// Instance unique du contrôleur de jeu
    /// </summary>
    public static ControleurJeu Instance => instance;

    [field:SerializeField, Tooltip("Le personnage actif")]
    public Personnage Personnage { get; private set; }

    private void Awake()
    {
        //Implémentation pour garantir l'unicité du contrôleur de jeu
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
