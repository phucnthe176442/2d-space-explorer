using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float delaySpawnStar = 5f;
    [SerializeField] private Star starPrefab;
    [SerializeField] private Asteroid asteroidPrefab;

    private int _score;
    public event Action<int> ScoreChanged;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            if (BestScore < _score) BestScore = _score;
            ScoreChanged?.Invoke(value);
        }
    }

    public int BestScore { get => PlayerPrefs.GetInt("BestScore", 0); set => PlayerPrefs.SetInt("BestScore", value); }

    private int _asteroidCount;

    public int AsteroidCount
    {
        get => _asteroidCount;
        set
        {
            if (_playing) _asteroidCount = value;
        }
    }

    private int _level = 0;
    private float _lastTimeSpawnStar;
    private bool _playing = false;

    private static GameManager instance;
    public static GameManager Instance => instance ??= FindObjectOfType<GameManager>();

    private void Awake()
    {
        _playing = true;
        DontDestroyOnLoad(gameObject);
        if (instance != null && instance != this) Destroy(gameObject);
        else if (instance == null) instance = this;
    }

    private void Update()
    {
        if (!_playing) return;
        if (Time.time - _lastTimeSpawnStar > delaySpawnStar)
        {
            _lastTimeSpawnStar = Time.time;
            var star = Instantiate(starPrefab, SpawnPosition(), Quaternion.identity);
        }

        if (AsteroidCount == 0)
        {
            _level++;
            var numAsteroids = 2 + (2 * _level);
            for (var i = 0; i < numAsteroids; ++i) Instantiate(asteroidPrefab, SpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 SpawnPosition()
    {
        var offset = Random.Range(0f, 1f);
        var viewportSpawnPositon = Vector2.zero;
        int edge = Random.Range(0, 4);
        viewportSpawnPositon = edge switch
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

    public void GameOver() { LoadScene(Constant.END_SCENE, () => { AsteroidCount = 0; _playing = false; }); }

    public void Replay()
    {
        LoadScene(Constant.GAME_SCENE,
            () =>
            {
                _playing = true;
                AsteroidCount = 0;
                _level = 0;
                Score = 0;
            });
    }

    public void GoHome()
    {
        LoadScene(Constant.MENU_SCENE,
            () =>
            {
                AsteroidCount = 0;
                _playing = false;
            });
    }

    private async void LoadScene(string name, Action callback = null) { StartCoroutine(LoadYourAsyncScene(name, callback)); }

    private IEnumerator LoadYourAsyncScene(string name, Action callback = null)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
        }

        callback?.Invoke();
    }
}