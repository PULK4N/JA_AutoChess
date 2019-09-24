using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Dijkstra
{
    private static int _rows = 8; // vertical size of the map
    private static int _columns = 8; // horizontal size size of the map

    private static BoardManager.FigureSet _graph;

    private static int _possibleDir = 8;
    private static int[] dx = { -1, -1, 1, 1, 0, -1, 0, 1 };
    private static int[] dy = { -1, 1, 1, -1, -1, 0, 1, 0 };

    private List<Point> path;

    public static void SetGraph(BoardManager.FigureSet graph)
    {
        _graph = graph;
    }

    public void ChangeRowAndColumnNumber(int rows, int columns)
    {
        _rows = rows;
        _columns= columns;
    }

    private static Point MinimumDistance(int[,] distance, bool[,] shortestPathTreeSet)
    {
        int min = int.MaxValue, min_indexRow = -1, min_indexColumn = -1;
        Point minIndex;

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (shortestPathTreeSet[i, j] == false && distance[i, j] <= min)
                {
                    min = distance[i, j];
                    min_indexRow = i;
                    min_indexColumn = j;
                }
            }
        }
        minIndex.X = min_indexRow;
        minIndex.Y = min_indexColumn;

        return minIndex;
    }

    public Figure EnemyInsideRange(Figure source, int row=-1, int column=-1)
    {
        int currentRow = row == -1 ? source.Position.Row : row;
        int currentColumn = column == -1 ? source.Position.Column : column;
        List<Figure> enemies = new List<Figure>();
        int range = source.Range;
        for(int distance=1;distance<=source.Range; ++distance)
        {
            for(int i=-distance;i<=distance;i++)
            {
                if (currentColumn + i >= 0 && currentColumn + i <= 7)
                {
                    Figure figure;
                    if (currentRow + distance - System.Math.Abs(i) >= 0 && currentRow + distance - System.Math.Abs(i) <= 7)
                    {
                        Range.Add(new Point(currentRow + distance - System.Math.Abs(i), currentColumn + i));
                        figure = _graph[currentRow + distance - System.Math.Abs(i), currentColumn + i];
                        if (figure != null)
                        {
                            if (figure.Owner != source.Owner)
                                enemies.Add(figure);
                        }
                    }
                    if (currentRow - distance + System.Math.Abs(i) >= 0 && currentRow - distance + System.Math.Abs(i) <= 7)
                    {
                        Range.Add(new Point(currentRow - distance + System.Math.Abs(i), currentColumn + i));
                        figure = _graph[currentRow - distance + System.Math.Abs(i), currentColumn + i];
                        if (figure != null)
                        {
                            if (figure.Owner != source.Owner)
                                enemies.Add(figure);
                        }
                    };
                }
            }
        }
        if (enemies.Count > 0)
            return enemies[0];
        else
            return null;
    }
    public List<Point> Range = new List<Point>();

    public Point FindNextStep(Figure source)
    {
        int[,] distance = new int[_rows, _columns];
        bool[,] shortestPathTreeSet = new bool[_rows, _columns];
        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _columns; ++j)
            {
                distance[i,j] = int.MaxValue;
                shortestPathTreeSet[i,j] = false;
            }
        }

        distance[source.Position.Row, source.Position.Column] = 0;

        Point u = new Point(source.Position.Row, source.Position.Column);
        while (EnemyInsideRange(source, u.X, u.Y) == null)
        {
            u = MinimumDistance(distance, shortestPathTreeSet);
            if(u.X <0 || u.Y<0)
                return new Point(-1, -1); // path not found

            if (distance[u.X, u.Y] == int.MaxValue)
            {
                return new Point(-1, -1); // path not found
            }

            shortestPathTreeSet[u.X, u.Y] = true;

            for (int counter = 0; counter < _possibleDir; ++counter)
            {
                int i = u.X + dx[counter], j = u.Y + dy[counter];
                if (i >= 0 && i < _rows && j >= 0 && j < _columns)
                    if (!shortestPathTreeSet[i, j] && _graph[i, j] == null && distance[u.X, u.Y] != int.MaxValue
                        && distance[u.X, u.Y] + 1 < distance[i, j])
                        distance[i, j] = distance[u.X, u.Y] + 1;
            }
        }

        return MakePath(source, u, distance)[0];
    }

    private List<Point> MakePath(Figure source, Point goal, int[,] distance)
    {
        int x = goal.X, y = goal.Y;
        List<Point> path = new List<Point>();
        path.Add(goal);
        int cnt = 0;
            Point pathNode = new Point();
        while (distance[x,y]!=0)
        {
            int place = 0;
            for (int i = 0; i < _possibleDir; ++i)
            {
                int xdx = x + dx[i];
                int ydy = y + dy[i];
                if (!(xdx < 0 || xdx >= _rows || ydy < 0 || ydy >= _columns))
                    if (distance[x, y] == 1 + distance[xdx, ydy] && (_graph[xdx, ydy] == null || _graph[xdx, ydy] ==source))
                    {
                        if(_graph[xdx, ydy] == source)
                            break;
                        pathNode.X = xdx;
                        pathNode.Y = ydy;
                        place = i;
                    }
            }
            path.Add(pathNode);
            x += dx[place];
            y += dy[place];
            
            if (cnt++ >= 10)
                break;
        }
        
        foreach (Point point in path)
            Debug.Log(point.X + "," + point.Y);
        path.Reverse();
        return path;
    }
}
