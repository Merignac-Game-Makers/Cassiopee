using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

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


    bool isInside = false;
    CinemachineVirtualCamera cam;

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
        isInside = m_Collider.bounds.Contains(player.transform.position);

        if (isInside) {
            // récupérer la caméra active
            cam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (cam != localCam) mainCam = cam;
            localCam.gameObject.SetActive(true);
            mainCam?.gameObject.SetActive(false);
        } else if (cam == localCam) {
            localCam.gameObject.SetActive(false);
            mainCam?.gameObject.SetActive(true);
        }

      // si le player est dans la zone => activer la caméra locale et désactiver la caméra principale
        //localCam.gameObject.SetActive(m_Collider.bounds.Contains(player.transform.position));
        //mainCam?.gameObject.SetActive(!localCam.gameObject.activeInHierarchy);
    }

    private void OnCollisionEnter(Collision collision) {
        PlayerControl.Instance.m_Agent.destination = gameObject.transform.position;
    }
}
