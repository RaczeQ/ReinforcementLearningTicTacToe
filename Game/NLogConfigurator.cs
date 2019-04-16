using NLog;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class NLogConfigurator
    {
        public static void Configure(LogLevel minLevel)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var coloredConsole = new NLog.Targets.ColoredConsoleTarget("console");
            coloredConsole.Layout = Layout.FromString("${message}");

            var xHighlightRule = new ConsoleWordHighlightingRule();
            xHighlightRule.Text = "X";
            xHighlightRule.ForegroundColor = ConsoleOutputColor.Red;
            coloredConsole.WordHighlightingRules.Add(xHighlightRule);

            var oHighlightRule = new ConsoleWordHighlightingRule();
            oHighlightRule.Text = "O";
            oHighlightRule.ForegroundColor = ConsoleOutputColor.Yellow;
            coloredConsole.WordHighlightingRules.Add(oHighlightRule);

            var boardHighlightRule = new ConsoleWordHighlightingRule();
            boardHighlightRule.Regex = "(\\||-|\\+)";
            boardHighlightRule.ForegroundColor = ConsoleOutputColor.DarkGray;
            coloredConsole.WordHighlightingRules.Add(boardHighlightRule);


            config.AddRule(minLevel, LogLevel.Fatal, coloredConsole);

            NLog.LogManager.Configuration = config;
        }
    }
}
