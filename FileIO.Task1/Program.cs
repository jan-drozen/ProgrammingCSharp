using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace FileIO.Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using var sr = new StreamReader("test01.txt");
            var cols = Int32.Parse(sr.ReadLine());
            var rows = Int32.Parse(sr.ReadLine());
            var mapStub = new string[rows];

            var initialStates = new List<GameState>();
            byte[][] mapList = new byte[rows][];
            for (int i = 0; i < rows; i++)
            {
                var line = sr.ReadLine();
                var byteArray = line.ToCharArray().Select((char c, int index) =>
                {
                    Direction direction = Direction.Up;
                    if (c == 'X') return (byte)1;
                    if (c == '.') return (byte)0;
                    if (c == '>')
                        direction = Direction.Right;
                    if (c == 'v')
                        direction = Direction.Down;
                    if (c == '<')
                        direction = Direction.Left;
                    if (c == '^')
                        direction = Direction.Up;
                    initialStates.Add(new GameState(new Point(i, index), direction));
                    return (byte)0;
                });
                mapList[i]=byteArray.ToArray();
            }
            var game = new Game(cols, rows, mapList, initialStates.ToArray());
            var players = initialStates.Count;
            for (int i = 0; i < 2000000; i++)
            {
                for (int p = 0; p < players; p++)
                {
                    game.DoMove(p);
                }
                game.WriteStateOut();
                Thread.Sleep(1000);
            }
        }

    }

    class Game
    {
        public int Columns { get; }
        public int Rows { get; }
        public byte[][] Map { get; }
        public GameState[] States { get; private set; }        

        public Game(int columns, int rows, byte[][] map, GameState[] states)
        {
            Columns = columns;
            Rows = rows;
            Map = map;
            States = states;
        }

        public void DoMove(int order)
        {
            var State = States[order];
            var pointAhead = GetPointAhead(State.Position, State.Direction);
            var pointAheadRight = GetRightHandFieldValue(pointAhead, State.Direction);
            var pointRight = GetRightHandFieldValue(State.Position, State.Direction);
            //pokud muzu jit rovne a po prave ruce bude zed, tak jdu rovne
            if (CanGoForward(order) && pointAheadRight == 1)
                GoForward(order);
            //pokud nemam po prave ruce zed, tak se otocim doprava(po smeru hod. rucicek) (v minulem kroku jsem ji tam jeste musel mit)
            else if (pointRight == 0)
                TurnClockwise(order);
            //pokud mam po prave ruce zed, ale nemuzu jit rovne, tak se otocim proti smeru hod. rucicek
            else if (pointRight == 1 && !CanGoForward(order))
                TurnCounterclockwise(order);
            else if (CanGoForward(order))
                GoForward(order);
        }

        public void WriteStateOut()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var occupiedField = States.FirstOrDefault(s => s.Position.Row == i && s.Position.Column == j);
                    if (occupiedField != null)
                    {
                        sb.Append(GetDirectionString(occupiedField.Direction));
                        continue;
                    }
                    sb.Append(Map[i][j] == 0 ? "." : "X");                    
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
        }

        private string GetDirectionString(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return "v";
                case Direction.Left: return "<";
                case Direction.Right: return ">";
                default :return "^";
            }
        }

        private void GoForward(int order)
        {
            Map[States[order].Position.Row][States[order].Position.Column] = 0;
            States[order] = new GameState(GetPointAhead(States[order].Position, States[order].Direction), States[order].Direction);
            Map[States[order].Position.Row][States[order].Position.Column] = 1;
        }

        private void TurnClockwise(int order)
        {            
            States[order] = new GameState(States[order].Position, TurnClockwise(States[order].Direction));            
        }

        private void TurnCounterclockwise(int order)
        {
            States[order] = new GameState(States[order].Position, TurnCounterclockwise(States[order].Direction));
        }

        private Direction TurnClockwise(Direction direction)
        {
            return (Direction)((((int)direction) + 1) % 4);
        }
        private Direction TurnCounterclockwise(Direction direction)
        {
            return (Direction)((((int)direction+4) - 1) % 4);
        }

        private bool CanGoForward(int order)
        {
            var pointAhead = GetPointAhead(States[order].Position, States[order].Direction);
            return Map[pointAhead.Row][pointAhead.Column] == 0;
        }

        private Point GetPointAhead(Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return new Point(point.Row-1, point.Column);
                case Direction.Right: return new Point(point.Row, point.Column+1);
                case Direction.Down: return new Point(point.Row + 1, point.Column);
                default: return new Point(point.Row, point.Column-1);
            }
        }

        private byte GetRightHandFieldValue(Point point, Direction direction)
        {
            switch (direction) {
                case Direction.Up: return Map[point.Row][point.Column + 1];
                case Direction.Right: return Map[point.Row+1][point.Column];
                case Direction.Down: return Map[point.Row][point.Column -1];
                default: return Map[point.Row - 1][point.Column];
            }
        }
    }

    enum Direction { Up = 0, Right = 1, Down = 2, Left = 3}
    class Point
    {
        public int Row { get; }
        public int Column { get; }
        public Point(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
    class GameState
    {
        public Point Position { get; }
        public Direction Direction { get; }
        public GameState(Point position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
