using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundView : MonoBehaviour
{
    [SerializeField] private List<BalloonView> _balloons;
    [SerializeField] private List<Sprite> _balloonSprites;

    [Space]
    [SerializeField] private float _minBalloonScale = 0.5f;
    [SerializeField] private float _maxBalloonScale = 1.5f;

    [Space]
    [SerializeField] private float _minBalloonMoveDuration;
    [SerializeField] private float _maxBalloonMoveDuration;

    [Space]
    [SerializeField] private float _minBalloonAmplitude = 10f;
    [SerializeField] private float _maxBalloonAmplitude = 100f;

    [Space]
    [SerializeField] private float _minBalloonFrequency = 1f;
    [SerializeField] private float _maxBalloonFrequency = 10f;

    private void Start()
    {
        _balloons.ForEach(balloon => {
            InitBalloon(balloon);
        });
    }

    private void InitBalloon(BalloonView balloon)
    {
        balloon.Init(_balloonSprites[Random.Range(0, _balloonSprites.Count)], Random.Range(_minBalloonScale, _maxBalloonScale));

        var leftSpawnPositionX = -balloon.rectTransform.rect.width / 2 * balloon.transform.localScale.x;
        var rightSpawnPositionX = Screen.width - leftSpawnPositionX;
        var isLeft = Random.Range(0, 2) == 0;

        balloon.rectTransform.position = new Vector2(
            x: isLeft ? leftSpawnPositionX : rightSpawnPositionX,
            y: Random.Range(Screen.height / 2, Screen.height - balloon.rectTransform.rect.height)
        );

        var targetPositionX = isLeft ? rightSpawnPositionX : leftSpawnPositionX;
        var moveDuration = Random.Range(_minBalloonMoveDuration, _maxBalloonMoveDuration);
        var moveAmplitude = Random.Range(_minBalloonAmplitude, _maxBalloonAmplitude);
        var moveFrequency = Random.Range(_minBalloonFrequency, _maxBalloonFrequency);

        balloon.rectTransform.DOMoveX(targetPositionX, moveDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => {
                balloon.rectTransform.anchoredPosition = new Vector2(
                    balloon.rectTransform.anchoredPosition.x,
                    Mathf.Sin(Time.time * moveFrequency) * moveAmplitude
                );
            })
            .OnComplete(() => {
                InitBalloon(balloon);
            });
    }
}