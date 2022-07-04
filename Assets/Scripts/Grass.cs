using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Grass : MonoBehaviour
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Color> colors = new List<Color>();
    public List<int> indicies = new List<int>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> length = new List<Vector2>();

    public float grassWidth = 1;
    public float grassLength = 1;
    
    public Color color = Color.white;
    
    [SerializeField] private int _widthCount = 38;
    [SerializeField] private int _heightCount = 38;
    [SerializeField] private int _width = 1;
    [SerializeField] private int _height = 1;
    
    private Mesh _startMesh;
    private MeshFilter _meshFilter;
    private bool _isMowed;

    public void ClearGrass()
    {
        _meshFilter.sharedMesh = new Mesh();
    }
    
    public void ResetGrass()
    {
        ClearLists();
        positions = _startMesh.vertices.ToList();
        colors = _startMesh.colors.ToList();
        normals = _startMesh.normals.ToList();
        length = _startMesh.uv.ToList();
        for (int i = 0; i < positions.Count; i++)
        {
            indicies.Add(i);
        }
        
        UpdateMesh();
    }

    public int RemoveInRadius(Vector3 center, float radius)
    {
        var removed = 0;
        if (_isMowed) return removed;
        
        center = transform.InverseTransformPoint(center);

        for (int i = 0; i < positions.Count; i++)
        {
            
            var pos = new Vector3(positions[i].x,0, positions[i].z );
            float dist = Vector3.Distance(center, pos);

            // if its within the radius of the brush, remove all info
            if (dist <= radius)
            {
                removed++;
                positions.RemoveAt(i);
                colors.RemoveAt(i);
                normals.RemoveAt(i);
                length.RemoveAt(i);
                indicies.RemoveAt(i);
                i--;

                for (int j = 0; j < indicies.Count; j++)
                {
                    indicies[j] = j;
                }
                
                if (positions.Count <= (_widthCount * _heightCount) * 0.2f)
                {
                    _isMowed = true;
                    GrowGrass();
                    UpdateMesh();
                    return removed;
                }
            }
        }
        
        UpdateMesh();
        return removed;
    }
    
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        GenerateGrass();
        _startMesh = Instantiate(_meshFilter.sharedMesh);
    }

    private void GenerateGrass()
    {
        
        var xDistance = (float) _width / _widthCount;
        var zDistance = (float) _height / _heightCount;

        var startPosition = new Vector3(-_width / 2f, 0, -_height / 2f);; 
        var brushPosition = startPosition;

        ClearLists();
        var i = 0;
        for (int z = 0; z < _heightCount; z++)
        {
            for (int x = 0; x < _widthCount; x++)
            {
                positions.Add(brushPosition);
                colors.Add(color);
                indicies.Add(i);
                normals.Add(Vector3.up);
                length.Add(new Vector2(grassWidth, grassLength));

                var offset = xDistance * x * Vector3.right + zDistance * z * Vector3.forward;
                brushPosition = startPosition + offset;
                i++;
            }
        }
        
        UpdateMesh();
    }
    
    private void UpdateMesh()
    {
        var mesh = new Mesh();
        mesh.SetVertices(positions);
        var indi = indicies.ToArray();
        mesh.SetIndices(indi, MeshTopology.Points, 0);
        mesh.SetUVs(0, length);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
        _meshFilter.mesh = mesh;
    }

    private void ClearLists()
    {
        positions.Clear();
        colors.Clear();
        indicies.Clear();
        normals.Clear();
        length.Clear();
    }

    private void GrowGrass()
    {
        transform.position += Vector3.down * 3;
        ResetGrass();
        transform.DOMoveY(transform.position.y + 3, 1f).SetEase(Ease.OutBack).
            OnComplete(() => _isMowed = false);
    }
}
