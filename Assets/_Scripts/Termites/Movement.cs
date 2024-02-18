using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Settings")]
    //[SerializeField] private int _age = 1;
    //[SerializeField] private int _health = 20;
    [SerializeField] private float _walkingSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _bouncingRotationSpeed = 10f;
    [SerializeField] private float _wanderStrength = .25f;
    [SerializeField] private float _sightDistance = .5f;
    [SerializeField] private float _bounceThreshold = .1f;
    [SerializeField] private LayerMask _obstacles;

    private Vector2 _position;
    private Vector2 _velocity;
    private Vector2 _desiredDirection;
    private float _rotationEndTime;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        bool isBouncingOffObstacle = _rotationEndTime < Time.time;
        float rotationSpeed = isBouncingOffObstacle ? _rotationSpeed : _bouncingRotationSpeed;

        _desiredDirection = GetDesiredDirection();
        Vector2 desiredVelocity = _desiredDirection * _walkingSpeed;
        ApplyAcceleration(desiredVelocity, rotationSpeed);
        
        UpdatePosition();
        UpdateRotation();
    }

    private void ApplyAcceleration(Vector2 desiredVelocity, float rotationSpeed)
    {
        Vector2 deltaVelocity = (desiredVelocity - _velocity) * rotationSpeed;
        Vector2 acceleration = Vector2.ClampMagnitude(deltaVelocity, rotationSpeed);
        _velocity = Vector2.ClampMagnitude(_velocity + acceleration * Time.deltaTime, _walkingSpeed);
    }

    private void UpdatePosition()
    {
        _position += _velocity * Time.deltaTime;
        transform.position = _position;
    }

    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Vector2 GetDesiredDirection()
    {
        Vector2 currentDirection = _velocity.normalized;
        Vector2[] rayDirections = CalculateRayDirections(currentDirection, 2, 40);

        bool obstacleDetected = DetectObstacles(rayDirections, out Vector2 newDirection);

        if (obstacleDetected)
        {
            _rotationEndTime = Time.time + _bounceThreshold;
            Debug.DrawRay(transform.position, newDirection.normalized, Color.green);
            return newDirection.normalized;
        }

        return (_desiredDirection + (Random.insideUnitCircle * _wanderStrength)).normalized;
    }

    private Vector2[] CalculateRayDirections(Vector2 currentDirection, int numberOfRays, float fieldOfView)
    {
        Vector2[] rayDirections = new Vector2[numberOfRays];

        float angleIncrement = fieldOfView / (numberOfRays - 1);
        float startAngle = -fieldOfView / 2;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + i * angleIncrement;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            rayDirections[i] = rotation * currentDirection;
        }

        return rayDirections;
    }

    private bool DetectObstacles(Vector2[] rayDirections, out Vector2 newDirection)
    {
        newDirection = Vector2.zero;
        bool obstacleDetected = false;

        foreach (Vector2 rayDirection in rayDirections)
        {
            Debug.DrawRay(transform.position, rayDirection * _sightDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _sightDistance, _obstacles);

            if (hit.collider != null)
            {
                obstacleDetected = true;
                newDirection += hit.normal.normalized;
            }
            else
            {
                newDirection += rayDirection.normalized;
            }
        }

        return obstacleDetected;
    }
}


