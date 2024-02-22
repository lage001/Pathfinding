using UnityEngine;
using UnityEngine.InputSystem;

public class CameraC : MonoBehaviour
{

    public Vector2 range;
    public float scrollSpeed;
    public float moveSpeed;

    Vector2 mousePos;

    Vector3 lastPos;
    Camera c;

    float scrollDir;
    float smoothSpeed = 0.125f;

    bool follow;
    void Start()
    {
        c = Camera.main;
    }

    void Update()
    {
        UpdateMapScale();
        DragMap();
    }
    private void OnEnable()
    {
        GameManager.Instance.playerInput.CameraControl.MousePos.performed += OnMouseMove;
        GameManager.Instance.playerInput.CameraControl.MouseScroll.performed += OnScrollMove;
        GameManager.Instance.playerInput.CameraControl.MouseScroll.canceled += OnScrollMove;
    }
    private void OnDisable()
    {
        GameManager.Instance.playerInput.CameraControl.MousePos.performed -= OnMouseMove;
        GameManager.Instance.playerInput.CameraControl.MouseScroll.performed -= OnScrollMove;
        GameManager.Instance.playerInput.CameraControl.MouseScroll.canceled -= OnScrollMove;
    }


    void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    void OnScrollMove(InputAction.CallbackContext ctx)
    {
        
        scrollDir = ctx.ReadValue<Vector2>().y/120;
    }
    void UpdateMapScale()
    {
        if (scrollDir != 0)
        {
            c.orthographicSize -= scrollDir * scrollSpeed;
            c.orthographicSize = Mathf.Clamp(c.orthographicSize, range.x, range.y);
            Vector3 direction;
            if (scrollDir > 0)
            {
                if (c.orthographicSize > range.x)
                {
                    Vector3 mouseWorldPos = c.ScreenToWorldPoint(mousePos);
                    direction = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0) - new Vector3(transform.position.x, transform.position.y, 0);
                    c.transform.Translate(direction / c.orthographicSize);
                }
            }
            else if (c.orthographicSize < range.y)
            {
                direction = (new Vector3(0, 0, 0) - new Vector3(transform.position.x, transform.position.y, 0));
                c.transform.Translate(direction / (54 - c.orthographicSize));
            }
        }
    }
    void DragMap()
    {
        if (GameManager.Instance.playerInput.CameraControl.MouseScrollButton.IsPressed())
        {
            Vector3 currentPos = mousePos;
            if(lastPos != default)
            {
                Vector3 direction = c.ScreenToWorldPoint(currentPos) - c.ScreenToWorldPoint(lastPos);
                Vector2Int boolVector = InBound(transform.position - direction);
                if (boolVector.x * boolVector.y == 0)
                {
                    transform.Translate(Vector3.zero);
                }
                else
                {
                    transform.Translate(-direction);
                }
            }
            lastPos = currentPos;
        }
        if (!GameManager.Instance.playerInput.CameraControl.MouseScrollButton.IsPressed())
        {
            lastPos = default;
        }
    }
    public Vector2Int InBound(Vector3 pos)
    {
        Vector2Int boolVector = new Vector2Int();
        boolVector.x = Mathf.Abs(pos.x) < 96 - c.orthographicSize * 16 / 9 ? 1 : 0;
        boolVector.y = Mathf.Abs(pos.y) < 54 - c.orthographicSize ? 1 : 0;
        return boolVector;
    }
    public void CameraMove(Vector3 target)
    {
        Vector3 desiredPos = new Vector3(target.x, target.y, transform.position.z);
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothPos;
    }
    public void SetFollow(bool follow)
    {
        this.follow = follow;
    }
    public void CameraFollow(Transform followObject)
    {
        if (follow)
        {
            Vector2Int boolVector = InBound(followObject.position);
            Vector3 cameraTarget = Vector3.zero;
            cameraTarget.x = boolVector.x == 1 ? followObject.position.x : transform.position.x;
            cameraTarget.y = boolVector.y == 1 ? followObject.position.y : transform.position.y;
            CameraMove(cameraTarget);
        }
    }
}
