using UnityEngine;

/// <summary>
/// Permet à un objet de toujours être lisible par la caméra en pointant dans la même direction que celle-ci.
/// Aligne l'axe forward dans le plan XZ de la caméra.
/// </summary>
public class FaceACamera : MonoBehaviour
{
    // Camera du jeu vers laquelle l'objet doit toujours faire face.
    private GameObject cameraJeu;

    void Start()
    {
        // Récupère la caméra principale du jeu
        cameraJeu = FindFirstObjectByType<Camera>().gameObject;
    }

    private void LateUpdate()
    {
        // Met à jour l'orientation de l'objet regarder dans le sens de la caméra
        transform.LookAt(transform.position + cameraJeu.transform.rotation * Vector3.forward,
                        cameraJeu.transform.rotation * Vector3.up);
        
    }
}
