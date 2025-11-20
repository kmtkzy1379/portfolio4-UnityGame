//タグEnemy
using UnityEngine;

// HealthGauge が別のスクリプトにある前提
public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp = 100;
    public int damage = 10;
    public float EnemySpeed = 3f;
    public float lostTargetDistance = 10f;

    public GameObject Target { get; private set; }

    public EnemyHealthGauge healthGauge;
    private Rigidbody Rbody;

    void Start()
    {
        Rbody = GetComponent<Rigidbody>();

        CurrentHp = MaxHp;

        // ゲーム開始時にHPバーを最大にする
        if (healthGauge != null)
        {
            healthGauge.SetGauge((float)CurrentHp / MaxHp);
        }
    }

    void Update()
    {
        // ターゲットがいる場合の処理
        if (Target)
        {
            // ターゲットとの距離を計算
            float distance = Vector3.Distance(transform.position, Target.transform.position);

            // ターゲットが一定距離離れたら、ターゲットを解除
            if (distance > lostTargetDistance)
            {
                ClearTarget();
                return; // ターゲットを解除したら、以降の追尾処理は行わない
            }

            // ターゲットの方向を向く
            transform.LookAt(Target.transform);

            // ターゲットに向かって移動
            this.transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
        }
    }

    // ターゲットを設定するパブリックメソッド
    public void SetTarget(GameObject newTarget)
    {
        Target = newTarget;
    }

    // ターゲットを解除するパブリックメソッド
    public void ClearTarget()
    {
        Target = null;
    }

    // ダメージ判定のみを担当
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Weapon Edamage = other.GetComponent<Weapon>();
            if (Edamage != null)
            {
                TakeDamage(Edamage.SwordDamage);
            }
        }
    }

    // hp減らす処理
    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        Debug.Log("Enemy HP: " + CurrentHp);

        if (healthGauge != null)
        {
            healthGauge.SetGauge((float)CurrentHp / MaxHp);
        }

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}