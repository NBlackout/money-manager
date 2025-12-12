using App.Read.Ports;
using App.Write.Model.Categories;
using App.Write.Model.ValueObjects;
using App.Write.UseCases;

namespace Client.Components;

public partial class CategoryCardChild : ComponentBase
{
    private bool isEditing;
    private ElementReference labelElement;

    [Inject] private RenameCategory RenameCategory { get; set; } = null!;
    [Inject] public DeleteCategory DeleteCategory { get; set; } = null!;

    [Parameter] public ChildCategorySummaryPresentation Category { get; set; } = null!;
    [Parameter] public EventCallback<Guid> OnChildDeleted { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.isEditing)
            await this.labelElement.FocusAsync();
    }

    private void EnterEditMode() =>
        this.isEditing = true;

    private void ExitEditMode() =>
        this.isEditing = false;

    private async Task LabelChanged(ChangeEventArgs args)
    {
        string newLabel = (string)args.Value!;
        await this.RenameCategory.Execute(new CategoryId(this.Category.Id), new Label(newLabel));

        this.Category = this.Category with { Label = newLabel };
        this.ExitEditMode();
    }

    private async Task Delete()
    {
        await this.DeleteCategory.Execute(new CategoryId(this.Category.Id));
        await this.OnChildDeleted.InvokeAsync(this.Category.Id);
    }
}