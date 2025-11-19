using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthGuage : MonoBehaviour
{
    // メインカメラのTransformをキャッシュするための変数
    private Transform mainCameraTransform;

    // ヘルスゲージのメイン画像
    [SerializeField]
    private Image healthImage;

    // 遅延して減る画像（バーンアウト）
    [SerializeField]
    private Image burnImage;

    // アニメーションの基本の長さ
    public float duration = 0.5f;

    void Start()
    {
        // Start()でメインカメラのTransformを一度取得しておく
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (mainCameraTransform == null) return;
        
        // HPバーの回転を、カメラの回転に合わせて常にカメラのほうを向かせる
        transform.rotation = mainCameraTransform.rotation;
    }

    // ヘルスゲージの割合を設定し、アニメーションを実行する

    public void SetGauge(float value)
    {
        // 値を0から1の間に収める
        value = Mathf.Clamp01(value);

        // 現在のゲージの割合を取得
        float currentFillAmount = healthImage.fillAmount;
        
        // 減少していない場合はアニメーションをスキップ
        if (value >= currentFillAmount)
        {
            // 増加する場合も即座に更新 (増加時にバーンアウト画像は追従させる)
            healthImage.fillAmount = value;
            burnImage.fillAmount = value;
            return;
        }

        // 減少する場合の処理 (healthImageが先に減り、burnImageが遅れて追従)
        
        // 既存のアニメーションがあれば強制終了
        DOTween.Kill(burnImage);

        // メインHPバーをすぐに新しい値に更新
        healthImage.fillAmount = value; 
        
        // バーンアウト画像を滑らかに減らす
        burnImage
            .DOFillAmount(value, duration / 2f)
            .SetDelay(0.5f); // メインバーが減ってから0.5秒後にアニメーション開始
    }

     void OnDestroy()
    {
        // burnImageを対象とするDOTweenアニメーションをすべて停止する
        // これにより、オブジェクト破棄後にアニメーションが実行されるのを防ぐ
        DOTween.Kill(burnImage); 
    }
}