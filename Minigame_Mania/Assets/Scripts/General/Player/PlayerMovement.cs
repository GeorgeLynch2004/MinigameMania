using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public enum ControlMode
{
    ButtonMash,
    FreeMovement,
    BoardGame
}

public class PlayerMovement : NetworkBehaviour 
{    
    [SerializeField] private ControlMode m_ControlMode;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_MaxVelocityX;
    [SerializeField] private float m_MaxVelocityY;
    [SerializeField] private float m_JumpHeight;
    [SerializeField] private bool m_CanMove;
    [SerializeField] private bool m_IsGrounded;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private float m_ButtonMashIncrement;

    private void Start() 
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();   
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!m_CanMove) return;

        // Depending on the level / playmode will determine the movement model used
        if (m_ControlMode == ControlMode.FreeMovement)
        {
            m_Rigidbody.velocity = FreeMovement();
        }
        if (m_ControlMode == ControlMode.ButtonMash)
        {
            transform.position = ButtonMashMovement();
        }
        if (m_ControlMode == ControlMode.BoardGame)
        {
            m_Rigidbody.velocity = BoardGameMovement();
        }
    }

    private Vector3 FreeMovement()
    {
        Vector3 _velocity = m_Rigidbody.velocity;

        // Horizontal input
        if (Input.GetKey(KeyCode.A)){_velocity += -Vector3.right * m_MovementSpeed * Time.deltaTime;}
        if (Input.GetKey(KeyCode.D)){_velocity += Vector3.right * m_MovementSpeed * Time.deltaTime;}

        // Jump input
        if (Input.GetKey(KeyCode.Space) && m_IsGrounded == true)
        {
            _velocity += Vector3.up * m_JumpHeight * Time.deltaTime;
        }

        // Cap velocity
        _velocity.x = Mathf.Clamp(_velocity.x, -m_MaxVelocityX, m_MaxVelocityX);

        if (_velocity.y > 0)
        {
            _velocity.y = Mathf.Clamp(_velocity.y, -m_MaxVelocityY, m_MaxVelocityY);
        }
        else
        {
            _velocity.y *= 1.1f;
        }
        
        return _velocity;
    }

    private Vector3 ButtonMashMovement()
    {
        Vector3 position = transform.position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            position.x += m_ButtonMashIncrement;
        }
    
        return position;
    }

    private Vector3 BoardGameMovement()
    {
        // Until completed return 0
        return Vector3.zero;
    }

    public void setIsGrounded(bool state)
    {
        m_IsGrounded = state;
    }

    public bool getCanMove()
    {
        return m_CanMove;
    }

    public void setCanMove(bool state)
    {
        m_CanMove = state;
    }

    public void setControlMode(ControlMode controlMode)
    {
        m_ControlMode = controlMode;
    }
}
