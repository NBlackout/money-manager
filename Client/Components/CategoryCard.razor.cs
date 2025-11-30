using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Components;

public partial class CategoryCard : ComponentBase
{
    private bool isCreating;
    private InputText? childLabelElement;

    [Inject] public CreateCategory CreateCategory { get; set; } = null!;
    [Inject] public DeleteCategory DeleteCategory { get; set; } = null!;

    [Parameter] public CategorySummaryPresentation Category { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnCategoryDeleted { get; set; }

    [SupplyParameterFromForm] public ChildForm? Child { get; set; }

    protected override void OnInitialized() =>
        this.Child ??= new ChildForm();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isCreating && this.childLabelElement?.Element != null)
            await this.childLabelElement.Element.Value.FocusAsync();
    }

    private void EnterCreateMode() =>
        this.isCreating = true;

    private void ExitCreateMode() =>
        this.isCreating = false;

    private async Task CreateChild()
    {
        Guid id = Guid.NewGuid();
        string label = this.Child!.Label!;
        await this.CreateCategory.Execute(new CategoryId(id), new Label(label), new CategoryId(this.Category.Id));

        this.Category = this.Category with { Children = this.Category.Children.Append(new ChildCategorySummaryPresentation(id, label)).ToArray() };
        this.Child = new ChildForm();
        this.ExitCreateMode();
    }

    private async Task Delete()
    {
        await this.DeleteCategory.Execute(new CategoryId(this.Category.Id));
        await this.OnCategoryDeleted.InvokeAsync(this.Category.Id);
    }

    private async Task DeleteChild(Guid categoryId)
    {
        await this.DeleteCategory.Execute(new CategoryId(categoryId));
        this.Category = this.Category with { Children = this.Category.Children.Where(c => c.Id != categoryId).ToArray() };
    }

    public class ChildForm
    {
        public string? Label { get; set; }
    }
}