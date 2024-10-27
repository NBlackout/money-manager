using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        List<double?> johnValues = [5, 3, 4, 7, 2];
        List<double?> janeValues = [2, -2, -3, 2, 1];
        List<double?> joeValues = [3, 4, 4, -2, 5];

        List<AreaSeriesData> johnData = [];
        List<AreaSeriesData> janeData = [];
        List<AreaSeriesData> joeData = [];

        johnValues.ForEach(p => johnData.Add(new AreaSeriesData { Y = p }));
        janeValues.ForEach(p => janeData.Add(new AreaSeriesData { Y = p }));
        joeValues.ForEach(p => joeData.Add(new AreaSeriesData { Y = p }));

        HighchartsRenderer renderer = new(
            new Highcharts
            {
                Title = new Title { Text = "Area chart with negative values" },
                XAxis = [new XAxis { Categories = ["Apples", "Oranges", "Pears", "Grapes", "Bananas"] }],
                Credits = new Credits { Enabled = false },
                Series =
                [
                    new AreaSeries { Name = "John", Data = johnData },
                    new AreaSeries { Name = "Jane", Data = janeData },
                    new AreaSeries { Name = "Joe", Data = joeData }
                ]
            }
        );

        await this.JsRuntime.InvokeVoidAsync(
            "window.renderChart",
            "dashboard-chart",
            renderer.GetJsonOptionsForBlazor()
        );
    }
}