using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomGrid : MaskableGraphic
{
    public int rows = 3; // 行数
    public int columns = 9; // 列数
    public float cellSize = 40f; // 单元格尺寸
    public Color[] cellColors; // 存储每个单元格颜色（含透明度）

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float totalWidth = columns * cellSize;
        float totalHeight = rows * cellSize;

        // 生成网格单元
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2 startPos = new Vector2(j * cellSize, i * cellSize);
                Vector2 endPos = startPos + new Vector2(cellSize, cellSize);
                Color cellColor = cellColors[i * columns + j];

                // 添加四边形（根据颜色透明度决定是否显示）
                AddQuad(vh, startPos, endPos, cellColor);
            }
        }
    }

    void AddQuad(VertexHelper vh, Vector2 start, Vector2 end, Color color)
    {
        UIVertex vert = UIVertex.simpleVert;
        vert.color = color; // 颜色含透明度，alpha=0时完全透明[7](@ref)

        // 添加四个顶点
        vert.position = new Vector3(start.x, start.y);
        vh.AddVert(vert);
        vert.position = new Vector3(end.x, start.y);
        vh.AddVert(vert);
        vert.position = new Vector3(end.x, end.y);
        vh.AddVert(vert);
        vert.position = new Vector3(start.x, end.y);
        vh.AddVert(vert);

        // 添加三角形索引
        int baseIndex = vh.currentVertCount - 4;
        vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        vh.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    void overrideAwake()
    {
        cellColors = new Color[rows * columns];
        for (int i = 0; i < cellColors.Length; i++)
            cellColors[i] = new Color(1, 1, 1, 0); // alpha=0表示透明
    }

    public void SetCellColor(int row, int column, Color color)
    {
        int index = row * columns + column;
        cellColors[index] = color;
        SetVerticesDirty(); // 触发网格重绘[5](@ref)
    }

    public class GridInteractor : MonoBehaviour, IPointerClickHandler
    {
        public CustomGrid grid;

        public void OnPointerClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                grid.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            // 计算行列索引
            int column = Mathf.FloorToInt((localPoint.x + grid.rectTransform.rect.width / 2) / grid.cellSize);
            int row = Mathf.FloorToInt((localPoint.y + grid.rectTransform.rect.height / 2) / grid.cellSize);

            // 切换颜色（示例：点击后显示红色）
            if (row >= 0 && row < grid.rows && column >= 0 && column < grid.columns)
                grid.SetCellColor(row, column, new Color(1, 0, 0, 1)); // alpha=1为不透明
        }
    }
}




