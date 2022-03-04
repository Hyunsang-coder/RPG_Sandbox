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
            // 첫 번째 자식 오브젝트의 vector3에서 두 번째 오브젝트의 vector3까지 선으로 연결
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
        // 0 - 1 - 2 - 0 - 1 - 2가 반복

    }

    public Vector3 GetWaypointPosition(int i)
    {
        return transform.GetChild(i).transform.position;
        // i 번째 자식 오브젝트의 vector3 값을 return
    }
}

