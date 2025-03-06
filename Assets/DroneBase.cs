using UnityEngine;

// Базовый класс дрона
[RequireComponent(typeof(Rigidbody))]
public abstract class DroneBase : MonoBehaviour
{
    [Header("Drone Components")]
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider droneCollider;
    [SerializeField] protected Renderer droneRenderer;

    [Header("Drone Configuration")]
    [SerializeField] protected DroneData droneData;

    protected IFlightBehavior flightBehavior;

    private Vector3 currentVelocity;

    protected virtual void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!droneCollider) droneCollider = GetComponent<Collider>();
        if (!droneRenderer) droneRenderer = GetComponent<Renderer>();
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

    public void ChangeFlightBehavior(IFlightBehavior newBehavior)
    {
        flightBehavior = newBehavior;
    }

    // Методы для расширения функционала
    public virtual void ApplyEnvironmentalEffects(Vector3 windForce) { }
    public virtual void UpdateSensors() { }
    public virtual void UpdateVisualization() { }
}

// Пример реализации поведения полета
public class HoverFlight : IFlightBehavior
{
    public void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data)
    {
        // Логика парящего полета
        Vector3 targetVelocity = Vector3.up * data.maxSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, data.acceleration * Time.fixedDeltaTime);
    }
}

public class ForwardFlight : IFlightBehavior
{
    public void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data)
    {
        // Логика поступательного движения вперед
        Vector3 targetVelocity = drone.transform.forward * data.maxSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, data.acceleration * Time.fixedDeltaTime);

        // Стабилизация вращения
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, data.rotationSpeed * Time.fixedDeltaTime);
    }
}