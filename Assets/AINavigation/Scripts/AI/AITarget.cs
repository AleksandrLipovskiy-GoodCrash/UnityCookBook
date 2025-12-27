using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AITarget : MonoBehaviour
{
    public Transform Target;
    public float AttackDistance = 2.5f;

    private NavMeshAgent m_Agent;
    private Animator m_Animaror;
    private float m_DistanceToTarget;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animaror = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Target == null)
        {
            return;
        }

        m_DistanceToTarget = Vector3.Distance(transform.position, Target.position);
        if (m_DistanceToTarget < AttackDistance)
        {
            m_Agent.isStopped = true;
            m_Animaror.SetBool("isAttacking", true);
            
        }
        else
        {
            m_Agent.isStopped = false;
            m_Animaror.SetBool("isAttacking", false);
            m_Agent.destination = Target.position;
        }
    }

    public void OnAnimatorMove()
    {
        if (m_Animaror.GetBool("isAttacking") == false)
        {
            m_Agent.speed = (m_Animaror.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}

