using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float throwRate = 1f;
    [SerializeField] float boundaryX = 4;
    [SerializeField] Vector3 spawnerDefaultPosition;
    [SerializeField] Transform itemSpawnPoint;



    void Start()
    {
        transform.position = spawnerDefaultPosition;
    }

    
    void Update()
    {
        Movement();
    }
    

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(horizontal * speed * Time.deltaTime, 0, 0);

        Vector3 newPosition = transform.position + movement;


        if (newPosition.x > boundaryX)
        {
            newPosition.x = boundaryX;
        }
        else if (newPosition.x < -boundaryX)
        {
            newPosition.x = -boundaryX;
        }

        transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
    }
   
    public Transform GetItemSpawnPoint()
    {
        return itemSpawnPoint;
    }
   
   public float GetThrowRate()
    {
        return throwRate;
    }
}
