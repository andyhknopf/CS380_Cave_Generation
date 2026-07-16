using UnityEngine;

[ExecuteAlways]
public class VoxelGrid : MonoBehaviour
{
    [Header("Grid Dimensions")]
    [SerializeField] private Vector3Int resolution = new Vector3Int(16, 16, 16);

    [SerializeField] private float cellSize = 1f;

    [Header("Density")]
    [SerializeField] private float isoLevel = 0.5f;

    [SerializeField] private float defaultDensity = 0f;

    [Header("Gizmo Preview")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private int maxGizmoPoints = 8000;

    private float[] densities;

    public Vector3Int Resolution => resolution;
    public float CellSize => cellSize;
    public float IsoLevel => isoLevel;
    public int PointCount => resolution.x * resolution.y * resolution.z;

    private void OnEnable()
    {
        GenerateGrid();
    }

    private void OnValidate()
    {
        resolution.x = Mathf.Max(2, resolution.x);
        resolution.y = Mathf.Max(2, resolution.y);
        resolution.z = Mathf.Max(2, resolution.z);
        cellSize = Mathf.Max(0.01f, cellSize);
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        int total = PointCount;
        densities = new float[total];
        for (int i = 0; i < total; i++)
            densities[i] = defaultDensity;
    }

    public int IndexFromCoord(int x, int y, int z)
    {
        return x + y * resolution.x + z * resolution.x * resolution.y;
    }

    public Vector3 PointToWorld(int x, int y, int z)
    {
        return transform.position + new Vector3(x, y, z) * cellSize;
    }

    public float GetDensity(int x, int y, int z)
    {
        if (densities == null || densities.Length != PointCount) GenerateGrid();
        return densities[IndexFromCoord(x, y, z)];
    }

    public void SetDensity(int x, int y, int z, float value)
    {
        if (densities == null || densities.Length != PointCount) GenerateGrid();
        densities[IndexFromCoord(x, y, z)] = value;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (densities == null || densities.Length != PointCount) return;
        if (PointCount > maxGizmoPoints) return;

        float gizmoSize = Mathf.Min(cellSize * 0.15f, 0.15f);

        for (int z = 0; z < resolution.z; z++)
        {
            for (int y = 0; y < resolution.y; y++)
            {
                for (int x = 0; x < resolution.x; x++)
                {
                    bool inside = GetDensity(x, y, z) >= isoLevel;
                    Gizmos.color = inside ? Color.red : Color.blue;
                    Gizmos.DrawSphere(PointToWorld(x, y, z), gizmoSize);
                }
            }
        }

        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(resolution.x - 1, resolution.y - 1, resolution.z - 1) * cellSize;
        Vector3 center = transform.position + size * 0.5f;
        Gizmos.DrawWireCube(center, size);
    }
}
