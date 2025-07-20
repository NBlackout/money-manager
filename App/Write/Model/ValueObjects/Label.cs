namespace App.Write.Model.ValueObjects;

public record Label
{
    public string Value { get; init; }

    public Label(string value)
    {
        this.Value = value.Trim();
    }

    public static Label? From(string? value)
    {
        if (value == null)
            return null;

        return new Label(value);
    }

    public virtual bool Equals(Label? other) =>
        string.Equals(this.Value, other?.Value, StringComparison.CurrentCultureIgnoreCase);

    public override int GetHashCode() =>
        this.Value.GetHashCode();
}