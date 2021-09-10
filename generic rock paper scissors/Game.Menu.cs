partial class Game
{
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


}
