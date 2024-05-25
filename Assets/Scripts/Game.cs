using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject pencilsContainer;
    [SerializeField] private GameObject pencilPrefab;
    [SerializeField] private Arrow[] arrows;

    [SerializeField] private int arrowsStrike = 0;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private int pencilSpeed;
    [SerializeField] private int strikeBoost;
    [SerializeField] private int pencilsCount;
    [SerializeField] private Vector3 pencilsOffset;
    [SerializeField] private Vector3 spawnPosition;

    [SerializeField] private GameType gameType;
    [SerializeField] private int lives;
    private int _lives;

    [SerializeField] private float endPositionY;

    private List<GameObject> pencils;

    private int currentArrow;

    private void Start()
    {
        _lives = lives;

        pencils = new List<GameObject>();
        CreatePencils();

        gameTimer.StartTimer();
        StartCoroutine(ChangeArrow());
    }

    private void CreatePencils()
    {
        var currentPencilOffset = new Vector3();
        for (int i = 0; i < pencilsCount; i++)
        {
            var pencil = Instantiate(pencilPrefab, pencilsContainer.transform);

            pencil.transform.localPosition = spawnPosition + currentPencilOffset;
            currentPencilOffset += pencilsOffset;

            pencils.Add(pencil);
        }
    }

    private IEnumerator ChangeArrow()
    {
        while (true)
        {
            HideArrows();
            currentArrow = Random.Range(0, arrows.Length);
            arrows[currentArrow].GameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.5f, 2));
        }
    }

    public void OnSlide(InputValue inputValue)
    {
        var direction = inputValue.Get<Vector3>();
        Debug.Log(direction);

        if (direction == Vector3.zero)
            return;

        if (direction != arrows[currentArrow].Direction)
        {
            _lives--;
            arrowsStrike = 0;
            return;
        }

        arrowsStrike += strikeBoost;
        Debug.Log(arrowsStrike);

        for (int i = 0; i < pencils.Count; i++)
        {
            var currentPencil = pencils[i];
            var position = currentPencil.transform.position;

            position.y += pencilSpeed + (float)arrowsStrike / 10;
            currentPencil.transform.position = position;

            if (currentPencil.transform.localPosition.y < endPositionY) continue;

            Destroy(currentPencil);
            pencils.Remove(currentPencil);

            if (pencils.Count < 1 || (gameType == GameType.WithLives && _lives < 1))
            {
                OnGameEnd();
            }
        }
    }

    private void HideArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.GameObject.SetActive(false);
        }
    }

    private void OnGameEnd()
    {
    }
}