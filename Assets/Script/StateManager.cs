using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum currentState {Idle, Rotate, Scale }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator IdleState()
    {
        
        //애니메이션
        yield return null;
    }

    IEnumerator Rotate()
    {
        
        //애니메이션 + 움직임
        yield return null;
    }

    IEnumerator Scale()
    {
        
        // 크기 변경
        yield return null;
    }
}
