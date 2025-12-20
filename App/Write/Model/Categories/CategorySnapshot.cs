namespace App.Write.Model.Categories;

public record CategorySnapshot(CategoryId Id, string Label, CategoryId? ParentId) : ISnapshot<CategoryId>;