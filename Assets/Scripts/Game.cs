using System.Collections;
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

    private static readonly Vector3 EMPTY_IMPUT = new();

    private GameObject currentPencil;

    private int currentArrow;

    private void Start()
    {
        gameTimer.StartTimer();
        StartCoroutine(ChangeArrow());
        currentPencil = Instantiate(pencilPrefab, pencilsContainer.transform);
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

        if (direction == EMPTY_IMPUT)
            return;

        if (direction != arrows[currentArrow].Direction)
        {
            arrowsStrike = 0;
            return;
        }

        arrowsStrike += strikeBoost;
        Debug.Log(arrowsStrike);
        var position = currentPencil.transform.position;

        position.y += pencilSpeed + (float)arrowsStrike / 10;
        currentPencil.transform.position = position;
    }

    private void HideArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.GameObject.SetActive(false);
        }
    }
}