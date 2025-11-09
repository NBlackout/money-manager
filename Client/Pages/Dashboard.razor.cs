using System.Collections;
using App.Read.Ports;
using App.Read.UseCases;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    [Inject] public MonthlyPerformance MonthlyPerformance { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    private PeriodPerformancePresentation[]? monthlyPerformance;

    protected override async Task OnInitializedAsync() =>
        this.monthlyPerformance = await this.MonthlyPerformance.Execute();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.monthlyPerformance?.Length != 0)
        {
            HighchartsRenderer renderer = new(
                new Highcharts
                {
                    Title = new Title { Text = "Monthly performance" },
                    XAxis = [new XAxis { Categories = CategoriesBy(this.monthlyPerformance!), TickWidth = 1 }],
                    YAxis =
                    [
                        new YAxis { Title = new YAxisTitle { Text = "Performance (€)" }, Id = "Performance" },
                        new YAxis { Title = new YAxisTitle { Text = "Balance (€)" }, Id = "Balance", Opposite = true }
                    ],
                    Credits = new Credits { Enabled = false },
                    Series = SeriesBy(this.monthlyPerformance!),
                    Tooltip = new Tooltip { Shared = true, Distance = 75 },
                    PlotOptions = new PlotOptions { Column = new PlotOptionsColumn { PointPadding = 0, GroupPadding = 0 } }
                }
            );

            await this.JsRuntime.InvokeVoidAsync("window.renderChart", "dashboard-chart", renderer.GetJsonOptionsForBlazor());
        }
    }

    private static List<string> CategoriesBy(PeriodPerformancePresentation[] presentation) =>
        presentation.Select(b => b.DateRange.From.ToString("MMMM")).ToList();

    private static List<Series> SeriesBy(PeriodPerformancePresentation[] presentation)
    {
        return
        [
            new ColumnSeries
            {
                Name = "Net",
                YAxis = "Performance",
                Data = presentation.Select(sb => new ColumnSeriesData { Y = decimal.ToDouble(sb.Performance.Net) }).ToList(),
                Zones = [new ColumnSeriesZone { Color = "red", Value = 0 }, new ColumnSeriesZone { Color = "green" }],
                Tooltip = new ColumnSeriesTooltip { ValueSuffix = " €" },
                Opacity = 0.5,
                States = new ColumnSeriesStates { Hover = new ColumnSeriesStatesHover { CustomFields = new Hashtable { { "opacity", "1" } } } }
            },
            new AreaSeries
            {
                Name = "Revenue",
                YAxis = "Performance",
                Data = presentation.Select(sb => new AreaSeriesData { Y = decimal.ToDouble(sb.Performance.Revenue) }).ToList(),
                Color = "green",
                Tooltip = new AreaSeriesTooltip { ValueSuffix = " €" },
                FillOpacity = 0.1,
                LineWidth = 0,
                Marker = new AreaSeriesMarker { Enabled = false }
            },
            new AreaSeries
            {
                Name = "Expenses",
                YAxis = "Performance",
                Data = presentation.Select(sb => new AreaSeriesData { Y = decimal.ToDouble(sb.Performance.Expenses) }).ToList(),
                Color = "red",
                Tooltip = new AreaSeriesTooltip { ValueSuffix = " €" },
                FillOpacity = 0.1,
                LineWidth = 0,
                Marker = new AreaSeriesMarker { Enabled = false }
            },
            new LineSeries
            {
                Name = "Balance",
                YAxis = "Balance",
                Data = presentation.Select(sb => new LineSeriesData { Y = decimal.ToDouble(sb.Balance) }).ToList(),
                ColorIndex = 0,
                Tooltip = new LineSeriesTooltip { ValueSuffix = " €" },
                PointPlacementNumber = -0.5,
                ZIndex = 1
            }
        ];
    }
}