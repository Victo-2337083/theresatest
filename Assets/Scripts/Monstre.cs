using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using EtatsMonstre;

/// <summary>
/// Gère un monstre dans le jeu. Le monstre peut :
/// 
/// - Patrouiller une zone
/// - Poursuivre le personnage
/// - Attaquer le personnage
/// </summary>
[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class Monstre : MonoBehaviour, FrappableParArme
{
    /// <summary>
    /// NavMeshAgent qui gère le monstre
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// La destination du monstre pour se déplacer
    /// </summary>
    public Vector3 Destination { get; private set; }

    [field:SerializeField, Tooltip("Le rayon dans lequel le monstre commence à poursuivre le personnage.")]
    public float RayonPoursuite { get; private set; }

    [field: SerializeField, Tooltip("Le rayon dans lequel le monstre commence à attaque le personnage.")]
    public float RayonAttaque { get; private set; }

    [SerializeField, Tooltip("Rayon dans lequel le monstre patrouille")]
    private float rayonPatrouille;

    [SerializeField, Tooltip("Position initiale du monstre")]
    private Vector3 positionInitiale;

    // Contrôleur d'animation du monstre
    private Animator controleurAnimation;

    [SerializeField, Tooltip("Temps pour accomplir l'attaque et attendre entre deux attaques.")]
    private float tempsAttaque;

    // Est-ce qu'une attaque se fait présentement
    private bool attaqueEnCours;

    [SerializeField, Tooltip("Arme avec laquelle l'attaque se fait")]
    private Arme arme;

    #region Gestion de la vie
    // Vie actuelle du monstre
    private float vie;

    [SerializeField, Tooltip("Vie maximale (et initiale) du monstre.")]
    private float vieMaximale;

    [SerializeField, Tooltip("Gère l'affichage de la vie du monstre.")]
    private BarreProgression barreVie;
    #endregion

    #region Gestion de l'état du monstre
    /// <summary>
    /// État actuel du monstre
    /// </summary>
    private EtatMonstre etatPrecedent;

    private EtatMonstre prochainEtat;
    #endregion

    private void Awake()
    {
        // Récupère une référence sur le component
        agent = GetComponent<NavMeshAgent>();
        controleurAnimation = GetComponent<Animator>();

        attaqueEnCours = false;

        // On commence dans l'état de patrouille
        etatPrecedent = null;
        prochainEtat = new EtatPatrouille();

        vie = vieMaximale;

        // TODO : À DÉPLACER
        ChoisirDestination();
    }

    private void Start()
    {
        positionInitiale = transform.position;
    }

    private void Update()
    {
        // Prochain état représente l'état à exécuter
        if (etatPrecedent != prochainEtat)
        {
            prochainEtat.EntrerEtat(this);
        }
        // On n'a plus besoin de cette valeur, on fait donc la mise à jour
        etatPrecedent = prochainEtat;
        prochainEtat = prochainEtat.ExecuterEtat(this);      // Met à jour le prochain état à exécuter. 
                                                            // À partir de ce point, l'état de la frame est accessible dans etatPrecedent (voir ligne au-dessus).

        if (etatPrecedent != prochainEtat)
        {
            etatPrecedent.SortirEtat(this);
        }
    }

    /// <summary>
    /// Assigne une nouvelle destination au hasard au monstre
    /// </summary>
    public void ChoisirDestination()
    {
        /*Vector3 pointAleatoire = new Vector3(Random.value * 50.0f - 25.0f,
            transform.position.y,
            Random.value * 50.0f - 25.0f);*/
        Vector3 pointAleatoire = positionInitiale + Random.insideUnitSphere * rayonPatrouille;

        AssignerDestination(pointAleatoire);
    }

    /// <summary>
    /// Assigne une destination au monstre
    /// </summary>
    /// <param name="destination">La destination à assigner au monstre</param>
    public void AssignerDestination(Vector3 destination)
    {
        // Permet d'obtenir un point sur le nav mesh
        if(NavMesh.SamplePosition(destination, out NavMeshHit pointDeterminee, 100f, NavMesh.AllAreas))
        {
            Destination = pointDeterminee.position;
        }

        agent.destination = Destination;
    }

    /// <summary>
    /// Commencer l'animation de marcher
    /// </summary>
    public void DemarrerMarche()
    {
        controleurAnimation.SetBool("Marcher", true);
    }

    /// <summary>
    /// Arrête l'animation de marcher
    /// </summary>
    public void ArreterMarche()
    {
        controleurAnimation.SetBool("Marcher", false);
    }

    /// <summary>
    /// Commencer la logique d'attaque du monstre
    /// </summary>
    public void DemarrerAttaque()
    {
        // Si une attaque n'a pas déjà lieu
        if(attaqueEnCours)
        {
            return;
        }

        // Lance la coroutine. Appeler Attaquer() ne permettrait pas de gérer le comportement avec le délai d'attente
        StartCoroutine(Attaquer());
    }

    /// <summary>
    /// Lance l'animation d'attaque et attend un délai dans lequel le monstre est devient inactif
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attaquer()
    {
        attaqueEnCours = true;

        arme.CommencerAttaque();
        controleurAnimation.SetTrigger("Attaquer");
        // Bloque l'exécution du code de cette méthode pour le nombre de seconde de tempsAttaque
        yield return new WaitForSeconds(tempsAttaque);

        attaqueEnCours = false;
    }

    /// <summary>
    /// Lorsqu'un monstre est frappé, il reçoit des dégâts.
    /// </summary>
    /// <param name="arme">L'arme qui a frappé le monstre.</param>
    public void RecevoirFrappe(Arme arme)
    {
        vie -= arme.Degat;
        barreVie.SetProgression(vie / vieMaximale);

        if(vie <= 0)
        {
            Debug.Log("Je me meurs !");
        }
    }
}
