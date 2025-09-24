using System;
using TowerDefence;
using UnityEngine;

/// <summary>
/// Singleton that manages the player's state, including their active ship, lives, score, and kills.
/// Handles spawning, respawning, and linking the ship to camera/input controllers.
/// </summary>
public class Player : SingletonBase<Player>
{
    public static SpaceShip SelectedSpaceShip;

    [SerializeField] private int m_NumLives;
    public int NumLives { get { return m_NumLives; } }

    [SerializeField] private SpaceShip m_PlayerShipPrefab;

    // [SerializeField] private StarfieldParallaxController m_ParallaxController;

    private FollowCamera m_FollowCamera;
    // private ShipInputController m_ShipInputController;
    private Transform m_SpawnPoint;

    public FollowCamera FollowCamera => m_FollowCamera;

    // public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
    // {
    //     m_FollowCamera = followCamera;
    //     m_ShipInputController = shipInputController;
    //     m_SpawnPoint = spawnPoint;
    // }

    private SpaceShip m_Ship;
    public SpaceShip ActiveShip => m_Ship;

    private int m_Score;
    private int m_NumKills;

    public int Score => m_Score;
    public int NumKills => m_NumKills;

    public SpaceShip ShipPrefab
    {
        get
        {
            if (SelectedSpaceShip == null)
            {
                return m_PlayerShipPrefab;
            }
            else
            {
                return SelectedSpaceShip;
            }
        }
    }

    private void Start()
    {
        // Respawn();
    }

    private void OnShipDeath()
    {
        m_NumLives--;

        if (m_NumLives > 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

        m_Ship = newPlayerShip.GetComponent<SpaceShip>();

        m_FollowCamera.SetTarget(m_Ship.transform);
        // m_ShipInputController.SetTargetShip(m_Ship);

        m_Ship.EventOnDeath.AddListener(OnShipDeath);

        // if (m_ParallaxController != null)
        // {
        //     m_ParallaxController.target = m_Ship.transform;
        // }
    }

    public void AddKill()
    {
        m_NumKills += 1;
    }

    public void AddScore(int num)
    {
        m_Score += num;
    }

    protected void TakeDamage(int m_damage)
    {
        m_NumLives -= m_damage;
        if (m_NumLives <= 0)
        {
            m_NumLives = 0;
        }     
    }
}
