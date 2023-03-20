using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class menumanager : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;

    private void Start()
    {
        currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;
    }
}
