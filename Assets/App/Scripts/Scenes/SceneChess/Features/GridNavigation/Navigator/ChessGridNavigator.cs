using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции

            int[,] graph = new int[64, 64];
            int[,] game = new int[8, 8];
            for (int i = 0; i < game.GetLength(0); i++)
                for (int j = 0; j < game.GetLength(1); j++)
                    game[i, j] = i * 8 + j;

            List<List<(int,int)>> list;
            switch (unit)
            {
                case ChessUnitType.Pon:
                    list = new List<List<(int, int)>>()
                        {new List<(int, int)>() { (1, 0) }, new List<(int, int)>() { (-1, 0) } };
                    break;
                case ChessUnitType.King:
                    list = new List<List<(int, int)>>()
                        {new List<(int, int)>() { (1, 1) }, new List<(int, int)>() { (-1, -1) },
                        new List<(int, int)>() { (1, -1) }, new List <(int, int) >() {(-1, 1) },
                        new List<(int, int)>() { (1, 0) }, new List <(int, int) >() {(-1, 0) },
                        new List <(int, int) >() {(0, 1) }, new List <(int, int) >() {(0, -1) }};
                    break;
                case ChessUnitType.Queen:
                    list = new List<List<(int, int)>>()
                        {new List<(int, int)>() { (1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 7) },
                        new List<(int, int)>() { (-1, -1), (-2, -2), (-3, -3), (-4, -4), (-5, -5), (-6, -6), (-7, -7) },
                        new List<(int, int)>() { (1, -1), (2, -2), (3, -3), (4, -4), (5, -5), (6, -6), (7, -7) },
                        new List<(int, int)>() { (-1, 1), (-2, 2), (-3, 3), (-4, 4), (-5, 5), (-6, 6), (-7, 7) },
                        new List<(int, int)>() { (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0) },
                        new List<(int, int)>() { (-1, 0), (-2, 0), (-3, 0), (-4, 0), (-5, 0), (-6, 0), (-7, 0) },
                        new List<(int, int)>() { (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7) },
                        new List<(int, int)>() { (0, -1), (0, -2), (0, -3), (0, -4), (0, -5), (0, -6), (0, -7) }};
                    break;
                case ChessUnitType.Rook:
                    list = new List<List<(int, int)>>()
                        {new List<(int, int)>() { (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0) },
                        new List<(int, int)>() { (-1, 0), (-2, 0), (-3, 0), (-4, 0), (-5, 0), (-6, 0), (-7, 0) },
                        new List<(int, int)>() { (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7) },
                        new List<(int, int)>() { (0, -1), (0, -2), (0, -3), (0, -4), (0, -5), (0, -6), (0, -7) }};
                    break;
                case ChessUnitType.Knight:
                    list = new List<List<(int, int)>>()
                        { new List <(int, int) >() { (2, -1) }, new List<(int, int)>() { (2, 1) },
                        new List <(int, int) >() { (-2, 1) }, new List<(int, int)>() { (-2, -1) },
                        new List <(int, int) >() { (1, 2) }, new List<(int, int)>() { (1, -2) },
                        new List <(int, int) >() { (-1, 2) }, new List <(int, int) >() { (-1, -2) }};
                    break;
                case ChessUnitType.Bishop:
                    list = new List<List<(int, int)>>()
                        {new List<(int, int)>() { (1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 7) },
                        new List<(int, int)>() { (-1, -1), (-2, -2), (-3, -3), (-4, -4), (-5, -5), (-6, -6), (-7, -7) },
                        new List<(int, int)>() { (1, -1), (2, -2), (3, -3), (4, -4), (5, -5), (6, -6), (7, -7) },
                        new List<(int, int)>() { (-1, 1), (-2, 2), (-3, 3), (-4, 4), (-5, 5), (-6, 6), (-7, 7) }};
                    break;
                default:
                    list = new List<List<(int, int)>>();
                    break;
            }

            List<int> list_unit_pos = new List<int>();
            foreach (var un in grid.Pieces)
                list_unit_pos.Add(un.CellPosition.y * 8 + un.CellPosition.x);

            for (int i = 0; i < game.GetLength(0); i++)
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    foreach(var list_direction in list)
                        foreach (var delta in list_direction)
                        {
                            int new_row = i + delta.Item1,
                                new_col = j + delta.Item2;

                            if (new_row < 0 || new_row >= 8 || new_col < 0 || new_col >= 8) break;

                            int num = new_row * 8 + new_col;
                            if (list_unit_pos.Contains(num)) break;

                            graph[game[i, j], num] = 1;
                        }
                }

            int vertx_from = from.y * 8 + from.x,
                vertex_to = to.y * 8 + to.x;

            return Do_BFS(graph, vertx_from, vertex_to);
        }

        private void Path(int[,] a, int num, int[] mark, Stack<int> stek)
        {
            stek.Push(num);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[num, i] == 1 && mark[i] == mark[num] - 1)
                {
                    Path(a, i, mark, stek);
                    break;
                }
            }
        }

        private List<Vector2Int> Do_BFS(int[,] arr, int start, int search)
        {
            bool ans = false;
            bool[] visited = new bool[arr.GetLength(0)];
            int[] marker = new int[arr.GetLength(0)];
            Stack<int> st = new Stack<int>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                visited[i] = false;
                marker[i] = 0;
            }


            Queue<int> que = new Queue<int>();
            que.Enqueue(start);
            visited[start] = true;

            while (que.Count != 0)
            {
                int node = que.Dequeue();

                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    if (arr[node, i] == 1 && visited[i] == false)
                    {
                        que.Enqueue(i);
                        visited[i] = true;
                        marker[i] = marker[node] + 1;
                    }
                }

                if (node == search)
                {
                    ans = true;
                    st.Push(search);
                    for (int i = 0; i < arr.GetLength(0); i++)
                    {
                        if (arr[node, i] == 1 && marker[i] == marker[node] - 1)
                        {
                            Path(arr, i, marker, st);
                            break;
                        }
                    }
                }

                if(ans) break;
            }

            if (ans == true)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                while(st.Count > 0)
                {
                    int vert = st.Pop();
                    path.Add(new Vector2Int(vert % 8, vert / 8));
                }
                return path;
            }
            else
                return null;
        }
    }
}