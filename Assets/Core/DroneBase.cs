using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class DroneBase : MonoBehaviour
{
    [Header("Drone Components")] [SerializeField]
    protected Rigidbody rb;

    [SerializeField] protected Collider droneCollider;
    [SerializeField] protected Renderer droneRenderer;

    [Header("Drone Configuration")] [SerializeField]
    protected DroneData droneData;

    protected IFlightBehavior flightBehavior;

    protected virtual void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!droneCollider) droneCollider = GetComponent<Collider>();
        if (!droneRenderer) droneRenderer = GetComponent<Renderer>();

        flightBehavior = new ForwardFlight();
    }

    protected virtual void Start()
    {
        ApplyDroneData();
    }

    private void ApplyDroneData()
    {
        if (droneData == null) return;

        transform.localScale = droneData.silhouetteScale;
        if (droneRenderer && droneData.defaultMaterial)
        {
            droneRenderer.material = droneData.defaultMaterial;
        }

        rb.mass = droneData.weight;
    }

    protected virtual void FixedUpdate()
    {
        if (flightBehavior != null)
        {
            flightBehavior.UpdateFlight(this, rb, droneData);
        }
    }

    // Метод для изменения типа движения
    public void SetMovementType(DroneMovementType newType)
    {
        if (droneData != null)
        {
            droneData.movementType = newType;
        }
    }
}

public class HoverFlight : IFlightBehavior
{
    public void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data)
    {
        Vector3 targetVelocity = Vector3.up * data.maxSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, data.acceleration * Time.fixedDeltaTime);
    }
}

public class ForwardFlight : IFlightBehavior
{
    private float _zigZagTimer = 0f;
    private float _targetHeight;

    public void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data)
    {
        Vector3 targetVelocity = drone.transform.forward * data.maxSpeed;

        MaintainHeight(drone, rb, data);

        switch (data.movementType)
        {
            case DroneMovementType.Straight:
                break;

            case DroneMovementType.ZigZag:
                _zigZagTimer += Time.fixedDeltaTime;
                float horizontalOffset = Mathf.Sin(_zigZagTimer * data.zigZagFrequency) * data.zigZagAmplitude;
                targetVelocity += drone.transform.right * horizontalOffset;
                break;

            case DroneMovementType.CustomPattern:
                break;
        }

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, data.acceleration * Time.fixedDeltaTime);
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, data.rotationSpeed * Time.fixedDeltaTime);
    }

    private void MaintainHeight(DroneBase drone, Rigidbody rb, DroneData data)
    {
        float currentHeight = drone.transform.position.y;

        if (_targetHeight == 0)
        {
            _targetHeight = currentHeight;
        }

        float heightDifference = _targetHeight - currentHeight;

        if (Mathf.Abs(heightDifference) > 0.1f)
        {
            Vector3 liftForce = Vector3.up * heightDifference * data.acceleration;
            rb.AddForce(liftForce, ForceMode.Acceleration);
        }
    }

    public void SetTargetHeight(float newHeight)
    {
        _targetHeight = newHeight;
    }
}

public interface IFlightBehavior
{
    void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data);
}

public enum DroneMovementType
{
    Straight,
    ZigZag,
    CustomPattern,
    Circle
}