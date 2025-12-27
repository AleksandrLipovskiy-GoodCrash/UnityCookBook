using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMoveToClick : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    Animator m_Animaror;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animaror = GetComponent<Animator>();
    }

    void Update()
    {
        var mouse = Mouse.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            var ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                m_Agent.destination = m_HitInfo.point;
            }
        }

        if (m_Agent.velocity.magnitude != 0f)
        {
            m_Animaror.SetBool("isRunning", true);
        }
        else
        {
            m_Animaror.SetBool("isRunning", false);
        }
    }

    public void OnAnimatorMove()
    {
        if (m_Animaror.GetBool("isRunning"))
        {
            m_Agent.speed = (m_Animaror.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
