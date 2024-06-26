using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable() {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // // Update is called once per frame
    // 기존 방식
    // void Update()
    // {
    //     if (!GameManager.instance.isLive) {
    //         return;
    //     }
    //     inputVec.x = Input.GetAxisRaw("Horizontal");
    //     inputVec.y = Input.GetAxisRaw("Vertical");
    // }

    // input system 방식
    void OnMove(InputValue value) {
        inputVec = value.Get<Vector2>();
    }

    void FixedUpdate() {
        if (!GameManager.instance.isLive) {
            return;
        }
        // 힘을 준다.
        // rigid.AddForce(inputVec);

        // 속도 제어
        // rigid.velocity = inputVec;

        // 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        Debug.Log(gameObject.transform.position);
    }

    // Update가 끝난이후 적용
    void LateUpdate() {
        if (!GameManager.instance.isLive) {
            return;
        }
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (!GameManager.instance.isLive) {
            return;
        }

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0) {
            for (int i = 2; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
