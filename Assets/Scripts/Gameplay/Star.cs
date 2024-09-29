using UnityEngine;

namespace DefaultNamespace.Gameplay
{
    public class Star : MonoBehaviour
    {
        private void Start()
        {
            var rb = GetComponent<Rigidbody2D>();
            var direction = new Vector2(Random.value, Random.value).normalized;
            float spawnSpeed = Random.Range(2f, 3f);
            rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.Instance.Score++;
            Destroy(gameObject);
        }
    }
}