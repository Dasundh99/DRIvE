using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    CarController playerMovement;

    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<CarController>();
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if  (collision.gameObject.name == "player")
        {
            playerMovement.Die();
        }  
            
    }



    // Update is called once per frame
    private void Update()
    {
        
    }
}
