using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Lumber : MonoBehaviour
{
    public enum State
    {
        Idle,
        Cutting,
        RunningEmpty,
        RunningLoaded,
        Loaded
    }

    NavMeshAgent agent;
    State state;
    Vector3 destination, startPos;
    int cutCount = 0;
    Animator anim;

    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
        StartCoroutine(CheckAction());
    }

    IEnumerator CheckAction()
    {
        while (true)
        {
            DoSomething();
            Debug.Log(state);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void DoSomething()
    {
        switch (state)
        {
            case State.Idle:
                if (Game.transforms.Count > 0)
                {
                    float min = 10000f, tempFloat;
                    for (int i = 0; i < Game.transforms.Count; i++)
                    {
                        if ((tempFloat = Vector3.Distance(transform.position, Game.transforms[i])) < min)
                        {
                            min = tempFloat;
                            destination = Game.transforms[i];
                        }
                    }
                    agent.speed = 5;
                    agent.SetDestination(destination);
                    state = State.RunningEmpty;
                    anim.SetInteger("action", 1);
                }
                else
                {
                    if (anim.GetInteger("action") != 0)
                        anim.SetInteger("action", 1);
                }
                break;
            case State.RunningEmpty:
                if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
                {
                    state = State.Cutting;
                    cutCount = 0;
                    anim.SetInteger("action", 2);
                }
                break;
            case State.Cutting:
                if (cutCount == 3)
                {
                    state = State.Loaded;
                }
                break;
            case State.Loaded:
                agent.speed = 3f;
                agent.SetDestination(startPos);
                state = State.RunningLoaded;
                Game.transforms.Remove(destination);
                destination.x += 1.5f;
                destination.z += 1.5f;
                PoolManager.Instance.DestroyObj(Game.instanceID, destination);
                anim.SetInteger("action", 3);
                break;
            case State.RunningLoaded:
                if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
                {
                    state = State.Idle; 
                    anim.SetInteger("action", 0);
                }
                break;
            default:
                break;
        }
    }

    void Cut()
    {
        cutCount++;
    }
}
