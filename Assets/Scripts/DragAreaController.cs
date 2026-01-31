using UnityEngine;

public class DragAreaController : MonoBehaviour
{
    public GameManager gameManager;

    private bool dragging = false;
    private bool draggable = true;
    private Vector3 lastPoint;

    public void SetDraggable(bool value)
    {
        draggable = value;
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleTouch()
    {
        if (!draggable || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.collider.gameObject != gameObject) return;

        if (touch.phase == TouchPhase.Began)
        {
            dragging = true;
            lastPoint = hit.point;
            Debug.Log("ドラッグ開始");
        }
        else if (touch.phase == TouchPhase.Ended && dragging)
        {
            dragging = false;
            Debug.Log("ドラッグ終了");
            gameManager.OnDragEnd();
        }
    }

    void HandleMouse()
    {
        if (!draggable) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject == gameObject)
            {
                dragging = true;
                lastPoint = hit.point;
                Debug.Log("ドラッグ開始");
            }
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            Debug.Log("ドラッグ終了");
            gameManager.OnDragEnd();
        }
    }
}
