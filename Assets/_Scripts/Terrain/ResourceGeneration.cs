using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneration : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _foodQuantity;
    [SerializeField] private int _buildingMaterialsQuantity;

    [Header("Instances")]
    [SerializeField] private Transform _terrainTransform;

    private float _terrainSize;


    // Start is called before the first frame update
    void Start()
    {
        _terrainSize = _terrainTransform.localScale.x;
    }

    void CreateRandomSpawnPoint()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
