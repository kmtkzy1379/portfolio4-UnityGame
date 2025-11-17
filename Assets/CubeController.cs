//タグPlayer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    Rigidbody Rbody;

    public float work = 3.0f; // 歩行速度
    public float run = 6.0f;  // 走行速度
    public float jumpforce = 5.0f; // ジャンプ力
    public float rotationSpeed = 10.0f; // 回転速度

    public int MaxHp = 500;
    public int CurrentHp = 500;
    
    public Animator PlayerAnimator;
    
    public Collider WeaponCollider;

    // ★★★ 追加: HealthGaugeスクリプトを関連付けるための変数 ★★★
    public HealthGauge healthGauge;

    void Start()
    {
        // Rigidbodyコンポーネントを取得
        Rbody = GetComponent<Rigidbody>();

        CurrentHp = MaxHp;
        
        // ★★★ 追加: ゲーム開始時にHPバーを最大にする ★★★
        if (healthGauge != null)
        {
            healthGauge.SetGauge((float)CurrentHp / MaxHp);
        }
    }

    void Update()
    {
        // --- 変数の定義 ---
        // Shiftキーが押されているかどうかの判定を毎フレーム行う
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isShiftPressed ? run : work; // Shiftが押されていればrun、そうでなければworkの速度を適用

        // --- 移動処理 ---

        // 前後移動
        // 前進
        if (Input.GetKey("up") == true)
        {
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }

        // 後退
        if (Input.GetKey("down") == true)
        {
            transform.position -= transform.forward * currentSpeed * Time.deltaTime;
        }

        // 左右移動(カニ歩きだから消すかも)
        // 右歩き
        if (Input.GetKey("right") == true)
        {
            transform.position += transform.right * currentSpeed * Time.deltaTime;
        }
        // 左歩き
        if (Input.GetKey("left") == true)
        {
            transform.position -= transform.right * currentSpeed * Time.deltaTime;
        }

        // --- 回転処理 ---

        // 左回転 (Aキー)
        if (Input.GetKey("a") == true)
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }

        // 右回転 (Dキー)
        if (Input.GetKey("d") == true)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        // --- ジャンプ処理 ---

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            Rbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
        //攻撃処理
        if (Input.GetKeyDown("c") == true)
        {
            PlayerAnimator.SetBool ("Attack",true);
        }
    }
    //ダメージ処理
    private void OnTriggerEnter(Collider other)
    {
        //タグがEnemyなら
        if (other.CompareTag("Enemy"))
        {
            //Enemyスクリプトからdamage関数を貰いPdamageに代入
            Enemy Pdamage = other.GetComponent<Enemy>();
            if (Pdamage != null)
            {
                TakeDamage(Pdamage.damage);
            }
        }
    }
    //hp減らす処理
    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        Debug.Log("Player HP: " + CurrentHp);

        // ★★★ 追加: HPバーの表示を更新する処理 ★★★
        if (healthGauge != null)
        {
            // 現在のHPの割合を計算して、SetGaugeメソッドに渡す
            healthGauge.SetGauge((float)CurrentHp / MaxHp);
        }

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
    }

    void WeaponOn()
    {
        WeaponCollider.enabled = true;
    }

    void WeaponOff()
    {
        WeaponCollider.enabled = false;
        PlayerAnimator.SetBool("Attack",false);
    }
}