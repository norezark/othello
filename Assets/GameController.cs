using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[,] board = new GameObject[8, 8]
    {
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null},
        {null,null,null,null,null,null,null,null}
    };


    // Use this for initialization
    void Start()
    {
        int half = board.GetLength(1) / 2;
        var cubeScale = piece.transform.localScale;
        board[half, half] = Instantiate(piece, new Vector3(half, cubeScale.y, half) + new Vector3(cubeScale.x / 2, 0, cubeScale.z / 2), Quaternion.identity);
        board[half, half].GetComponent<Renderer>().material.color = Color.white;
        board[half - 1, half] = Instantiate(piece, new Vector3(half - 1, cubeScale.y, half) + new Vector3(cubeScale.x / 2, 0, cubeScale.z / 2), Quaternion.identity);
        board[half - 1, half].GetComponent<Renderer>().material.color = Color.black;
        board[half, half - 1] = Instantiate(piece, new Vector3(half, cubeScale.y, half - 1) + new Vector3(cubeScale.x / 2, 0, cubeScale.z / 2), Quaternion.identity);
        board[half, half - 1].GetComponent<Renderer>().material.color = Color.black;
        board[half - 1, half -1] = Instantiate(piece, new Vector3(half - 1, cubeScale.y, half - 1) + new Vector3(cubeScale.x / 2, 0, cubeScale.z / 2), Quaternion.identity);
        board[half - 1, half - 1].GetComponent<Renderer>().material.color = Color.white;

    }

    public GameObject piece;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PutPiece(0);
        }
        if (Input.GetMouseButtonUp(1))
        {
            PutPiece(1);
        }
    }

    private void PutPiece(int button)
    {
        var cubeScale = piece.transform.localScale;
        var mousePos = Input.mousePosition;
        mousePos.z = 10f;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.x = Mathf.Floor(worldPos.x);
        worldPos.z = Mathf.Floor(worldPos.z);
        worldPos.y = cubeScale.y;

        int x = (int)worldPos.x;
        int z = (int)worldPos.z;
        if (x >= 0 && x < board.GetLength(1) && z >= 0 && z < board.GetLength(1)
            && !board[x, z])
        {
            var p = Instantiate(piece, worldPos + new Vector3(cubeScale.x / 2, 0, cubeScale.z / 2), Quaternion.identity);
            board[x, z] = p;
            var color = button == 0 ? Color.white : Color.black;
            p.GetComponent<Renderer>().material.color = color;
            CheckBoard(x, z, color);
        }
    }

    private void CheckBoard(int x, int z, Color center)
    {
        if (board[x, z] == null) return;

        for (int i = 1; i >= -1; i--)
        {
            for (int j = 1; j >= -1; j--)
            {
                CheckLine(x, z, i, j, center);
            }
        }
    }

    private void CheckLine(int x, int z, int xStep, int zStep, Color center)
    {
        if (xStep == 0 && zStep == 0) return;

        for (int i = x + xStep, j = z + zStep; true; i += xStep, j += zStep)
        {
            if (i < 0 || j < 0 || i >= board.GetLength(1) || j >= board.GetLength(1)) break;

            GameObject piece = board[i, j];
            if (piece == null) break;

            var target = piece.GetComponent<Renderer>().material.color;
            if (target == center)
            {
                for (; i != x || j != z; i -= xStep, j -= zStep)
                {
                    board[i, j].GetComponent<Renderer>().material.color = center;
                }
                break;
            }
        }
    }
}
