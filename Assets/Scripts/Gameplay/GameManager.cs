using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;

    public int asteroidCount = 0;

    private int _level = 0;

    private void Update()
    {
        if (asteroidCount == 0)
        {
            _level++;
            var numAsteroids = 2 + (2 * _level);
            for (var i = 0; i < numAsteroids; ++i) SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
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
        var asteroid = Instantiate(asteroidPrefab, worldSpawnPostion, Quaternion.identity);
        asteroid.gameManager = this;
    }

    public void GameOver() { StartCoroutine(IeRestart()); }

    private IEnumerator IeRestart()
    {
        Debug.Log("Game Over");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}