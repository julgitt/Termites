using UnityEngine;

public class Termite : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _age = 1;
    [SerializeField] private int _health = 20;
    [SerializeField] private float _rotationSpeed = .2f;
    [SerializeField] private float _walkingSpeed = 2f;
    [SerializeField] private float _accelerationSpeed = 2f;
    [SerializeField] private float _sightDistance = 1f;
    [SerializeField] private float _bounceTreshold = .1f;
    [SerializeField] private LayerMask _obstacles;

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 desiredDirection;
    private float turnAroundEndTime;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Time.time > turnAroundEndTime)
            desiredDirection = CheckCollision(desiredDirection);

        Vector2 desiredVelocity = desiredDirection * _walkingSpeed;

        ApplyAcceleration(desiredVelocity);
        UpdatePosition();
        UpdateRotation();
    }

    private void ApplyAcceleration(Vector2 desiredVelocity)
    {
        Vector2 deltaVelocity = (desiredVelocity - velocity) * _accelerationSpeed;
        Vector2 acceleration = Vector2.ClampMagnitude(deltaVelocity, _accelerationSpeed);
        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, _walkingSpeed);
    }

    private void UpdatePosition()
    {
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Vector2 CheckCollision(Vector2 direction)
    {
        Vector2[] rayDirections = {
            direction,
            Quaternion.Euler(0, 0, 30) * direction,
            Quaternion.Euler(0, 0, -30) * direction,
        };

        bool obstacleOccurred = false;
        RaycastHit2D hit;
        Vector2 newDirection = Vector2.zero;

        foreach (Vector2 rayDirection in rayDirections)
        {
            Debug.DrawRay(transform.position, rayDirection * _sightDistance, Color.red);

            hit = Physics2D.Raycast(transform.position, rayDirection, _sightDistance, _obstacles);

            if (hit.collider != null)
            {
                obstacleOccurred = true;
                newDirection += -rayDirection;
            }
        }

        if (obstacleOccurred)
        {
            turnAroundEndTime = Time.time + _bounceTreshold;
            newDirection = CalculateAvoidanceDirection(direction, newDirection);
            newDirection -= direction;

            Debug.DrawRay(transform.position, newDirection, Color.green);
            return newDirection;
        }

        return (direction + GetRandomRotation()).normalized;
    }

    private Vector2 CalculateAvoidanceDirection(Vector2 currentDirection, Vector2 collisionDirection)
    {
        float crossProduct = Vector3.Cross(currentDirection, collisionDirection).z;
        Vector2 turnLeft = Quaternion.Euler(0, 0, -120) * currentDirection;
        Vector2 turnRight = Quaternion.Euler(0, 0, 120) * currentDirection;

        return crossProduct < 0 ? turnLeft : turnRight;
    }

    private Vector2 GetRandomRotation()
    {
        return Random.insideUnitCircle * _rotationSpeed;
    }
}
