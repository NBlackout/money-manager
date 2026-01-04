using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.UseCases.Categories;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Components;

public partial class CategoryCard : ComponentBase
{
    private bool isCreating;
    private bool isEditing;
    private ElementReference labelElement;
    private InputText? childLabelElement;

    [Inject] private CreateCategory CreateCategory { get; set; } = null!;
    [Inject] private RenameCategory RenameCategory { get; set; } = null!;
    [Inject] private DeleteCategory DeleteCategory { get; set; } = null!;

    [Parameter] public CategorySummaryPresentation Category { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnCategoryDeleted { get; set; }

    [SupplyParameterFromForm] public ChildForm? Child { get; set; }

    protected override void OnInitialized() =>
        this.Child ??= new ChildForm();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isEditing)
            await this.labelElement.FocusAsync();
        if (this.isCreating && this.childLabelElement?.Element != null)
            await this.childLabelElement.Element.Value.FocusAsync();
    }

    private void EnterEditMode() =>
        this.isEditing = true;

    private void ExitEditMode() =>
        this.isEditing = false;

    private void EnterCreateMode() =>
        this.isCreating = true;

    private void ExitCreateMode() =>
        this.isCreating = false;

    private async Task LabelChanged(ChangeEventArgs args)
    {
        string newLabel = (string)args.Value!;
        await this.RenameCategory.Execute(new CategoryId(this.Category.Id), new Label(newLabel));

        this.Category = this.Category with { Label = newLabel };
        this.ExitEditMode();
    }

    private async Task CreateChild()
    {
        Guid id = Guid.NewGuid();
        string label = this.Child!.Label!;
        await this.CreateCategory.Execute(new CategoryId(id), new Label(label), new CategoryId(this.Category.Id));

        this.SetChildrenTo(this.Category.Children.Append(new ChildCategorySummaryPresentation(id, label)).ToArray());
        this.Child = new ChildForm();
        this.ExitCreateMode();
    }

    private async Task Delete()
    {
        await this.DeleteCategory.Execute(new CategoryId(this.Category.Id));
        await this.OnCategoryDeleted.InvokeAsync(this.Category.Id);
    }

    private void OnChildDeleted(Guid categoryId) =>
        this.SetChildrenTo([..this.Category.Children.Where(c => c.Id != categoryId)]);

    private void SetChildrenTo(ChildCategorySummaryPresentation[] children) =>
        this.Category = this.Category with { Children = children.OrderBy(c => c.Label).ToArray() };

    public class ChildForm
    {
        public string? Label { get; set; }
    }
}