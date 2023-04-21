namespace Program
{
    public class Runner
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++) Console.Clear();

            var game = new TicTacToe(State.X);
            game.Begin();
        }
    }
    public enum State
    {
        EMPTY,
        X,
        O
    }
    public class StateString
    {
        public static char? From(State state)
        {
            switch (state)
            {
                case State.EMPTY:
                    return '·';
                case State.X:
                    return 'X';
                case State.O:
                    return 'O';
            }
            return null;
        }
    }
    public class TicTacToe
    {
        private State turn;
        private State[,] board;

        public TicTacToe(State starting)
        {
            turn = starting;
            board = new State[3, 3];
        }

        public void Begin()
        {
            while (CountEmpties() != 0)
            {
                Ask();

                Console.Clear();
                Print();

                if (Check())
                {
                    Console.WriteLine($"Winner: {turn}!");
                    break;
                }

                // Swap
                turn = turn == State.X ? State.O : State.X;

                Task.Delay(2000).Wait();
            }
        }

        public bool Check()
        {
            // Possible wins
            var wins = new State?[] {
                // Horizontal
                CheckPattern(0, 0, 1, 0, 2, 0),
                CheckPattern(0, 1, 1, 1, 2, 1),
                CheckPattern(0, 2, 1, 2, 2, 2),

                // Vertical
                CheckPattern(0, 0, 0, 1, 0, 2),
                CheckPattern(1, 0, 1, 1, 1, 2),
                CheckPattern(2, 0, 2, 1, 2, 2),

                // Diagonal
                CheckPattern(0, 0, 1, 1, 2, 2),
                CheckPattern(0, 2, 1, 1, 2, 0)
            };

            foreach (var win in wins) if (win != null) return true;

            return false;
        }

        public State? CheckPattern(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            var a = board[x1, y1];
            var b = board[x2, y2];
            var c = board[x3, y3];

            if (a == State.EMPTY) return null;
            if (a == b && b == c) return a;

            return null;
        }

        public void Ask()
        {
            int count = CountEmpties();
            bool first = true;
            while (true)
            {
                Console.Clear();

                if (!first) Console.WriteLine($"Invalid square. Please enter a number from 1 to {count}.\n");

                Console.WriteLine($"{StateString.From(turn)}'s turn:");
                PrintSquares();
                Console.WriteLine("Enter a square: ");

                first = false;

                try
                {
                    var str = Console.ReadLine();
                    if (str == null) continue;

                    int val = Int32.Parse(str);
                    if (val <= 0 || val > count) continue;

                    int index = 0;
                    int num = 1;
                    foreach (var state in board)
                    {
                        if (state == State.EMPTY) num++;
                        if (num > val) break;
                        index++;
                    }
                    board[index / 3, index % 3] = turn;
                    break;
                }
                catch { }
            }
        }

        public int CountEmpties()
        {
            int count = 0;
            foreach (var val in board) if (val == State.EMPTY) count++;

            return count;
        }

        public void Print()
        {
            Console.WriteLine("Board:");
            Console.WriteLine(ToString());
        }

        public void PrintSquares()
        {
            var str = "";
            int num = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var state = board[i, j];
                    str += state == State.EMPTY ? num++ : StateString.From(state);
                }
                str += "\n";
            }
            Console.WriteLine(str);
        }

        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    str += StateString.From(board[i, j]);
                }
                str += "\n";
            }
            return str;
        }
    }
}