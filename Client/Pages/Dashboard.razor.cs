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
                    Tooltip = new Tooltip { Shared = true, Distance = 75, ValueSuffix = " €" },
                    PlotOptions = new PlotOptions
                    {
                        Column = new PlotOptionsColumn { PointPadding = 0, GroupPadding = 0 },
                        Area = new PlotOptionsArea
                        {
                            Marker = new PlotOptionsAreaMarker { Enabled = false },
                            States = new PlotOptionsAreaStates { Hover = new PlotOptionsAreaStatesHover { LineWidthPlus = 0 } },
                            LineWidth = 0,
                            FillOpacity = 0.1
                        }
                    }
                }
            );

            await this.JsRuntime.InvokeVoidAsync("window.renderChart", "dashboard-chart", renderer.GetJsonOptionsForBlazor());
        }
    }

    private static List<string> CategoriesBy(PeriodPerformancePresentation[] periods) =>
        periods.Select(p => p.DateRange.From.ToString("MMMM")).ToList();

    private static List<Series> SeriesBy(PeriodPerformancePresentation[] periods)
    {
        return
        [
            new ColumnSeries
            {
                Name = "Net",
                YAxis = "Performance",
                Data = periods.Select(p => new ColumnSeriesData { Y = decimal.ToDouble(p.Performance.Net) }).ToList(),
                Color = "green",
                NegativeColor = "red",
                Opacity = 0.5,
                States = new ColumnSeriesStates { Hover = new ColumnSeriesStatesHover { CustomFields = new Hashtable { { "opacity", "1" } } } },
                ZIndex = 1
            },
            new AreaSeries
            {
                Name = "Revenue",
                YAxis = "Performance",
                Data = periods.Select(p => new AreaSeriesData { Y = decimal.ToDouble(p.Performance.Revenue) }).ToList(),
                Color = "green",
            },
            new AreaSeries
            {
                Name = "Expenses",
                YAxis = "Performance",
                Data = periods.Select(p => new AreaSeriesData { Y = decimal.ToDouble(p.Performance.Expenses) }).ToList(),
                Color = "red",
            },
            new LineSeries
            {
                Name = "Balance",
                YAxis = "Balance",
                Data = periods.Select(p => new LineSeriesData { Y = decimal.ToDouble(p.Balance) }).ToList(),
                ColorIndex = 0,
                PointPlacementNumber = -0.5,
                ZIndex = 1
            }
        ];
    }
}