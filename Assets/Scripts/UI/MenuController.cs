using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MenuController : MonoBehaviour
{
    [SerializeField] private RectTransform[] uis;
    [SerializeField, Space] private int maxStars = 20;
    [SerializeField] private float delaySpawnStar = 2f;
    [SerializeField] private GameObject starPrefab;

    private float _lastTimeSpawnStar, _spawned;

    private void Update()
    {
        if (Time.time - _lastTimeSpawnStar > delaySpawnStar && _spawned < maxStars)
        {
            _spawned++;
            _lastTimeSpawnStar = Time.time;
            var star = Instantiate(starPrefab, SpawnPosition(), Quaternion.identity);
            star.transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Vector3.up);
        }
    }

    private Vector3 SpawnPosition()
    {
        var offset = Random.Range(0f, 1f);
        int edge = Random.Range(0, 4);
        var viewportSpawnPositon = edge switch
        {
            0 => new Vector2(offset, 0),
            1 => new Vector2(offset, 1),
            2 => new Vector2(0, offset),
            3 => new Vector2(1, offset)
        };
        var worldSpawnPostion = Camera.main.ViewportToWorldPoint(viewportSpawnPositon);
        worldSpawnPostion.z = 0f;
        return worldSpawnPostion;
    }

    private void FixedUpdate()
    {
        foreach (var rectTransform in uis)
            rectTransform.gameObject.SetActive(!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
                                                 Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)));
    }

    public void OnPlayClicked()
    {
        var ao = SceneManager.LoadSceneAsync(Constant.GAME_SCENE);
        if (ao != null) ao.allowSceneActivation = true;
    }
}