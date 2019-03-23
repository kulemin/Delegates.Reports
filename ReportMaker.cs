using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    public class HtmlReportMaker
    {
        public string MakeReport(IEnumerable<Measurement> measurements, 
            Func<IEnumerable<double>, object> makeStatistics, 
            string caption)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append($"<h1>{caption}</h1>");
            result.Append("<ul>");
            result.Append($"<li><b>{"Temperature"}</b>: " +
                $"{makeStatistics(data.Select(z => z.Temperature)).ToString()}");
            result.Append($"<li><b>{"Humidity"}</b>: " +
                $"{makeStatistics(data.Select(z => z.Humidity)).ToString()}");
            result.Append("</ul>");
            return result.ToString();
        }

        public Func<IEnumerable<double>, object> MeanAndStdStatistic = (value) =>
        {
            var data = value.ToList();
            var mean = data.Average();
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));
            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        };

        public Func<IEnumerable<double>, object> MedianStatistics = (value) =>
        {
            var list = value.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            return list[list.Count / 2];
        };
    }

    public class MarkdownReportMaker
    {
        public string MakeReport(IEnumerable<Measurement> measurements, 
            Func<IEnumerable<double>, object> makeStatistics, 
            string caption)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append($"## {caption}\n\n");
            result.Append("");
            result.Append($" * **{"Temperature"}**: " +
                $"{makeStatistics(data.Select(z => z.Temperature)).ToString()}\n\n");

            result.Append($" * **{"Humidity"}**: " +
                $"{makeStatistics(data.Select(z => z.Humidity)).ToString()}\n\n");
            result.Append("");
            return result.ToString();
        }

        public Func<IEnumerable<double>, object> MedianStatistics = (value) =>
        {
            var list = value.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
            return list[list.Count / 2];
        };

        public Func<IEnumerable<double>, object> MeanAndStdStatistic = (value) =>
        {
            var data = value.ToList();
            var mean = data.Average();
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));
            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        };
    }

    public static class ReportMakerHelper
    {
        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> measurements)
        {
            var meanAndStdHtmlReport = new HtmlReportMaker();
            return meanAndStdHtmlReport.MakeReport
                (measurements, meanAndStdHtmlReport.MeanAndStdStatistic, "Mean and Std");
        }

        public static string MedianMarkdownReport(IEnumerable<Measurement> measurements)
        {
            var medianMarkdownReportMaker = new MarkdownReportMaker();
            return medianMarkdownReportMaker.MakeReport
                (measurements, medianMarkdownReportMaker.MedianStatistics, "Median");
        }

        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
        {
            var meanAndStdMarkdownReport = new MarkdownReportMaker();
            return meanAndStdMarkdownReport.MakeReport
                (measurements, meanAndStdMarkdownReport.MeanAndStdStatistic, "Mean and Std");
        }

        public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
        {
            var medianHtmlReport = new HtmlReportMaker();
            return medianHtmlReport.MakeReport
                (measurements, medianHtmlReport.MedianStatistics, "Median");
        }
    }
}
