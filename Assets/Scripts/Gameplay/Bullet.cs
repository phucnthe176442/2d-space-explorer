using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;

    private void Awake() { Destroy(gameObject, lifeTime); }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}