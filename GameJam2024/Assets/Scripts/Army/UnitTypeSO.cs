using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inutil, nu am mai folosit SO pana la urma

[CreateAssetMenu(fileName = "Unit_", menuName = "Unit")]
public class UnitTypeSO : ScriptableObject
{
    public enum Team{Ally, Enemy};
    public Team team;
    public int health;
    public int damage;
    public Sprite sprite;
}
