using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField] private List<Image> _balloons;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private void Start()
    {
        _balloons.ForEach(balloon => {
            InitBalloon(balloon);
        });
    }

    private void InitBalloon(Image balloon)
    {
        balloon.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        balloon.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        balloon.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        // random -1 or 1
        var leftSpawnPositionX = -balloon.rectTransform.rect.width / 2 * balloon.transform.localScale.x;
        var rightSpawnPositionX = Screen.width - leftSpawnPositionX;
        var isLeft = Random.Range(0, 2) == 0;

        balloon.rectTransform.position = new Vector2(
            x: isLeft ? leftSpawnPositionX : rightSpawnPositionX,
            y: Random.Range(balloon.rectTransform.rect.height, Screen.height - balloon.rectTransform.rect.height)
        );

        var targetPositionX = isLeft ? rightSpawnPositionX : leftSpawnPositionX;
        balloon.rectTransform.DOMoveX(targetPositionX, Random.Range(_minSpeed, _maxSpeed)).SetEase(Ease.Linear).OnComplete(() => {
            InitBalloon(balloon);
        });
    }
}