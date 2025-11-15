using System.Collections;
using App.Read.Ports;
using App.Read.UseCases;
using App.Shared;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.JSInterop;

namespace Client.Pages;

public partial class Dashboard
{
    [Inject] public MonthlyPerformance MonthlyPerformance { get; set; } = null!;
    [Inject] public PerformanceForecast PerformanceForecast { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    private PeriodPerformancePresentation[]? monthlyPerformance;
    private PerformanceForecastPresentation? performanceForecast;

    protected override async Task OnInitializedAsync()
    {
        this.monthlyPerformance = await this.MonthlyPerformance.Execute();
        this.performanceForecast = await this.PerformanceForecast.Execute();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.monthlyPerformance == null || this.performanceForecast == null || this.monthlyPerformance.Length == 0)
            return;

        HighchartsRenderer renderer = new(
            new Highcharts
            {
                Title = new Title { Text = "Monthly performance" },
                XAxis =
                [
                    new XAxis
                    {
                        Categories = CategoriesBy(this.monthlyPerformance),
                        TickWidth = 1,
                        PlotLines = [new XAxisPlotLines { Value = 10.5, DashStyle = XAxisPlotLinesDashStyle.Dash, Width = 2, Color = "#4840d6" }],
                        PlotBands =
                        [
                            new XAxisPlotBands
                            {
                                From = 10.5,
                                To = 12.5,
                                Color = "rgba(255, 75, 66, 0.07)",
                                Label = new XAxisPlotBandsLabel { Text = "Forecast" }
                            }
                        ]
                    }
                ],
                YAxis =
                [
                    new YAxis { Title = new YAxisTitle { Text = "Performance (€)" }, Id = "Performance" },
                    new YAxis { Title = new YAxisTitle { Text = "Balance (€)" }, Id = "Balance", Opposite = true }
                ],
                Credits = new Credits { Enabled = false },
                Series = SeriesBy(this.monthlyPerformance, this.performanceForecast),
                Tooltip = new Tooltip { Shared = true, Distance = 75, ValueSuffix = " €" },
                PlotOptions = new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0,
                        GroupPadding = 0,
                        Opacity = 0.5,
                        States = new PlotOptionsColumnStates
                        {
                            Hover = new PlotOptionsColumnStatesHover { CustomFields = new Hashtable { { "opacity", "1" } } }
                        }
                    },
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

    private static List<string> CategoriesBy(PeriodPerformancePresentation[] periods)
    {
        DateRange[] ranges = [..periods.Select(p => p.DateRange)];
        ranges = [..ranges, ranges.Last() with { From = ranges.Last().From.AddMonths(1) }];

        return ranges.Select(r => r.From.ToString("MMMM")).ToList();
    }

    private static List<Series> SeriesBy(PeriodPerformancePresentation[] periods, PerformanceForecastPresentation forecast)
    {
        return
        [
            new ColumnSeries
            {
                Name = "Net", YAxis = "Performance", Data = NetOf(periods, forecast), Color = "green", NegativeColor = "red", Opacity = 0.5
            },
            new AreaSeries { Name = "Revenue", YAxis = "Performance", Data = RevenueOf(periods, forecast), Color = "green" },
            new AreaSeries { Name = "Expenses", YAxis = "Performance", Data = ExpensesOf(periods, forecast), Color = "red" },
            new LineSeries { Name = "Balance", YAxis = "Balance", Data = BalancesOf(periods, forecast), ColorIndex = 0, PointPlacementNumber = -0.5 }
        ];
    }

    private static List<ColumnSeriesData> NetOf(PeriodPerformancePresentation[] periods, PerformanceForecastPresentation forecast)
    {
        decimal[] net = [..periods.Select(p => p.Performance.Net), forecast.Net];

        return net.Select(n => new ColumnSeriesData { Y = decimal.ToDouble(n) }).ToList();
    }

    private static List<AreaSeriesData> RevenueOf(PeriodPerformancePresentation[] periods, PerformanceForecastPresentation forecast)
    {
        decimal[] revenue = [..periods.Select(p => p.Performance.Revenue), forecast.Revenue];

        return revenue.Select(r => new AreaSeriesData { Y = decimal.ToDouble(r) }).ToList();
    }

    private static List<AreaSeriesData> ExpensesOf(PeriodPerformancePresentation[] periods, PerformanceForecastPresentation forecast)
    {
        decimal[] expenses = [..periods.Select(p => p.Performance.Expenses), forecast.Expenses];

        return expenses.Select(e => new AreaSeriesData { Y = decimal.ToDouble(e) }).ToList();
    }

    private static List<LineSeriesData> BalancesOf(PeriodPerformancePresentation[] periods, PerformanceForecastPresentation forecast)
    {
        decimal[] balances = [..periods.Select(p => p.Balance), forecast.CurrentBalance + forecast.Net];

        return balances.Select(b => new LineSeriesData { Y = decimal.ToDouble(b) }).ToList();
    }
}