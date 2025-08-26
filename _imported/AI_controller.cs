using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Controller for enemy spaceships that handles movement, targeting, and combat behavior.
/// Supports patrol behavior with both random movement and predefined route patrolling.
/// </summary>
[RequireComponent(typeof(SpaceShip))]
public class AIController : MonoBehaviour
{
    /// <summary>
    /// Defines the current behavior state of the AI
    /// </summary>
    public enum AIBehaviour
    {
        Null,   // No behavior - AI is inactive
        Patrol  // Patrol behavior - move between points and engage targets
    }

    #region Fields

    [Header("AI Configuration")]
    [SerializeField] private AIBehaviour m_AIBehaviour;        // Current AI behavior mode
    [SerializeField] private AIPointPatrol m_PatrolPoint;      // Reference to patrol point/route

    [Header("Movement Settings")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationLinear;         // Thrust control multiplier (0-1)
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationAngular;        // Torque control multiplier (0-1)

    [Header("Timing Settings")]
    [SerializeField] private float m_RandomSelectMovePointTime; // Time between random direction changes
    [SerializeField] private float m_FindNewTargetTime;         // Time between target searches
    [SerializeField] private float m_ShootDelay;               // Time between shots

    [Header("Combat Settings")]
    [SerializeField] private float m_EvadeRayLength;           // Length of collision detection ray
    [SerializeField] private float m_AggroRange = 50f; // Alert Enemy Distance

    // Internal state variables
    private SpaceShip m_SpaceShip;              // Reference to the spaceship component
    private Vector3 m_MovePosition;             // Current target position for movement
    private Destructable m_SelectedTarget;      // Currently selected target for attack

    // Timer objects for controlling AI behavior timing
    private Timer m_RandomizeDirectionTimer;    // Controls random direction changes
    private Timer m_FireTimer;                  // Controls firing rate
    private Timer m_FindNewTargetTimer;         // Controls target search frequency

    private const float ProjectileSpeed = 15.0f;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Initialize AI controller and get required components
    /// </summary>
    private void Start()
    {
        m_SpaceShip = GetComponent<SpaceShip>();
        // Debug.Log(m_SpaceShip);
        InitTimers();
    }

    /// <summary>
    /// Main update loop - updates timers and AI behavior
    /// </summary>
    private void Update()
    {
        UpdateTimers();
        UpdateAI();
    }

    /// <summary>
    /// Main AI behavior update - routes to appropriate behavior based on current mode
    /// </summary>
    private void UpdateAI()
    {
        // Debug.Log(m_AIBehaviour);
        if (m_AIBehaviour == AIBehaviour.Patrol)
        {
            UpdateBehaviouralPatrol();
        }
    }

    #endregion


    #region PrivateMethods

    /// <summary>
    /// Patrol behavior update - handles movement, targeting, combat, and collision avoidance
    /// </summary>
    private void UpdateBehaviouralPatrol()
    {
        ActionFindNewMovePosition();    // Determine where to move
        ActionControlShip();            // Apply movement controls
        ActionFindNewAttackTarget();    // Find and select targets
        ActionFire();                   // Attack if conditions are met
        ActionEvadeCollision();         // Avoid obstacles
    }

    /// <summary>
    /// Determines the target position for movement based on current state
    /// Priority: Target position > Patrol route > Random patrol > Return to patrol center
    /// </summary>
    private void ActionFindNewMovePosition()
    {
        // If we have a target, move towards it
        if (m_SelectedTarget != null)
        {
            Vector3 targetVelocity = GetTargetVelocity(m_SelectedTarget);
            m_MovePosition = MakeLead(m_SelectedTarget.transform.position, targetVelocity, ProjectileSpeed);
        }
        else
        {
            // No target - perform patrol behavior
            if (m_PatrolPoint != null)
            {
                // Check if using predefined patrol route
                if (m_PatrolPoint.UsePatrolRoute && m_PatrolPoint.PatrolPointsCount > 0)
                {
                    // Check if we've reached the current patrol point
                    if (m_PatrolPoint.IsAtPatrolPoint(transform.position))
                    {
                        // Advance to next patrol point
                        m_PatrolPoint.AdvanceToNextPatrolPoint();
                    }
                    
                    // Move towards current patrol point
                    m_MovePosition = m_PatrolPoint.GetCurrentPatrolPoint();
                }
                else
                {
                    // Random patrol behavior within patrol zone
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                    // Debug.Log(isInsidePatrolZone);

                    if (isInsidePatrolZone == true)
                    {
                        // Inside patrol zone - move to random point
                        if (m_RandomizeDirectionTimer.IsFinished == true)
                        {
                            Vector2 newPoint = UnityEngine.Random.insideUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                            // Debug.Log("Position: " + newPoint);
                            m_MovePosition = newPoint;
                            m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                        }
                    }
                    else
                    {
                        // Outside patrol zone - return to center
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Simple collision avoidance - if obstacle detected ahead, move to the right
    /// </summary>
    private void ActionEvadeCollision()
    {
        if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
        {
            m_MovePosition = transform.position + transform.right * 100.0f;
        }
    }

    /// <summary>
    /// Attempts to fire at the selected target if conditions are met
    /// Requires: Valid target and fire timer finished
    /// </summary>
    private void ActionFire()
    {
        if (m_SelectedTarget != null)
        {
            if (m_FireTimer.IsFinished == true)
            {
                Debug.Log($"[AI] {name} firing at {m_SelectedTarget.name}");
                
                m_SpaceShip.Fire(TurretMode.Primary);

                m_FireTimer.Start(m_ShootDelay);
            }
        }
    }

    /// <summary>
    /// Searches for new attack targets periodically
    /// Updates the selected target and restarts the search timer
    /// </summary>
    private void ActionFindNewAttackTarget()
    {
        if (m_FindNewTargetTimer.IsFinished == true)
        {
            m_SelectedTarget = FindNearestDestructibleTarget();
            m_FindNewTargetTimer.Start(m_FindNewTargetTime);
        }
    }

    /// <summary>
    /// Finds the nearest valid target from all destructible objects
    /// Filters out: self, neutral team, same team
    /// Returns the closest enemy target or null if none found
    /// </summary>
    /// <returns>The nearest valid target or null</returns>
    private Destructable FindNearestDestructibleTarget()
    {
        float maxDist = float.MaxValue;
        Destructable potentialTarget = null;
        float maxSearchDistance = (m_PatrolPoint != null) ? m_AggroRange : float.MaxValue; // No distance limit if no patrol point

        foreach (var v in Destructable.AllDestructibles)
        {
            // Skip if the destructible is the current spaceship itself
            if (v.GetComponent<SpaceShip>() == m_SpaceShip)
            {
                continue;
            }
            
            // Skip if the destructible belongs to a neutral team
            if (v.TeamId == Destructable.TeamIdNeutral)
            {
                continue;
            }

            // Skip if the destructible belongs to the same team as the spaceship
            if (v.TeamId == m_SpaceShip.TeamId)
            {
                continue;
            }

            // Calculate the distance between the spaceship and the potential target
            float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

            // If the current potential target is closer than the previously found one,
            // update the closest target and the max distance
            if (dist < maxDist && dist < maxSearchDistance)
            {
                maxDist = dist;
                potentialTarget = v;
            }
        }

        return potentialTarget;
    }


    /// <summary>
    /// Applies movement controls to the spaceship
    /// Sets thrust and torque based on navigation settings and target position
    /// </summary>
    private void ActionControlShip()
    {
        m_SpaceShip.ThrustControl = m_NavigationLinear;
        m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
    }

    /// <summary>
    /// Maximum angle for ship rotation (prevents over-rotation)
    /// </summary>
    private const float MAX_ANGLE = 45.0f;

    /// <summary>
    /// Computes normalized torque control to align ship with target position
    /// Converts target position to local coordinates and calculates rotation angle
    /// </summary>
    /// <param name="targetPosition">World position to align towards</param>
    /// <param name="ship">Ship transform to compute alignment for</param>
    /// <returns>Normalized torque value (-1 to 1)</returns>
    private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
    {
        Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

        float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

        angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

        return -angle;
    }

    /// <summary>
    /// Calculates lead position for a moving target based on projectile speed and target velocity
    /// </summary>
    /// <param name="targetPosition">Current target position</param>
    /// <param name="targetVelocity">Target's velocity vector</param>
    /// <param name="projectileSpeed">Speed of the projectile (or ship movement speed)</param>
    /// <returns>Lead position where the target will be when projectile arrives</returns>
    private Vector3 MakeLead(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        // If target is not moving or projectile is very fast, return current position
        if (targetVelocity.magnitude < 0.1f || projectileSpeed < 0.1f)
        {
            return targetPosition;
        }

        // Calculate time to reach target
        float distance = Vector3.Distance(transform.position, targetPosition);
        float timeToTarget = distance / projectileSpeed;

        // Calculate where target will be after that time
        Vector3 leadPosition = targetPosition + targetVelocity * timeToTarget;

        return leadPosition;
    }

    /// <summary>
    /// Gets the velocity of a target (SpaceShip or other Rigidbody2D)
    /// </summary>
    /// <param name="target">Target to get velocity from</param>
    /// <returns>Velocity vector of the target</returns>
    private Vector3 GetTargetVelocity(Destructable target)
    {
        // Try to get velocity from SpaceShip component
        SpaceShip targetShip = target.GetComponent<SpaceShip>();
        if (targetShip != null)
        {
            Rigidbody2D shipRigid = targetShip.GetComponent<Rigidbody2D>();
            if (shipRigid != null)
            {
                return shipRigid.linearVelocity;
            }
        }

        // Try to get velocity from any Rigidbody2D component
        Rigidbody2D targetRigid = target.GetComponent<Rigidbody2D>();
        if (targetRigid != null)
        {
            return targetRigid.linearVelocity;
        }

        // If no velocity component found, return zero
        return Vector3.zero;
    }
    #endregion

    #region Timer Management

    /// <summary>
    /// Initialize all timer objects with their respective durations
    /// </summary>
    private void InitTimers()
    {
        m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
        m_FireTimer = new Timer(m_ShootDelay);
        m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
    }

    /// <summary>
    /// Update all timers by removing delta time from each
    /// </summary>
    private void UpdateTimers()
    {
        m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
        m_FireTimer.RemoveTime(Time.deltaTime);
        m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
    }
    #endregion
    
    #region Public API

    /// <summary>
    /// Sets the AI to patrol behavior and assigns a patrol point
    /// </summary>
    /// <param name="point">The patrol point to use for movement</param>
    public void SetPatrolBehaviour(AIPointPatrol point)
    {
        m_AIBehaviour = AIBehaviour.Patrol;
        m_PatrolPoint = point;
    }

    /// <summary>
    /// Sets a predefined patrol route with multiple waypoints
    /// </summary>
    /// <param name="patrolPoints">List of waypoint transforms</param>
    /// <param name="loopRoute">Whether to loop back to first point after last</param>
    public void SetPatrolRoute(List<Transform> patrolPoints, bool loopRoute = true)
    {
        if (m_PatrolPoint != null)
        {
            m_PatrolPoint.SetPatrolPoints(patrolPoints, loopRoute);
        }
    }
    #endregion
}
