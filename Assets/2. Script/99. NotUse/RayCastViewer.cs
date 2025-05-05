using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastViewer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10f);
    }
}
