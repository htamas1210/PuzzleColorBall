using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class playbutton : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public CinemachineTrackedDolly cinemachineTrackedDolly;
    public CinemachinePathBase.PositionUnits m_PositionUnits;

    public CinemachineTrackedDolly dolly;    

    public CinemachineSmoothPath smoothPath;
    public CinemachinePath utvonal;
    public float pathPosition = 0.0f;
    public float speed = 0.2f;
    public float pathLength;
    public GameObject playButton;
    public Transform cameraTransform;
    public Vector3 cameraTransformOriginal;


    private void Start()
    {
        //CinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>;
        pathPosition = 0.0f;
        cinemachineTrackedDolly = currentCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        pathLength = utvonal.MaxPos;
    }



    public void PlayButton()
    {
        playButton.SetActive(false);
        if (pathPosition < pathLength)
        {
            pathPosition += speed;
            cinemachineTrackedDolly.m_PathPosition = pathPosition;
            //smoothPath.m_PathPosition = pathPosition;
        }
    }
}
