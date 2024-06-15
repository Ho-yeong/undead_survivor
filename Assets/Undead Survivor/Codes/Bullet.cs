using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) {
        this.damage = damage;
        this.per = per;

        // 원거리
        if (per > -1) {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy") || per == -1) {
            return;
        }    

        per--;

        if (per == -1) {
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}