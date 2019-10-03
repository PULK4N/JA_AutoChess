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

    public static bool IsEnemyInRange(Figure source, Figure target)
    {
        List<Point> range = GetRange(source.Range, source.Position.Row, source.Position.Column);
        foreach (Point point in range)
            if (target.Position.Row == point.X && target.Position.Column == point.Y)
                return true;
        return false;
    }

    public static List<Point> GetRange(int range, int row, int column)
    {
        List<Point> tilesInRange = new List<Point>();
        for (int distance = 1; distance <= range; ++distance)
        {
            for (int i = -distance; i <= distance; i++)
            {
                if (column + i >= 0 && column + i <= 7)
                {
                    if (row + distance - System.Math.Abs(i) >= 0 && row + distance - System.Math.Abs(i) <= 7)
                    {
                        tilesInRange.Add(new Point(row + distance - System.Math.Abs(i), column + i));
                    }
                    if (row - distance + System.Math.Abs(i) >= 0 && row - distance + System.Math.Abs(i) <= 7)
                    {
                        tilesInRange.Add(new Point(row - distance + System.Math.Abs(i), column + i));
                    }
                }
            }
        }
        return tilesInRange;
    }

    public static Figure EnemyInsideRange(Figure source, int range, int targetRow = -1, int targetColumn = -1,
        Enums.TargetingSystem targetingSystem = Enums.TargetingSystem.ClosestEnemy)
    {
        int currentRow = targetRow == -1 ? source.Position.Row : targetRow;
        int currentColumn = targetColumn == -1 ? source.Position.Column : targetColumn;
        List<Figure> enemies = new List<Figure>();
        List<Figure> allies = new List<Figure>();
        List<Point> tilesInRange = GetRange(range, currentRow, currentColumn);

        foreach(Point tile in tilesInRange)
        {
            Figure figure = _graph[tile.X, tile.Y];
            if (figure != null && !figure.Untargetable)
            {
                if (figure.Owner != source.Owner)
                    enemies.Add(figure);
                else
                    allies.Add(figure);
            }
        }

        Figure target = null;
        switch(targetingSystem)
        {
            case Enums.TargetingSystem.ClosestEnemy:
                if (enemies.Count == 0)
                    return null;
                enemies.Sort((o, e) => Mathf.Abs(o.Position.Row - e.Position.Row) +
                Mathf.Abs(o.Position.Column - e.Position.Column));
                target = enemies[0];
                break;
            case Enums.TargetingSystem.HighestEnemyDps:
                if (enemies.Count == 0)
                    return null;
                enemies.Sort((o, e) => o.DPS - e.DPS);
                target = enemies[0];
                break;
            case Enums.TargetingSystem.LowestAllyHp:
                if (allies.Count == 0)
                    return source;
                allies.Sort((o, e) => (int)(o.CurrentHealth - e.CurrentHealth));
                target = allies[0];
                break;
            default:
                return enemies[0];
        }
        return target;
    }

    public static Figure EnemyInsideRangeOptimised(Figure source, int sourceRange, int targetRow = -1, int targetColumn = -1)
    {
        int currentRow = targetRow == -1 ? source.Position.Row : targetRow;
        int currentColumn = targetColumn == -1 ? source.Position.Column : targetColumn;
        List<Figure> enemies = new List<Figure>();
        int range = sourceRange;
        for (int distance = 1; distance <= source.Range; ++distance)
        {
            for (int i = -distance; i <= distance; i++)
            {
                if (currentColumn + i >= 0 && currentColumn + i <= 7)
                {
                    Figure figure;
                    if (currentRow + distance - System.Math.Abs(i) >= 0 && currentRow + distance - System.Math.Abs(i) <= 7)
                    {
                        figure = _graph[currentRow + distance - System.Math.Abs(i), currentColumn + i];
                        if (figure != null)
                        {
                            if (!figure.Untargetable && figure.Owner != source.Owner)
                                enemies.Add(figure);
                        }
                    }
                    if (currentRow - distance + System.Math.Abs(i) >= 0 && currentRow - distance + System.Math.Abs(i) <= 7)
                    {
                        figure = _graph[currentRow - distance + System.Math.Abs(i), currentColumn + i];
                        if (figure != null)
                        {
                            if (!figure.Untargetable && figure.Owner != source.Owner)
                                enemies.Add(figure);
                        }
                    }
                }
            }
            if (enemies.Count > 0)
                break;
        }
        if (enemies.Count > 0)
            return enemies[0];
        else
            return null;
    }

    public static Point FindNextStep(Figure source, bool isKnight = false)
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
        while (EnemyInsideRangeOptimised(source, source.Range, u.X, u.Y) == null)
        {
            u = MinimumDistance(distance, shortestPathTreeSet);
            if (u.X < 0 || u.Y < 0)
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
        if(!isKnight)
            return MakePath(source, u, distance)[0];
        else
            return MakePath(source, u, distance, true)[0];
    }

    private static List<Point> MakePath(Figure source, Point goal, int[,] distance, bool isknight=false)
    {
        int x = goal.X, y = goal.Y;
        List<Point> path = new List<Point>();
        path.Add(goal);
        int cnt = 0;
        Point pathNode = new Point();
        while (distance[x, y] != 0)
        {
            Debug.Log(x + "," + y);
            int place = 0;
            for (int i = 0; i < _possibleDir; ++i)
            {
                int xdx = x + dx[i];
                int ydy = y + dy[i];
                if (!(xdx < 0 || xdx >= _rows || ydy < 0 || ydy >= _columns))
                    if (distance[x, y] == 1 + distance[xdx, ydy] && (_graph[xdx, ydy] == null || _graph[xdx, ydy] ==source))
                    {
                        if(_graph[xdx, ydy] == source)
                        {
                            if (!isknight)
                                path.Reverse();
                            return path;
                        }
                        pathNode.X = xdx;
                        pathNode.Y = ydy;
                        place = i;
                    }
            }
            path.Add(pathNode);
            x += dx[place];
            y += dy[place];
            Debug.Log(x + "," + y + "," +place);
            if (cnt++ >= 10)
                break;
        }
        if (!isknight)
            path.Reverse();
        return path;
    }

    public static Point KnightJumpOnStart(Figure source)
    {
        int row = 0;
        int column = source.Position.Column;
        List<Point> freeTilesInRange = new List<Point>();
        if (source.Position.Row < 4)
        {
            row = 7;
        }

        if (_graph[row, column] == null)
            return new Point(row, column);

        for (int distance = 1; distance <= 8; ++distance)
        {
            for (int i = -distance; i <= distance; i++)
            {
                if (column + i >= 0 && column + i <= 7)
                {
                    if (row + distance - System.Math.Abs(i) >= 0 && row + distance - System.Math.Abs(i) <= 7)
                    {
                        if (_graph[row + distance - System.Math.Abs(i), column] == null)
                            freeTilesInRange.Add(new Point(row + distance - System.Math.Abs(i), column + i));
                    }
                    if (row - distance + System.Math.Abs(i) >= 0 && row - distance + System.Math.Abs(i) <= 7)
                    {
                        if (_graph[row - distance + System.Math.Abs(i), column] == null)
                            freeTilesInRange.Add(new Point(row - distance + System.Math.Abs(i), column + i));
                    }
                }
                if (freeTilesInRange.Count > 0)
                    return freeTilesInRange[Random.Range(0, freeTilesInRange.Count)];
            }
        }
        return new Point(source.Position.Row, source.Position.Column);
    }
}
