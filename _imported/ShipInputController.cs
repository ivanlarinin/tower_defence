using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player input for controlling a SpaceShip.
/// Supports both keyboard and mobile (virtual joystick + on-screen buttons) control modes.
/// </summary>
public class ShipInputController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Mobile
    }

    public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

    [SerializeField] private ControlMode m_ControlMode;

    public void Construct(VirtualGamepad virtualGamePad)
    {
        m_VirtualGamePad = virtualGamePad;
    }

    private SpaceShip m_TargetShip;
    private VirtualGamepad m_VirtualGamePad;

    private void Start()
    {
        if (m_ControlMode == ControlMode.Keyboard)
        {
            m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(false);
            m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(false);
            m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(false);
        }
        else
        {
            m_VirtualGamePad.VirtualJoystick.gameObject.SetActive(true);
            m_VirtualGamePad.MobileFirePrimary.gameObject.SetActive(true);
            m_VirtualGamePad.MobileFireSecondary.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (m_TargetShip == null) return;

        if (m_ControlMode == ControlMode.Keyboard)
            ControlKeyboard();

        if (m_ControlMode == ControlMode.Mobile)
            ControlMobile();
    }

    private void ControlMobile()
    {
        Vector3 dir = m_VirtualGamePad.VirtualJoystick.Value;

        var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
        var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

        // If the mobile primary fire button is held, it triggers the primary firing mode.
        if (m_VirtualGamePad.MobileFirePrimary.IsHold == true)
        {
            // m_TargetShip.Fire(TurretMode.Primary);
        }

        // If the mobile secondary fire button is held, it triggers the secondary firing mode.
        if (m_VirtualGamePad.MobileFireSecondary.IsHold == true)
        {
            // m_TargetShip.Fire(TurretMode.Secondary);
        }


        m_TargetShip.ThrustControl = Mathf.Max(0, dot);
        m_TargetShip.TorqueControl = -dot2;
    }

    private void ControlKeyboard()
    {
        float thrust = 0;
        float torque = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            thrust = 1.0f;

        if (Input.GetKey(KeyCode.DownArrow))
            thrust = -1.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            torque = 1.0f;

        if (Input.GetKey(KeyCode.RightArrow))
            torque = -1.0f;

        // If the Space key is pressed, it triggers the primary firing mode of the target ship.
        if (Input.GetKey(KeyCode.Space))
        {
            // m_TargetShip.Fire(TurretMode.Primary);
        }

        // If the X key is pressed, it triggers the secondary firing mode of the target ship.
        if (Input.GetKey(KeyCode.X))
        {
            // m_TargetShip.Fire(TurretMode.Secondary);
        }

        m_TargetShip.ThrustControl = thrust;
        m_TargetShip.TorqueControl = torque;
    }
}
