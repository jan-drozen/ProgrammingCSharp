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
            using var sr = new StreamReader("testFile02.txt");
            var cols = Int32.Parse(sr.ReadLine());
            var rows = Int32.Parse(sr.ReadLine());
            var mapStub = new string[rows];

            GameState initialState = null;
            var mapList = new List<byte[]>(rows);
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
                    initialState = new GameState(new Point(i, index), direction);
                    return (byte)0;
                });
                mapList.Add(byteArray.ToArray());
            }
            var game = new Game(cols, rows, mapList.ToArray(), initialState);

            for (int i = 0; i < 2000000; i++)
            {
                game.DoMove();
                Thread.Sleep(300);
            }
        }

    }

    class Game
    {
        public int Columns { get; }
        public int Rows { get; }
        public byte[][] Map { get; }
        public GameState State { get; private set; }

        public Game(int columns, int rows, byte[][] map, GameState initialState)
        {
            Columns = columns;
            Rows = rows;
            Map = map;
            State = initialState;
        }

        public void DoMove()
        {
            var pointAhead = GetPointAhead(State.Position, State.Direction);
            var pointAheadRight = GetRightHandFieldValue(pointAhead, State.Direction);
            var pointRight = GetRightHandFieldValue(State.Position, State.Direction);
            //pokud muzu jit rovne a po prave ruce bude zed, tak jdu rovne
            if (CanGoForward() && pointAheadRight == 1)
                GoForward();
            //pokud nemam po prave ruce zed, tak se otocim doprava(po smeru hod. rucicek) (v minulem kroku jsem ji tam jeste musel mit)
            else if (pointRight == 0)
                TurnClockwise();
            //pokud mam po prave ruce zed, ale nemuzu jit rovne, tak se otocim proti smeru hod. rucicek
            else if (pointRight == 1 && !CanGoForward())
                TurnCounterclockwise();
            else if (CanGoForward())
                GoForward();
        }

        private void WriteStateOut()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (State.Position.Row == i && State.Position.Column == j)
                    {
                        sb.Append(GetDirectionString(State.Direction));
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

        private void GoForward()
        {            
            State = new GameState(GetPointAhead(State.Position, State.Direction), State.Direction);
            WriteStateOut();
        }

        private void TurnClockwise()
        {            
            State = new GameState(State.Position, TurnClockwise(State.Direction));
            WriteStateOut();
        }

        private void TurnCounterclockwise()
        {            
            State = new GameState(State.Position, TurnCounterclockwise(State.Direction));
            WriteStateOut();
        }

        private Direction TurnClockwise(Direction direction)
        {
            return (Direction)((((int)direction) + 1) % 4);
        }
        private Direction TurnCounterclockwise(Direction direction)
        {
            return (Direction)((((int)direction+4) - 1) % 4);
        }

        private bool CanGoForward()
        {
            var pointAhead = GetPointAhead(State.Position, State.Direction);
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
