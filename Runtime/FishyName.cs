using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishyName : MonoBehaviour {
    [Tooltip("How high the text should be above the players head")]
    public float Offset = 0.17f;
    public Transform Head;

    private void Update() {
        transform.position = Head.position + new Vector3(0, Offset, 0);

        Vector3 direction = FishyManager.Manager.Head.position - transform.position;
        Quaternion quaternion = new Quaternion(0, Quaternion.LookRotation(direction).y, 0, Quaternion.LookRotation(direction).w);
        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 10 * Time.deltaTime);
    }
}