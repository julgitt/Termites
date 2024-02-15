using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _target;

    private Vector2 _destPos;

    private void Update()
    {
        if (_target == null)
            _destPos = new Vector3(0, 0, -10);
        else
            _destPos = _target.position;

        Vector3 newPos = Vector2.Lerp(transform.position, _destPos, Time.deltaTime * _speed);
        newPos.z = -10f;

        transform.position = newPos;
    }
}
