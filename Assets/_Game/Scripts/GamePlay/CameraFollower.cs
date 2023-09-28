using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{
    public Transform TF;
    public Transform playerTF;

    [SerializeField] Vector3 offset;
    [SerializeField] float minX = -10f; 
    [SerializeField] float maxX = 10f;  
    [SerializeField] float minY = -5f; // Giới hạn đáy
    [SerializeField] float maxY = 5f;  // Giới hạn đỉnh

    private void LateUpdate()
    {
        if(!GameManager.Instance.IsState(GameState.GamePlay))
            return;
        Vector3 targetPosition = playerTF.position + offset;

        // Giới hạn camera theo trục x
        if (targetPosition.x < minX)
        {
            targetPosition.x = minX; 
        }
        else if (targetPosition.x > maxX)
        {
            targetPosition.x = maxX; 
        }

        // Giới hạn camera theo trục y
        if (targetPosition.y < minY)
        {
            targetPosition.y = minY; 
        }
        else if (targetPosition.y > maxY)
        {
            targetPosition.y = maxY; 
        }

        TF.position = Vector3.Lerp(TF.position, targetPosition, Time.deltaTime * 5f);
    }
}







