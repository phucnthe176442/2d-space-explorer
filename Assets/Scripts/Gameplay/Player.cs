using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ship Parameters"), SerializeField] private float shipAcceleration;
    [SerializeField] private float shipMaxVelocity;
    [SerializeField] private float shipRotationSpeed;
    [SerializeField] private float bulletSpeed;

    [Header("Object References"), SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private GameObject destroyedParticlesPrefab;

    private Rigidbody2D _rigidbody2D;
    private bool _isAlive = true, _isAcceleration = false;

    // Start is called before the first frame update
    private void Start() { _rigidbody2D = GetComponent<Rigidbody2D>(); }

    // Update is called once per frame
    private void Update()
    {
        if (_isAlive)
        {
            HandleShipAcceleration();
            HandleShipRotation();
            HandlerShooting();
        }
    }

    private void HandlerShooting()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        var shipVelocity = _rigidbody2D.velocity;
        var shipDirection = transform.up;
        float shipForwardSpeed = Vector2.Dot(shipDirection, shipVelocity);

        if (shipForwardSpeed < 0) shipForwardSpeed = 0;

        bullet.velocity = shipDirection * shipForwardSpeed;
        bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (_isAlive && _isAcceleration)
        {
            _rigidbody2D.AddForce(shipAcceleration * transform.up);
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
    }

    private void HandleShipAcceleration() { _isAcceleration = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            _isAlive = false;
            var gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
            Instantiate(destroyedParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}