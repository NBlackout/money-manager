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
    [Inject] public BalanceForecasts BalanceForecasts { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    private PeriodPerformancePresentation[]? periodPerformance;
    private BalanceForecastPresentation[]? balanceForecasts;

    protected override async Task OnInitializedAsync()
    {
        this.periodPerformance = await this.MonthlyPerformance.Execute();
        this.balanceForecasts = await this.BalanceForecasts.Execute();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.periodPerformance == null || this.balanceForecasts == null || this.periodPerformance.Length == 0)
            return;

        DateTime today = DateTime.Today;
        double todayIndicator = 10.5 + today.Day / (double)DateTime.DaysInMonth(today.Year, today.Month);
        HighchartsRenderer renderer = new(
            new Highcharts
            {
                Title = new Title { Text = "Monthly performance" },
                XAxis =
                [
                    new XAxis
                    {
                        Categories = CategoriesBy(this.periodPerformance, this.balanceForecasts),
                        TickWidth = 1,
                        PlotLines =
                        [
                            new XAxisPlotLines
                            {
                                Value = todayIndicator, DashStyle = XAxisPlotLinesDashStyle.Dash, Width = 2, Color = "#4840d6"
                            }
                        ],
                        PlotBands =
                        [
                            new XAxisPlotBands
                            {
                                From = todayIndicator,
                                To = 14.5,
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
                Series = SeriesBy(this.periodPerformance, this.balanceForecasts),
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

    private static List<string> CategoriesBy(PeriodPerformancePresentation[] performances, BalanceForecastPresentation[] balanceForecasts)
    {
        DateOnly[] months = performances.Select(p => p.Period.From).ToArray();
        DateOnly[] forecastedMonths = balanceForecasts.Select(f => f.Date.AddDays(1)).ToArray();
        DateOnly[] from = [..months, ..forecastedMonths];

        return from.Select(f => f.ToString("MMMM")).ToList();
    }

    private static List<Series> SeriesBy(PeriodPerformancePresentation[] periods, BalanceForecastPresentation[] forecast) =>
    [
        new ColumnSeries { Name = "Net", YAxis = "Performance", Data = NetOf(periods), Color = "green", NegativeColor = "red", Opacity = 0.5 },
        new AreaSeries { Name = "Revenue", YAxis = "Performance", Data = RevenueOf(periods), Color = "green" },
        new AreaSeries { Name = "Expenses", YAxis = "Performance", Data = ExpensesOf(periods), Color = "red" },
        new LineSeries { Name = "Starting balance", YAxis = "Balance", Data = BalancesOf(periods), ColorIndex = 0, PointPlacementNumber = -0.5 },
        new LineSeries { Name = "Forecast", YAxis = "Balance", Data = ForecastsOf(periods, forecast), ColorIndex = 0, PointPlacementNumber = 0.5 }
    ];

    private static List<ColumnSeriesData> NetOf(PeriodPerformancePresentation[] periods) =>
        periods.Select(p => new ColumnSeriesData { Y = decimal.ToDouble(p.Performance.Net) }).ToList();

    private static List<AreaSeriesData> RevenueOf(PeriodPerformancePresentation[] periods) =>
        periods.Select(p => new AreaSeriesData { Y = decimal.ToDouble(p.Performance.Revenue) }).ToList();

    private static List<AreaSeriesData> ExpensesOf(PeriodPerformancePresentation[] periods) =>
        periods.Select(p => new AreaSeriesData { Y = decimal.ToDouble(p.Performance.Expenses) }).ToList();

    private static List<LineSeriesData> BalancesOf(PeriodPerformancePresentation[] periods) =>
        periods.Select(p => new LineSeriesData { Y = decimal.ToDouble(p.Balance) }).ToList();

    private static List<LineSeriesData> ForecastsOf(PeriodPerformancePresentation[] periods, BalanceForecastPresentation[] forecasts)
    {
        decimal?[] balances = [..Enumerable.Repeat((decimal?)null, periods.Length - 1), ..forecasts.Select(f => f.Balance)];

        return balances.Select(b => new LineSeriesData { Y = b.HasValue ? decimal.ToDouble(b.Value) : null }).ToList();
    }
}