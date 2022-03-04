using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    private void OnDrawGizmos()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextPoint(i);
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(GetWaypointPosition(i), 0.5f);
            Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));
            // ù ��° �ڽ� ������Ʈ�� vector3���� �� ��° ������Ʈ�� vector3���� ������ ����
        }

    }

    public int GetNextPoint(int i)
    {
        int j = i + 1;
        if (j == transform.childCount)
        {
            j = 0;
        }
        return j;
        // 0 - 1 - 2 - 0 - 1 - 2�� �ݺ�

    }

    public Vector3 GetWaypointPosition(int i)
    {
        return transform.GetChild(i).transform.position;
        // i ��° �ڽ� ������Ʈ�� vector3 ���� return
    }
}

