using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tileGrassPrefab, _tileMountainPrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform prefabHolder;
    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    }
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GenerateCell(x, y);
            }
        }

        CenterCam();

        GameManager.Instance.ChangeState(GameState.SpawnHero);
    }

    private void CenterCam()
    {
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -15);
    }

    private void GenerateCell(int x, int y)
    {
        var randomTile = UnityEngine.Random.Range(0, 6) == 3 ? _tileMountainPrefab : _tileGrassPrefab;
        var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);

        spawnedTile.name = $"Tile {x} {y}";

        spawnedTile.Init(x, y);

        spawnedTile.transform.parent = prefabHolder;
        _tiles[new Vector2(x, y)] = spawnedTile;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}