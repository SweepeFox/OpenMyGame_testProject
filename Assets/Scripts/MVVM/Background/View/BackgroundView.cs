using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof (RectTransform))]
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

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _balloons.ForEach(balloon => {
            InitBalloon(balloon);
        });
    }

    private void InitBalloon(BalloonView balloon)
    {
        balloon.Init(_balloonSprites[Random.Range(0, _balloonSprites.Count)], Random.Range(_minBalloonScale, _maxBalloonScale));

        var balloonRectWidthHalf = balloon.rectTransform.rect.width * balloon.transform.localScale.x / 2;
        var balloonRectHeightHalf = balloon.rectTransform.rect.height * balloon.transform.localScale.y / 2;

        var leftSpawnPositionX = -_rectTransform.rect.width / 2 - balloonRectWidthHalf;
        var rightSpawnPositionX = _rectTransform.rect.width / 2 + balloonRectWidthHalf;
        var isLeft = Random.Range(0, 2) == 0;

        balloon.rectTransform.anchoredPosition = new Vector2(
            x: isLeft ? leftSpawnPositionX : rightSpawnPositionX,
            y: Random.Range(0, _rectTransform.rect.height / 2 - balloonRectHeightHalf)
        );

        var targetPositionX = isLeft ? rightSpawnPositionX : leftSpawnPositionX;
        var moveDuration = Random.Range(_minBalloonMoveDuration, _maxBalloonMoveDuration);
        var moveAmplitude = Random.Range(_minBalloonAmplitude, _maxBalloonAmplitude);
        var moveFrequency = Random.Range(_minBalloonFrequency, _maxBalloonFrequency);

        var initialAnchoredY = balloon.rectTransform.anchoredPosition.y;
        balloon.rectTransform.DOAnchorPosX(targetPositionX, moveDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => {
                balloon.rectTransform.anchoredPosition = new Vector2(
                    balloon.rectTransform.anchoredPosition.x,
                    initialAnchoredY + Mathf.Sin(Time.time * moveFrequency) * moveAmplitude
                );
            })
            .OnComplete(() => {
                InitBalloon(balloon);
            });
    }

    private void OnDestroy()
    {
        _balloons.ForEach(balloon => {
            DOTween.Kill(balloon.rectTransform);
        });
    }
}