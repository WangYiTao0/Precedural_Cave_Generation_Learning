using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
    }

    private void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    Gizmos.color = squareGrid.squares[x, y].topLeft.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[x, y].topRight.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[x, y].bottomRight.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.squares[x, y].BottomLeft.active ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].BottomLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreTop.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreRight.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreBottom.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreLeft.position, Vector3.one * 0.15f);
                }
            }
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2f + x * squareSize + squareSize / 2f, 0, -mapHeight / 2f + y * squareSize + squareSize / 2f);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);   // map[x,y] == 1 Wall   active;
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];

            for (int x = 0; x < nodeCountX -1; x++)
            {
                for (int y = 0; y < nodeCountY -1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x+1, y], controlNodes[x, y]);  //BottomLeft(x,y)
                }
            }
        }
    }


    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, BottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _BottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            BottomLeft = _BottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = BottomLeft.right;
            centreLeft = BottomLeft.above;

        }
    }


    public class Node  // Contain Node pos and VertexIndex
    {

        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {
        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;

            above = new Node(position + Vector3.forward * squareSize /2f);  // 0,0,1
            right = new Node(position + Vector3.right * squareSize / 2f);  //1,0,0

        }
        public bool active;
        public Node above, right;
    }
}
