using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private Enemy parentEnemy;

    void Start()
    {
        // 親オブジェクトの Enemy スクリプトを取得
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが追尾範囲に入ったとき
        if (other.CompareTag("Player"))
        {
            // ターゲットがまだ設定されていない場合のみ設定する
            if (parentEnemy.Target == null)
            {
                parentEnemy.SetTarget(other.gameObject);
            }
        }
    }
}