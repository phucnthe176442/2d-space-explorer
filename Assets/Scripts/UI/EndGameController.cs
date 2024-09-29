using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bestScoreText;

        private void OnEnable() { bestScoreText.text = $"Best Score: {GameManager.Instance.BestScore}"; }
        public void OnClickHome() => GameManager.Instance.GoHome();
        public void OnClickReplay() => GameManager.Instance.Replay();
    }
}