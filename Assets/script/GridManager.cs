using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public float cellSize = 1.0f;
    private List<Tile> tiles = new List<Tile>();

    public delegate void BlackTileCountChanged(int newCount);
    public static event BlackTileCountChanged OnBlackTileCountChanged;

    // 新增统计功能
    private int blackTileCount;
    public Text counterText; // 关联UI文本


    void Start()
    {
        GenerateGrid();
        UpdateCounter(); // 初始化显示
        //Debug.Log($"生成的Tile数量：{tiles.Count}"); // 应该输出81
        //Debug.Log($"第一个Tile是否为空：{tiles[0] == null}"); // 应该输出False
        UpdateGrid("000101000000101000000111000000010000001111100000010000000111000000101000000111000");

    }

    void Update()
    {
        //Debug.Log(blackTileCount);
    }

    public Vector2 gridOffset = new Vector2(5, 5); // 自定义偏移量
    void GenerateGrid()
    {
        tiles.Clear(); // 清除旧数据

        float startPos = -4 * cellSize;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Vector2 pos = new Vector2(
                    startPos + x * cellSize + gridOffset.x,
                    startPos + y * cellSize + gridOffset.y
                );

                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                Tile tileComponent = tile.GetComponent<Tile>();

                // 添加空引用保护（必须在订阅事件之前检查，否则空引用时这里会先崩溃）
                if (tileComponent == null)
                {
                    Debug.LogError($"预制体 {tilePrefab.name} 缺少Tile脚本！", tile);
                    continue;
                }

                tiles.Add(tileComponent);
                tileComponent.OnStateChanged += HandleTileStateChange; // 注册事件

                // 核心修改部分：设置初始状态
                int currentIndex = y * 9 + x; // 计算二维坐标对应的线性索引
                if (currentIndex >= 38 && currentIndex <= 42) // 39-43号格子
                {
                    tileComponent.SetState(1);
                }
            }
        }
    }

    private void HandleTileStateChange(Tile tile)
    {
        UpdateCounter(); // 每次点击都会触发计数更新
    }
    // 新增方法：更新计数器
    public void UpdateCounter()
    {
        blackTileCount = 0;
        foreach (Tile tile in tiles)
        {
            if (tile.GetState() == 1) blackTileCount++;
        }

        if (counterText != null)
            counterText.text = $"当前攻击力: {blackTileCount}/81";

        // 触发事件
        OnBlackTileCountChanged?.Invoke(blackTileCount);
    }

    public void UpdateGrid(string input)
    {
        // 添加空引用检查
        if (tiles == null || tiles.Count == 0)
        {
            //Debug.LogError("Tiles列表未初始化！");
            return;
        }

        if (input.Length != 81)
        {
            Debug.LogError("需要81位二进制字符串");
            return;
        }

        for (int i = 0; i < 81; i++)
        {
            if (input[i] == '1') tiles[i].SetState(1);
            else if (input[i] == '0') tiles[i].SetState(0);
            //else Debug.LogError($"第{i}位字符无效");
        }
        // 新增更新计数
        UpdateCounter();
    }
}