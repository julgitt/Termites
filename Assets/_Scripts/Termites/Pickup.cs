using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _sightDistance;
    [SerializeField] private float _pickupRange;
    [SerializeField] private LayerMask _resourceType;
    [SerializeField] private float _fieldOfView = 40;

    private Transform _targetResource;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_targetResource == null)
            ResourceSearching(_resourceType);
        else
            HandlePicking();
    }

    private void ResourceSearching(LayerMask resourceType)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sightDistance, resourceType);
        List<Transform> resources = new List<Transform>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform resource = colliders[i].gameObject.transform;
            Vector2 resourceDirection = resource.position - transform.position;
            float angle = Vector2.Angle(transform.forward, resourceDirection);
            if (angle < _fieldOfView / 2)
                resources.Add(resource);
        }
        int resourceIndex = Random.Range(0, resources.Count);
        _targetResource = resources[resourceIndex];
    }

    private void HandlePicking()
    {
        bool isInRange = Vector2.Distance(transform.position, _targetResource.position) < _pickupRange;
        if (isInRange)
        {
            _targetResource = null;
        }
    }
}
