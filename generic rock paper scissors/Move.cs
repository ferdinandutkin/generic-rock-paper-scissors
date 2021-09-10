// See https://aka.ms/new-console-template for more information








class Move
{

    public readonly string Name;

    public readonly int Number;

    public readonly HashSet<Move> Beats;

    public readonly HashSet<Move> BeatenBy;

    public Move(string name, int number, HashSet<Move> beats, HashSet<Move> beatenBy)
        => (Name, Number, Beats, BeatenBy) = (name, number, beats, beatenBy);


    public Move(string name, int number) : this(name, number, new(), new())
    { }


    public override string ToString() => Name;



}
