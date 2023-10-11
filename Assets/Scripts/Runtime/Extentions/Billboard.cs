using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using Cinemachine;

public class Billboard : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private RectTransform rectTransform; 

    private void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        rectTransform = GetComponent<RectTransform>();  
    }

    private void LateUpdate()
    {
        if (rectTransform != null && virtualCamera != null)
        {
            
            Transform virtualCameraTransform = virtualCamera.transform;

            rectTransform.LookAt(
                rectTransform.position + virtualCameraTransform.rotation * Vector3.forward,
                virtualCameraTransform.rotation * Vector3.up
            );
        }
    }
}