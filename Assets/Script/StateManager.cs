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
        
        //�ִϸ��̼�
        yield return null;
    }

    IEnumerator Rotate()
    {
        
        //�ִϸ��̼� + ������
        yield return null;
    }

    IEnumerator Scale()
    {
        
        // ũ�� ����
        yield return null;
    }
}
