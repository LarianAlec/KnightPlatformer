using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;

public class EscortHeroComponent : MonoBehaviour
{
    [SerializeField] float offsetY = 0.0f;
    private Transform playerPosition;
    

    private void Awake()
    {
       if (playerPosition == null)
        {
            playerPosition = GameObject.FindWithTag("Player").transform;
        }
        Vector3 modifiedPosition = playerPosition.position;
        modifiedPosition.y = offsetY;
        gameObject.transform.position = modifiedPosition;
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y + offsetY, playerPosition.position.z);
    }
}
