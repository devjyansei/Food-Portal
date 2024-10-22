using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Item : MonoBehaviour
{
    public itemSO itemSO;
    [SerializeField] GameObject creationEffect;

    bool isExploded = false;
    Rigidbody rigidBody;
    public float speed = 5f;
    bool canExplode = false;
    public bool createdByExplosion = false;
    bool isInGameoverTrigger;
    bool isThrowed = false;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (!createdByExplosion)
        {
            SetVelocityZero();
        }
        if (createdByExplosion)
        {
            transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            AddRandomForce();
            creationEffect.gameObject.SetActive(true);
            
        }
        StartCoroutine("ExplodeOpener");

    }
  
    private void AddRandomForce()
    {
        float forcePower = Random.Range(1, 4);
        rigidBody.AddForce(Vector3.up * forcePower, ForceMode.Impulse);

    }
    IEnumerator ExplodeOpener()
    {
        yield return new WaitForSeconds(.05f);
        if (createdByExplosion)
        {
            OpenGravity();
        }
        canExplode = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Item"))
        {
            Vector3[] forceDirections = { Vector3.right, Vector3.left };
            Vector3 randomDirection = forceDirections[Random.Range(0, forceDirections.Length)];

            rigidBody.AddForce(randomDirection / 2, ForceMode.Impulse);
        }


        if (collision.collider.TryGetComponent<Item>(out Item itemComponent))
        {
            
            if (itemComponent.itemSO != null && this.itemSO != null) //null check
            {
                if (itemComponent.itemSO.level == this.itemSO.level)
                {

                    Vector3 collisionPoint = collision.contacts[0].point; // carpisma noktasi
                    if (collisionPoint.y < 0.5)
                    {
                        collisionPoint.y = 0.5f;
                    }

                    GameManager.Instance.Explode(this, itemComponent,collisionPoint);
                }
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameOverTrigger"))
        {
            if (isThrowed || createdByExplosion)
            {
                isInGameoverTrigger = true;
                StartCoroutine("GameoverTimer");
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameOverTrigger"))
        {
            isInGameoverTrigger = false;
        }
    }
    IEnumerator GameoverTimer()
    {
        yield return new WaitForSeconds(2);
        if (isInGameoverTrigger)
        {
            GameManager.Instance.GameOver();
        }
    }
    public void OpenGravity()
    {
        rigidBody.useGravity = true;
    }
    public void SetVelocityZero()
    {
        rigidBody.velocity = Vector3.zero;
    }
    public void SetIsThrowed(bool state)
    {
        isThrowed = state;
    }
    public bool GetCreatedByExplosion()
    {
        return createdByExplosion;
    }
    public void SetCreatedByExplosion(bool state)
    {
        createdByExplosion = state;
    }
    public bool GetIsExploded()
    {
        return isExploded;
    }
    public void SetIsExploded(bool state)
    {
        isExploded = state;
    }
}
