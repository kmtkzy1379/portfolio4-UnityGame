using UnityEngine;

/// プレイヤーの周囲に敵をスポーンさせるシステム。
public class EnemySpawner : MonoBehaviour
{
    [Header("参照オブジェクト")]
    [Tooltip("スポーンさせる敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("プレイヤー設定")]
    [Tooltip("追跡するプレイヤーのタグ")]
    public string playerTag = "Player";

    [Header("スポーン設定")]
    [Tooltip("スポーン間隔（秒）")]
    public float spawnInterval = 30f;

    [Tooltip("スポーン範囲の最小半径（プレイヤーからの最短距離）")]
    public float minSpawnRadius = 5f;

    [Tooltip("スポーン範囲の最大半径（プレイヤーからの最長距離）")]
    public float maxSpawnRadius = 10f;

    [Tooltip("同時に存在する敵の最大数")]
    public int maxEnemies = 5;

    [Tooltip("ゲーム開始時の敵の数")]
    public int initialEnemies = 1;
    
    [Header("敵の管理")]
    [Tooltip("敵を識別するためのタグ")]
    public string enemyTag = "Enemy";


    // --- プライベート変数 ---
    private Transform playerTransform; // プレイヤーのTransformコンポーネント
    private float spawnTimer;          // スポーンタイミングを計るタイマー

    // ゲーム開始時に一度だけ呼び出される初期化処理
    void Start()
    {
        // タグを頼りにプレイヤーのGameObjectを検索
        GameObject player = GameObject.FindWithTag(playerTag);

        if (player != null)
        {
            // プレイヤーが見つかったら、そのTransformコンポーネントを保存
            playerTransform = player.transform;
        }

        // ゲーム開始時に指定された数の敵をスポーンさせる
        for (int i = 0; i < initialEnemies; i++)
        {
            SpawnEnemy();
        }

        // スポーンタイマーをリセット
        spawnTimer = 0f;
    }

    // フレームごとに呼び出される更新処理
    void Update()
    {
        // プレイヤーが存在しなくなった場合は処理を中断
        if (playerTransform == null) return;

        // 現在フィールドにいる敵の数をタグで検索して取得
        int currentEnemyCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        // 敵の数が最大数未満の場合のみ、スポーン処理を考慮
        if (currentEnemyCount < maxEnemies)
        {
            // タイマーを加算
            spawnTimer += Time.deltaTime;

            // タイマーが指定したスポーン間隔を超えたら
            if (spawnTimer >= spawnInterval)
            {
                // 敵を一体スポーンさせる
                SpawnEnemy();
                // タイマーをリセット
                spawnTimer = 0f;
            }
        }
    }

    // 敵を一体スポーンさせる処理
    void SpawnEnemy()
    {
        // 現在の敵の数が最大数以上なら、何もしない
        if (GameObject.FindGameObjectsWithTag(enemyTag).Length >= maxEnemies)
        {
            return;
        }

        // スポーンさせるランダムな位置を取得
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // 敵プレハブを、計算した位置に、回転させずに生成
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // プレイヤーの周囲のドーナツ型範囲内にランダムな位置を計算して返す
    // スポーンさせるべき位置座標
    private Vector3 GetRandomSpawnPosition()
    {
        // 0度から360度までのランダムな角度をラジアン単位で取得
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        // 最小半径と最大半径の間のランダムな距離を取得
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // 角度と距離からX,Z座標を計算
        float x = distance * Mathf.Cos(angle);
        float z = distance * Mathf.Sin(angle);
        
        // プレイヤー座標を加えてスポーン位置を決定
        // Y座標はプレイヤーと同じ高さに設定
        Vector3 spawnPosition = playerTransform.position + new Vector3(x, 0, z);

        return spawnPosition;
    }
}