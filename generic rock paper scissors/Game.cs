using static Game.Menu.MenuCommand;

partial class Game
{

    private readonly Ruleset ruleset;

    private readonly HelpTableDrawer helpTableDrawer;

    private readonly PC pc;

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

    public void Move(Move move) => Console.WriteLine($"Your move: {move}");


    public void Exit() => Environment.Exit(0);

    public void Help() => helpTableDrawer.Draw();


}
