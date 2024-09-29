using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject destroyedParticlesPrefab;
    public int size = 3;

    private void Start()
    {
        transform.localScale = Vector3.one * (size * 0.5f);

        var rb = GetComponent<Rigidbody2D>();
        var direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
        GameManager.Instance.AsteroidCount++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.AsteroidCount--;
            Destroy(other.gameObject);
            if (size > 1)
            {
                for (int i = 0; i < 2; ++i)
                {
                    var asteroid = Instantiate(this, transform.position, Quaternion.identity);
                    asteroid.size = size - 1;
                }
            }

            Instantiate(destroyedParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}