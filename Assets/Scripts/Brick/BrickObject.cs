using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Brick", order = 1)]
public class brickObject : ScriptableObject
{
    public int score;
    public int nbrHit;
    public Color color;
}
