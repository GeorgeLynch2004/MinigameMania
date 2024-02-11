using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
