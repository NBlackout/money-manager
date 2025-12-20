namespace App.Write.Model;

public interface ISnapshot<out TId>
{
    TId Id { get; }
}