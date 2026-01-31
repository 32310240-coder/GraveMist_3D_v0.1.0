using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject dragArea;

    public enum GameState
    {
        Idle,
        Shake,
        Cooldown
    }

    public GameState currentState = GameState.Idle;

    void Start()
    {
        EnterIdle();
    }

    // Playボタンから呼ばれる
    public void OnPlayButtonPressed()
    {
        if (currentState != GameState.Idle) return;
        EnterShake();
    }

    void EnterShake()
    {
        currentState = GameState.Shake;
        playButton.SetActive(false);
        dragArea.SetActive(true);
    }

    public void OnDragEnd()
    {
        if (currentState != GameState.Shake) return;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        currentState = GameState.Cooldown;

        // ドラッグ不可にする
        dragArea.GetComponent<DragAreaController>().SetDraggable(false);

        float timer = 3f;
        while (timer > 0)
        {
            Debug.Log($"Countdown: {Mathf.Ceil(timer)}");
            timer -= Time.deltaTime;
            yield return null;
        }

        EnterIdle();
    }

    void EnterIdle()
    {
        currentState = GameState.Idle;
        playButton.SetActive(true);
        dragArea.SetActive(false);

        // 次回のためにリセット
        dragArea.GetComponent<DragAreaController>().SetDraggable(true);
    }
}
