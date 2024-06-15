using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake() {
        player = GetComponentInParent<Player>();
    }

    void Start() {
        Init();
    }

    void Update() {
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0;
                    Fire();
                }
                break;
        }

        // .. Test Code ..
        if (Input.GetButtonDown("Jump")) {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count) {
        this.damage = damage;
        this.count += count;

        if (id == 0) {
            Batch();
        }
    }

    public void Init() {
        switch (id) {
            case 0:
                // 회전속도
                speed = -150;
                Batch();
                break;
            default:
                // 연사속도
                speed = 0.3f;
                break;
        }
    }

    void Batch() {
        for (int i = 0; i < count; i++) {
            Transform bullet; 
            
            if (i < transform.childCount) {
                bullet = transform.GetChild(i);
            } else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  
            }
            

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity per
        }
    }

    void Fire() {
        if (!player.scanner.nearestTarget) {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        // 크기는 1로, 방향만 남기는 값으로 변경
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
