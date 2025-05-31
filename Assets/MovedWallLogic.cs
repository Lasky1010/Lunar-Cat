using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedWallLogic : MonoBehaviour
{
    [SerializeField] private float _wallSpeed;
    [SerializeField] private int Vector = 1;
    private void FixedUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(gameObject.transform.position.x + Vector, gameObject.transform.position.y, gameObject.transform.position.z),_wallSpeed * Time.fixedDeltaTime);
    }
}
