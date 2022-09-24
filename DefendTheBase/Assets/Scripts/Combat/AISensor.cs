using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [ExecuteAlways]
    public class AISensor : MonoBehaviour
    {
        public event Action<List<Transform>> TargetDetectedEvent;
        
        [SerializeField] private float distance = 10f;
        [SerializeField] [Range(0f, 360f)] private float angle = 120f;
        [SerializeField] private float height = 1f;
        [SerializeField] private Color defaultMeshColor = new(0.3657968f, 1f, 0.3254717f, 0.38f);
        [SerializeField] private Color detectedStateMeshColor = new(0.9846383f, 1f, 0.3254902f, 0.38f);
        [SerializeField] private Color rangeGizmosColor = Color.green;
        [SerializeField] private Color objectsInSightGizmosColor = Color.blue;
        [SerializeField] [Tooltip("times / second")] private int scanFrequency = 30;
        [SerializeField] private LayerMask searchingLayers;
        [SerializeField] private LayerMask occlusionLayers;

        private readonly List<Transform> _detectedObjects = new();
        private Mesh _mesh;
        private int _count;
        private float _scanTimer;

        private float ScanInterval => 1f / scanFrequency;
        private float HalfAngle => angle / 2;

        private List<Transform> DetectedObjects
        {
            get
            {
                _detectedObjects.RemoveAll(obj => !obj);
                return _detectedObjects;
            }
        }

        private void Start()
        {
            _mesh = CreateWedgeMesh();
        }

        void Update()
        {
            _scanTimer -= Time.deltaTime;
            if (_scanTimer <= 0f)
            {
                _scanTimer += ScanInterval;
                Scan();
            }
        }

        private void OnValidate()
        {
            _mesh = CreateWedgeMesh();
        }

        private void OnDrawGizmos()
        {
            if (!_mesh) return;
            DrawSensorMesh();
            DrawRange();
            HighlightObjectsInSight();
        }

        private void Scan()
        {
            Collider[] colliders = new Collider[50];
            _count = Physics.OverlapSphereNonAlloc
                (transform.position, distance, colliders, searchingLayers, QueryTriggerInteraction.Collide);

            _detectedObjects.Clear();
            for (int i = 0; i < _count; i++)
            {
                Transform gameObj = colliders[i].gameObject.transform;
                if (IsInSight(gameObj))
                {
                    _detectedObjects.Add(gameObj);
                    TargetDetectedEvent?.Invoke(DetectedObjects);
                }
            }
        }

        private Mesh CreateWedgeMesh()
        {
            var mesh = new Mesh();

            int segments = 20;
            int trianglesQuantity = segments * 4 + 2 + 2;
            int verticesQuantity = trianglesQuantity * 3;

            Vector3[] vertices = new Vector3[verticesQuantity];
            int[] triangles = new int[verticesQuantity];
            
            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0f, -HalfAngle, 0f) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0f, HalfAngle, 0f) * Vector3.forward * distance;
            
            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;

            int verticesIndex = 0;
            
            // left side
            vertices[verticesIndex++] = bottomCenter;
            vertices[verticesIndex++] = bottomLeft;
            vertices[verticesIndex++] = topLeft;
            
            vertices[verticesIndex++] = topLeft;
            vertices[verticesIndex++] = topCenter;
            vertices[verticesIndex++] = bottomCenter;
            
            // right side
            vertices[verticesIndex++] = bottomCenter;
            vertices[verticesIndex++] = topCenter;
            vertices[verticesIndex++] = topRight;
            
            vertices[verticesIndex++] = topRight;
            vertices[verticesIndex++] = bottomRight;
            vertices[verticesIndex++] = bottomCenter;

            float currentAngle = -HalfAngle;
            float deltaAngle = HalfAngle * 2 / segments;
            for (int i = 0; i < segments; i++)
            {
                bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * Vector3.forward * distance;
            
                topLeft = bottomLeft + Vector3.up * height;
                topRight = bottomRight + Vector3.up * height;
                
                // far side
                vertices[verticesIndex++] = bottomLeft;
                vertices[verticesIndex++] = bottomRight;
                vertices[verticesIndex++] = topRight;
            
                vertices[verticesIndex++] = topRight;
                vertices[verticesIndex++] = topLeft;
                vertices[verticesIndex++] = bottomLeft;
            
                // top side
                vertices[verticesIndex++] = topCenter;
                vertices[verticesIndex++] = topLeft;
                vertices[verticesIndex++] = topRight;
            
                // bottom side
                vertices[verticesIndex++] = bottomCenter;
                vertices[verticesIndex++] = bottomRight;
                vertices[verticesIndex++] = bottomLeft;
                
                currentAngle += deltaAngle;
            }
            
            for (int i = 0; i < verticesQuantity; i++)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
        
        private void DrawSensorMesh()
        {
            Gizmos.color = IsTargetDetected() ? detectedStateMeshColor : defaultMeshColor;
            var ownerTransform = transform;
            Gizmos.DrawMesh(_mesh, ownerTransform.position, ownerTransform.rotation);
        }

        private bool IsTargetDetected()
        {
            return DetectedObjects.Count != 0;
        }

        private void DrawRange()
        {
            Gizmos.color = rangeGizmosColor;
            Gizmos.DrawWireSphere(transform.position, distance);
        }

        private void HighlightObjectsInSight()
        {
            Gizmos.color = objectsInSightGizmosColor;
            foreach (Transform gameObj in DetectedObjects)
            {
                Gizmos.DrawSphere(gameObj.position, 0.2f);
            }
        }
        
        private bool IsInSight(Transform gameObj)
        {
            Bounds gameObjBounds = gameObj.GetComponent<Collider>().bounds;
            
            if (!IsInVerticalSight(gameObjBounds))
            {
                return false;
            }
            
            if (!IsInHorizontalAngularSight(gameObjBounds))
            {
                return false;
            }
            
            if (!IsVisible(gameObjBounds))
            {
                return false;
            }
            
            return true;
        }

        private bool IsInVerticalSight(Bounds gameObjBounds)
        {
            var ownerPosition = transform.position;
            Vector3 distanceVectorToMinBoundPoint = gameObjBounds.min - ownerPosition;
            Vector3 distanceVectorToMaxBoundPoint = gameObjBounds.max - ownerPosition;
            
            return !(distanceVectorToMinBoundPoint.y < 0f && distanceVectorToMaxBoundPoint.y < 0f ||
                   distanceVectorToMinBoundPoint.y > height && distanceVectorToMaxBoundPoint.y > height);
        }
        
        
        private bool IsInHorizontalAngularSight(Bounds gameObjBounds)
        {
            // As long as You are calculating only on XZ plane, it doesn't matter if You take bottom or top vertices.
            // But it is important to assign 0f to Y coordinate, because of transform.forward has the Y = 0f;
            Vector3[] bottomVertices =
            {
                new(gameObjBounds.min.x, 0f, gameObjBounds.min.z),
                new(gameObjBounds.max.x, 0f, gameObjBounds.max.z),
                new(gameObjBounds.min.x, 0f, gameObjBounds.max.z),
                new(gameObjBounds.max.x, 0f, gameObjBounds.min.z)
            };

            Transform ownerTransform = transform;
            foreach (var bottomVertex in bottomVertices)
            {
                Vector3 distanceVector = bottomVertex - ownerTransform.position;
                float deltaAngle = Vector3.Angle(distanceVector, ownerTransform.forward);
                
                if (deltaAngle <= HalfAngle) return true;
            }

            return false;
        }
        
        private bool IsVisible(Bounds gameObjBounds)
        {
            Vector3[] allVertices =
            {
                // bottom
                new(gameObjBounds.min.x, gameObjBounds.min.y, gameObjBounds.min.z),
                new(gameObjBounds.max.x, gameObjBounds.min.y, gameObjBounds.max.z),
                new(gameObjBounds.min.x, gameObjBounds.min.y, gameObjBounds.max.z),
                new(gameObjBounds.max.x, gameObjBounds.min.y, gameObjBounds.min.z),
                
                // top
                new(gameObjBounds.min.x, gameObjBounds.max.y, gameObjBounds.min.z),
                new(gameObjBounds.max.x, gameObjBounds.max.y, gameObjBounds.max.z),
                new(gameObjBounds.min.x, gameObjBounds.max.y, gameObjBounds.max.z),
                new(gameObjBounds.max.x, gameObjBounds.max.y, gameObjBounds.min.z)
                
            };

            Vector3 ownerPosition = transform.position;
            ownerPosition.y = height / 2;
            foreach (var vertex in allVertices)
            {
                if (!Physics.Linecast(ownerPosition, vertex, occlusionLayers)) return true;
            }

            return false;
        }
    }
}
