using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // // Update is called once per frame
    // 기존 방식
    // void Update()
    // {
    //     inputVec.x = Input.GetAxisRaw("Horizontal");
    //     inputVec.y = Input.GetAxisRaw("Vertical");
    // }

    // input system 방식
    void OnMove(InputValue value) {
        inputVec = value.Get<Vector2>();
    }

    void FixedUpdate() {
        // 힘을 준다.
        // rigid.AddForce(inputVec);

        // 속도 제어
        // rigid.velocity = inputVec;

        // 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
}