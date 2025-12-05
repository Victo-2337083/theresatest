using EtatPersonnages;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contrôle le personnage principal du jeu. Répond aux actions de la personne joueuse et 
/// aux événements qui se produisent dans le monde du jeu.
/// </summary>
public class Personnage : MonoBehaviour, FrappableParArme
{
    ///<summary>
    ///Dernière valeur saisie dans les contrôles de déplacement
    ///</summary> 
    private Vector2 controleDeplacement;

    ///<summary>
    ///Dernière valeur saisie dans les contrôles de rotation
    ///</summary> 
    private float controleRotation;

    /// <summary>
    /// Controleur d'animation
    /// </summary>
    private Animator controleurAnimation;

    [Header("Déplacement")]
    [SerializeField, Tooltip("La vitesse du perso en m/s")]
    private float vitesse;

    [SerializeField, Tooltip("La vitesse de rotation du perso en deg/s")]
    private float vitesseRotation;

    [Header("Références aux dépendances")]
    [SerializeField, Tooltip("L'arme du personnage")]
    private Arme arme;

    [SerializeField, Tooltip("Barre de vie du personnage")]
    private BarreProgression barreVie;

    #region Vie du personnage
    [Header("Attributs")]
    [SerializeField, Tooltip("Vie maximale du personnage")]
    private float vieMaximale;

    // La vie actuelle du personnage
    private float vie;


    #endregion

    #region Drapeaux pour la gestion des etats
    /// <summary>
    /// Drapeau pour indiquer qu'on attaque
    /// </summary>
    public bool AttaqueEnCours { get; private set; }

    /// <summary>
    /// Drapeau pour indiquer que l'on cours
    /// </summary>
    private bool enCourse;

    /// <summary>
    /// Drapeau pour indiquer que l'on tourne
    /// </summary>
    private bool enRotation;

    /// <summary>
    /// Drapeau pour indiquer un déplacement. Un déplacement est une course ou une rotation
    /// </summary>
    public bool EnDeplacement => enCourse || enRotation;

    #endregion

    #region Gestion de l'attaque
    [SerializeField, Tooltip("Temps pour l'animation et le délai entre les attaques")]
    private float tempsAttaque;
    #endregion

    #region Gestion des états

    // Etat de la frame précédente
    private EtatPersonnage etatPrecedent;

    // État de la frame suivante (ou actuelle)
    private EtatPersonnage etatSuivant;

    #endregion

    // Exécuter à la première frame
    private void Awake()
    {
        controleurAnimation = GetComponent<Animator>();
        vie = vieMaximale;
        etatSuivant = new EtatAttente();
    }

    /// <summary>
    /// Gestionnaire du contrôle de déplacement pour le personnage.
    /// </summary>
    /// <param name="context">Les données de l'événement</param>
    public void OnDeplacement(InputAction.CallbackContext context)
    {
        controleDeplacement = context.ReadValue<Vector2>();

        // On se déplace
        /*if (controleDeplacement.sqrMagnitude > 0.01f)
        {
            EnDeplacement = true;
        }
        else
        {
            EnDeplacement = false;
        }*/
        enCourse = controleDeplacement.sqrMagnitude > 0.01f;
    }

    /// <summary>
    /// Gestionnaire du contrôle de rotation pour le personnage.
    /// </summary>
    /// <param name="context">Les données de l'événement</param>
    public void OnRotation(InputAction.CallbackContext context)
    {
        controleRotation = context.ReadValue<float>();

        enRotation = !Mathf.Approximately(controleRotation, 0.0f);
    }

    /// <summary>
    /// Permet de déclencher une attaque
    /// </summary>
    /// <param name="context">Les données de l'événement</param>
    public void OnAttaque(InputAction.CallbackContext context)
    {
        // Lorsque l'action est en mode « performed » on lance le trigger
        if (context.performed)
        {
            AttaqueEnCours = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(etatPrecedent != etatSuivant)
        {
            etatSuivant.EntrerEtat(this);
        }
        etatPrecedent = etatSuivant;
        etatSuivant = etatSuivant.ExecuterEtat(this);

        if(etatPrecedent != etatSuivant)
        {
            etatPrecedent.SortirEtat(this);
        }
    }

    /// <inheritdoc/>
    public void RecevoirFrappe(Arme arme)
    {
        vie -= arme.Degat;
        barreVie.SetProgression(vie / vieMaximale);
    }

    /// <summary>
    /// Permet de démarrer les animations et autres comportements du personnage lorsqu'il
    /// se déplace
    /// </summary>
    public void DemarrerDeplacement()
    {
        controleurAnimation.SetBool("Courir", true);
    }

    /// <summary>
    /// Applique le déplacement donné par les contrôles actifs
    /// </summary>
    public void Deplacer()
    {
        // Code anciennement dans le Update

        // === Deplacement ===
        // Remappe le deplacement 2D (contrôle) en déplacement 3D pour l'espace
        Vector3 deplacement = controleDeplacement.y * transform.forward +
            controleDeplacement.x * transform.right;

        // Applique le déplacement
        deplacement = vitesse * Time.deltaTime * deplacement.normalized;
        transform.position += deplacement;

        // === Rotation ===

        //transform.Rotate(vitesseRotation * Time.deltaTime * controleRotation * Vector3.up));

        Quaternion rotation = Quaternion.Euler(0.0f, controleRotation * Time.deltaTime * vitesseRotation, 0.0f);
        transform.rotation *= rotation;
    }

    /// <summary>
    /// Arrête le traitement du déplacement.
    /// </summary>
    public void ArreterDeplacement()
    {
        controleurAnimation.SetBool("Courir", false);
    }

    /// <summary>
    /// Permet de déclencher une attaque.
    /// </summary>
    public void Attaquer()
    {
        StartCoroutine(CoroutineDelaiAttaque());
    }

    /// <summary>
    /// Gère l'application du délai de l'attaque et d'immobilité suivant l'attaque.
    /// </summary>
    /// <returns>Enumerator de coroutine</returns>
    public IEnumerator CoroutineDelaiAttaque()
    {
        arme.CommencerAttaque();
        GetComponent<Animator>().SetTrigger("Attaquer");
        
        yield return new WaitForSeconds(tempsAttaque);
        AttaqueEnCours = false;
    }
}
