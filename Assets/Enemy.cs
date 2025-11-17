//タグEnemy
using UnityEngine;

// HealthGauge が別のスクリプトにある前提
public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp = 100;
    public int damage = 10; // 衝突した相手にダメージを与える変数

    // **↓↓ クラス内に入れる ↓↓**
    public EnemyHealthGuage healthGauge; 
    private Rigidbody Rbody; // Rbodyがどこかで使われているため追加

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

    private void OnTriggerEnter(Collider other)
    {
        //タグがWeaponなら
        if (other.CompareTag("Weapon"))
        {
            //Weaponスクリプトからdamage関数を貰いPdamageに代入
            Weapon Edamage = other.GetComponent<Weapon>();
            if (Edamage != null)
            {
                // TakeDamageに Weapon の damage を渡す
                TakeDamage(Edamage.SwordDamage);
            }
        }
    }

    //hp減らす処理
    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        Debug.Log("Enemy HP: " + CurrentHp);

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
        Debug.Log("Enemy died.");
        // 例: 死亡時にオブジェクトを破壊
        Destroy(gameObject); 
    }
    // **↑↑ クラス内に入れる ↑↑**
}
