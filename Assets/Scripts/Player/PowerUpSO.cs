using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "powerup", menuName = "SOs/Powerup")]
public class PowerUpSO : ScriptableObject
{
    [SerializeField] public int Cooldown;
    [SerializeField] public string PowerupType;
    [SerializeField] public Color color;
    [SerializeField] public float Duration;


        
 }
