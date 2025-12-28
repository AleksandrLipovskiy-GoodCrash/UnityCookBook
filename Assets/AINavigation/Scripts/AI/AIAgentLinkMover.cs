using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

/// <summary>
/// Move an m_Agent when traversing a OffMeshLink given specific animated methods
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve m_Curve = new AnimationCurve();

    private NavMeshAgent m_Agent;
    private Animator m_Animaror;

    IEnumerator Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animaror = GetComponent<Animator>();

        m_Agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (m_Agent.isOnOffMeshLink)
            {
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(m_Agent));
                else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(m_Agent, 1.0f, 0.5f));
                else if (m_Method == OffMeshLinkMoveMethod.Curve)
                    yield return StartCoroutine(Curve(m_Agent, 0.5f));
                m_Agent.CompleteOffMeshLink();
            }

            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent m_Agent)
    {
        OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;
        while (m_Agent.transform.position != endPos)
        {
            m_Agent.transform.position =
                Vector3.MoveTowards(m_Agent.transform.position, endPos, m_Agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent m_Agent, float height, float duration)
    {
        OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
        Vector3 startPos = m_Agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            m_Animaror.SetBool("isInJump", true);

            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            m_Agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator Curve(NavMeshAgent m_Agent, float duration)
    {
        OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
        Vector3 startPos = m_Agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = m_Curve.Evaluate(normalizedTime);
            m_Agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    private void Update()
    {
        if (m_Agent.isOnOffMeshLink == false)
        {
            m_Animaror.SetBool("isInJump", false);
        }
    }
}
