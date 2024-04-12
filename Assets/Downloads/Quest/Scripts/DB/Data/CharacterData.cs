using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private string _spritePath;
    
    [SerializeField] private float _hp;
    [SerializeField] private float _mp;
    [SerializeField] private float _atk;
    [SerializeField] private float _def;

    public int Id => _id;
    public string Name => _name;
    public string Description => _description;
    public string SpritePath => _spritePath;

    public float Hp => _hp;
    public float Mp => _mp;
    public float Atk => _atk;
    public float Def => _def;
}
