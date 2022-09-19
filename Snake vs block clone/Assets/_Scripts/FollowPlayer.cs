using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _followObject;

    private void LateUpdate()
    {
        var followZ = _followObject.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, followZ);
    }

}
