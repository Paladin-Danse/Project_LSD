using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Dungeon[] dungeons;
    private GameObject dungeonMapPrefab;
    SelectedDungeonKeep selectedDungeonKeep;

    private void Awake()
    {
        selectedDungeonKeep = FindObjectOfType<SelectedDungeonKeep>();
        dungeonMapPrefab = dungeons[selectedDungeonKeep.mapNumber].Ddata.dungeonPrefab;
    }
    private void Start()
    {
        CreateMap();
    }
    void CreateMap()
    {
        Instantiate(dungeonMapPrefab, transform.position, transform.rotation);
    }
}
