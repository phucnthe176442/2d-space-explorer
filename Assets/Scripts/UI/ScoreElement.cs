using System.Collections.Generic;
using DefaultNamespace.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private RectTransform[] numbers;

        private Dictionary<GameObject, List<GameObject>> _deactived = new();

        private void OnEnable() { GameManager.Instance.ScoreChanged += OnChanged; }
        private void OnDisable() { GameManager.Instance.ScoreChanged -= OnChanged; }

        private void OnChanged(int score)
        {
            for (var i = 0; i < container.childCount; ++i)
            {
                if (container.GetChild(i).TryGetComponent(out PooledObjectId pooledObjectId))
                {
                    if (!_deactived.ContainsKey(pooledObjectId.prefab)) _deactived.Add(pooledObjectId.prefab, new List<GameObject>());
                    if (!_deactived.TryGetValue(pooledObjectId.prefab, out List<GameObject> list)) return;
                    list.Add(pooledObjectId.gameObject);
                    pooledObjectId.gameObject.SetActive(false);
                }
            }

            if (score == 0) Instantiate(numbers[0], container);
            while (score > 0)
            {
                if (!_deactived.TryGetValue(numbers[score % 10].gameObject, out var list))
                {
                    var x = Instantiate(numbers[score % 10], container);
                    x.SetSiblingIndex(0);
                    x.AddComponent<PooledObjectId>().prefab = numbers[score % 10].gameObject;
                }
                else
                {
                    list[0].gameObject.SetActive(true);
                    list.Remove(list[0].gameObject);
                }

                score /= 10;
            }
        }
    }
}