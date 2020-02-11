using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb = null;
    [SerializeField]
    float initialSpeed = 5.0f;
    [SerializeField]
    int damage = 1;

    float currentSpeed;
    Vector2 directionStock;

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = initialSpeed;
        rb.velocity = transform.up * initialSpeed;

        GameManager.Instance.OnGameOver.AddListener(DestroyProjectile);
        GameManager.Instance.OnGamePause.AddListener(PauseProjectile);
        GameManager.Instance.OnGameContinue.AddListener(OnContinueProjectile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Enter colision with: " + collision.transform.name);

        if(collision.transform.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }

        if (collision.transform.CompareTag("Block"))
        {
            Block block = collision.transform.GetComponent<Block>();

            if (block != null)
            {
                block.ReceivedDamage(this, damage);
            }
        }
    }
    #endregion

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    void PauseProjectile()
    {
        directionStock = rb.velocity;
        rb.velocity = Vector2.zero;
    }

    void OnContinueProjectile()
    {
        rb.velocity = directionStock;
    }
}