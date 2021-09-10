using System.Security.Cryptography;
using System.Text;
using static Game.Menu.MenuCommand;

class Game
{

    private readonly Ruleset ruleset;

    private readonly HelpTableDrawer helpTableDrawer;

    private readonly PC pc;


    public class PC
    {

        private readonly Move move;


        private readonly byte[] key;

        private readonly byte[] hash;


        public PC(Ruleset ruleset)
        {
            key = RandomNumberGenerator.GetBytes(16);

            var hmac = new HMACSHA256(key);

            var moveIdx = RandomNumberGenerator.GetInt32(0, ruleset.Moves.Count);

            move = ruleset.Moves.ElementAt(moveIdx);

            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(move.ToString().ToCharArray()));

        }

        public void PreMove() => Console.WriteLine($"HMAC: {Convert.ToBase64String(hash)}");



        public Move Move()
        {
            Console.WriteLine($"PC move: {move} ");

            Console.WriteLine($"HMAC key: {Convert.ToBase64String(key)}");

            return move;

        }
    }

    public Game(params string[] moveNames)
    {
        ruleset = new(moveNames);
        helpTableDrawer = new(ruleset);
        pc = new(ruleset);
    }
        
     

    public void Start()
    {
        pc.PreMove();

        var commands = ruleset.Moves
           .OrderBy(move => move.Number)
           .Select(move => FromPattern(move.Number.ToString(), new(move.ToString(), () => OnMoveCommand(move))))
           .Append(FromPattern("0", new("exit", Exit)))
           .Append(FromPattern("?", new("help", Help)))

           .ToArray();

        var menu = new Menu(commands);

        menu.Title = "Available commands";

        menu.Commands.Add(Default(new("default", menu.Show)));

        menu.Show();

        menu.ProcessInput();

    }

    void OnMoveCommand(Move playerMove)
    {
        Move(playerMove);

        var pcMove = pc.Move();

        var playerRes = ruleset.GetPlayerResult(playerMove, pcMove);

        Console.WriteLine(playerRes switch
        {
            GameResult.Win => "You win!",
            GameResult.Lose => "You lose",
            GameResult.Draw => "Draw",
            _ => throw new NotImplementedException($"{nameof(playerRes)} : {playerRes}")
        });

        Exit();
    }

    public class Menu
    {
        public readonly IList<MenuCommand> Commands;
        public string Title { get; set; } = string.Empty;
        public Menu(IEnumerable<MenuCommand> commands) => Commands = new List<MenuCommand>(commands);


        public void ProcessInput()
        {
            while (true)
            {
                var input = Console.ReadLine();

                var command = Commands.FirstOrDefault(command => command.Pattern == input)
                    ?? Commands.FirstOrDefault(command => command.IsDefault)
                    ?? throw new InvalidOperationException($"no command found for ${nameof(input)}: {input}");

                command.Action.Execute();
            }
        }

        public class MenuCommand
        {

            public readonly string? Pattern;

            public readonly MenuAction Action;

            public readonly bool IsDefault;

            public static MenuCommand Default(MenuAction action) => new MenuCommand(null, action, true);
            public static MenuCommand FromPattern(string pattern, MenuAction action) => new MenuCommand(pattern, action, false);
            private MenuCommand(string pattern, MenuAction action, bool isDefault) =>
                (Pattern, Action, IsDefault) = (pattern, action, isDefault);

            public override string ToString() => $"{Pattern} - {Action}";


            public class MenuAction
            {
                public readonly Action Action;

                public readonly string Name;
                public void Execute() => Action();
                public MenuAction(string name, Action action) => (Name, Action) = (name, action);
                public override string ToString() => Name;

            }

        }

        public override string ToString() => string.Join(Environment.NewLine, Commands
            .Where(command => !command.IsDefault)
            .Select(command => command.ToString())
            .Prepend(Title));

        public void Show() => Console.WriteLine(this);
    }



    public void Move(Move move) => Console.WriteLine($"Your move: {move}");


    public void Exit() => Environment.Exit(0);

    public void Help() => helpTableDrawer.Draw();


}
