using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking;
using Unity.Netcode;

public class Board : NetworkBehaviour
{
    [SerializeField] private GameObject[] m_board;
}
