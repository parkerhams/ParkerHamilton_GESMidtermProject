using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour {
    public static event Action PlayerRespawnedFromCheckpoint;
    private void Respawn(Transform playerTransform)
    {
        if (Checkpoint.currentlyActiveCheckpoint == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            
            playerTransform.position = Checkpoint.currentlyActiveCheckpoint.transform.position;

            if (PlayerRespawnedFromCheckpoint != null)
            {
                PlayerRespawnedFromCheckpoint.Invoke();
            }
        }
    }
    void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
            Respawn(other.transform);
		}
	}
}
