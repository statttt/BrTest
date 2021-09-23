using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Camera))]
public sealed class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject _ray;

    [SerializeField]private MapCreator _mapCreator;

    [Range(0, 10f)]
    public float moveSpeed = 10f;
    [Range(0f, 5f)]
    public float sensitivity = 3;
    public bool isDragging { get; private set; }
    public new Camera camera { get; private set; }

    private Vector2 _tempCenter, _targetDirection, _tempMousePos;
    private float _tempSens;

    [SerializeField] private float _maxZoom = 5f;
    [SerializeField] private float _minZoom = 2.5f;

    private float _leftLimit;
    private float _rightLimit;
    private float _bottomLimit;
    private float _upperLimit;

    private void Start()
    {
        this.camera = GetComponent<Camera>();
        gameObject.transform.position = new Vector2(_mapCreator.maxHorizontalMapPosition/2,_mapCreator.minVerticalMapPosition/2);
        _ray.transform.position = new Vector2(_mapCreator.maxHorizontalMapPosition / 2 - 5f * camera.pixelWidth / camera.pixelHeight, _mapCreator.minVerticalMapPosition / 2 + 5f);
        _leftLimit = _mapCreator.minHorizontalMapPosition + 5f*camera.pixelWidth/ camera.pixelHeight;
        _rightLimit = _mapCreator.maxHorizontalMapPosition - 5f * camera.pixelWidth / camera.pixelHeight;
        _upperLimit = _mapCreator.maxVerticalMapPosition - 5f;
        _bottomLimit = _mapCreator.minVerticalMapPosition + 5f;
    }

    private void Update()
    {
        UpdateInput();
        UpdatePosition();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _leftLimit, _rightLimit), Mathf.Clamp(transform.position.y, _bottomLimit, _upperLimit), transform.position.z);
        ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void UpdateInput()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) OnPointDown(mousePosition);
        else if (Input.GetMouseButtonUp(0)) OnPointUp(mousePosition);
        else if (Input.GetMouseButton(0)) OnPointMove(mousePosition);
    }

    private void UpdatePosition()
    {
        float speed = Time.deltaTime * this.moveSpeed;
        if (this.isDragging) this._tempSens = this.sensitivity;
        else this._tempSens = Mathf.Lerp(this._tempSens, 0f, speed);
        Vector2 newPosition = this.position + this._targetDirection * this._tempSens;
        this.position = Vector2.Lerp(this.position, newPosition, speed);
    }

    private void OnPointDown(Vector2 mousePosition)
    {
        this._tempCenter = GetWorldPoint(mousePosition);
        this._targetDirection = Vector2.zero;
        this._tempMousePos = mousePosition;
        this.isDragging = true;
    }

    private void OnPointMove(Vector2 mousePosition)
    {
        if (this.isDragging)
        {
            Vector2 point = GetWorldPoint(mousePosition);
            float sqrDist = (this._tempCenter - point).sqrMagnitude;
            if (sqrDist > 0.1f)
            {
                this._targetDirection = (this._tempMousePos - mousePosition).normalized;
                this._tempMousePos = mousePosition;
            }
        }
    }

    private void OnPointUp(Vector2 mousePosition)
    {
        this.isDragging = false;
    }

    public Vector2 position
    {
        get { return this.transform.position; }
        set { this.transform.position = new Vector3(value.x, value.y, -10f); }
    }

    private Vector2 GetWorldPoint(Vector2 mousePosition)
    {
        Vector2 point = Vector2.zero;
        Ray ray = this.camera.ScreenPointToRay(mousePosition);
        Vector3 normal = Vector3.forward;
        Vector3 position = Vector3.zero;
        Plane plane = new Plane(normal, position);
        float distance;
        plane.Raycast(ray, out distance);
        point = ray.GetPoint(distance);
        return point;
    }


    private void ZoomCamera(float increment)
    {
        this.camera.orthographicSize = Mathf.Clamp(this.camera.orthographicSize - increment * sensitivity, _minZoom, _maxZoom);
    }
}