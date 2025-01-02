using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerUI playerUI;
    public Transform playerFoot;
    public Health health;
}
