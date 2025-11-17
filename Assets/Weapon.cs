//タグWeapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // **↓↓ クラスのフィールドとして定義する ↓↓**
    public int SwordDamage = 50; // 武器のダメージ量

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Enemyにダメージを与える処理は Enemy 側で行っているため、
            // ここでは特に処理は不要か、または武器側で何かしたい場合に記述。
            // 既に Enemy スクリプト側でダメージ処理が実装されているため、
            // このブロックはなくても機能します。
        }
    }
}
