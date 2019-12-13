using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwapCamera : MonoBehaviour
{
    /// <summary>
    /// Changer de caméra lorsque 1 objet (player) entre dans une zone à surveiller
    /// </summary>
    CinemachineBrain cinemachineBrain;      // le cerveau de cinemachine
    CinemachineVirtualCamera mainCam;       // la caméra active AVANT d'entrer dans la zone
    CinemachineVirtualCamera localCam;      // la caméra à activer pour la zone
    Collider m_Collider;                    // la zone à surveiller
    GameObject player;                      // l'objet à surveiller

    // Start is called before the first frame update
    void Start()
    {
        // initialisation des variables
        cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
        localCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        m_Collider = gameObject.GetComponentInChildren<Collider>();
        player = PlayerControl.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // récupérer la caméra active
        var cam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (cam != localCam) mainCam = cam;
        /// /!\ le collider doit être désactivé pour permettre de naviguer vers l'intérieur de la zone
        /// cela impose de le réactiver à chaque frame, contrôler la présence du player dans le collider, puis désactiver le collider pour autoriser la navigation
        m_Collider.enabled = true;
        // si le player est dans la zone => activer la caméra locale et désactiver la caméra principale
        localCam.gameObject.SetActive(m_Collider.bounds.Contains(player.transform.position));       
        mainCam?.gameObject.SetActive(!localCam.gameObject.activeInHierarchy);
        // réactiver le collider our autoriser la navigation
        m_Collider.enabled = false;
    }
}
