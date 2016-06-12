using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class PathFinding
    {
        private Tile[,] _tileArray;
        private GameOptions _gameOptions;
        private int _scaledTile;
        public PathFinding(Tile[,] tileArray)
        {
            _tileArray = tileArray;
            _gameOptions = new GameOptions();
            _scaledTile = _gameOptions.scaledTile;
        }

        private IEnumerable<Vector2> GetNeighborNodes(Vector2 node)
        {
            var nodes = new List<Vector2>();
            var x = (int)Math.Floor(node.X / _scaledTile);
            var y = (int)Math.Floor(node.Y / _scaledTile);
            //up
            if (y != 0)
            {
                if (_tileArray[x, y - 1].IsPassable)
                {
                    nodes.Add(new Vector2(node.X, node.Y - 1));
                }
            }

            //right
            if (x != _tileArray.GetLength(0))
            {
                if (_tileArray[x + 1, y].IsPassable)
                {
                    nodes.Add(new Vector2(node.X + 1, node.Y - 1));
                }
            }


            //down
            if (y != _tileArray.GetLength(1))
            {
                if (_tileArray[x, y + 1].IsPassable)
                {
                    nodes.Add(new Vector2(node.X, node.Y + 1));
                }
            }


            //left
            if (x != 0)
            {
                if (_tileArray[x - 1, y].IsPassable)
                {
                    nodes.Add(new Vector2(node.X - 1, node.Y));
                }
            }

            return nodes;
        }

        private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Vector2> { current };
            }

            var path = ReconstructPath(cameFrom, cameFrom[current]);
            path.Add(current);
            return path;
        }

        public List<Vector2> PathFind(Vector2 start, Vector2 end) 
        {
            var closedSet = new List<Vector2>();
            var openSet = new List<Vector2> { start };

            var cameFrom = new Dictionary<Vector2, Vector2>();
            var currentDistance = new Dictionary<Vector2, int>();
            var predictedDistance = new Dictionary<Vector2, float>();

            currentDistance.Add(start, 0);
            predictedDistance.Add(
                start,
                0 + +Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)
            );

            while (openSet.Count > 0)
            {
                var current = (
                    from p in openSet orderby predictedDistance[p] ascending select p
                ).First();

                if (current.X == end.X && current.Y ==end.Y)
                {
                    return ReconstructPath(cameFrom, end);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighborNodes(current))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;

                    if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor])
                    {
                        continue;
                    }

                    if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.Keys.Contains(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor]
                                                    + Math.Abs(neighbor.X - end.X)
                                                    + Math.Abs(neighbor.Y - end.Y);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            return ReconstructPath(cameFrom, end);
        }
    }
}
