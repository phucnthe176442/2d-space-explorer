using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        private void OnEnable() { GameManager.Instance.ScoreChanged += OnChanged; }
        private void OnDisable() { GameManager.Instance.ScoreChanged -= OnChanged; }
        private void OnChanged(int score) { scoreText.text = $"Score: {score}"; }
    }
}