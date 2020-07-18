using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string sceneToLoad;
    public Vector3 playerLocation;
    public Vector3 playerWalkToLocation;
    public VectorValue playerStoredLocation;
    public VectorValue playerStoredWalkToLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Mario")
        {
            playerStoredLocation.initialValue = playerLocation;
            playerStoredWalkToLocation.initialValue = playerWalkToLocation;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
