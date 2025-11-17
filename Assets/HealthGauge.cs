//タグEnemyは不要
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthGauge : MonoBehaviour
{
    // ヘルスゲージのメイン画像
    [SerializeField]
    private Image healthImage;

    // 遅延して減るエフェクト用の画像（バーンアウト）
    [SerializeField]
    private Image burnImage;

    // アニメーションの基本の長さ
    public float duration = 0.5f;

    /// <summary>
    /// ヘルスゲージの割合を設定し、アニメーションを実行する
    /// </summary>
    /// <param name="value">設定する割合 (0.0f ～ 1.0f)</param>
    public void SetGauge(float value)
    {
        // 値を0から1の間に収める
        value = Mathf.Clamp01(value);

        // 現在のゲージの割合を取得
        float currentFillAmount = healthImage.fillAmount;
        
        // 減少していない場合はアニメーションをスキップ
        if (value >= currentFillAmount)
        {
            healthImage.fillAmount = value;
            burnImage.fillAmount = value;
            return;
        }

        // DoTweenを使ってゲージを滑らかに減らす
        healthImage.DOFillAmount(value, duration)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, duration / 2f)
                    .SetDelay(0.5f);
            });
    }
}