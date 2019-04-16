using NLog;
using NLog.Conditions;
using NLog.Layouts;
using NLog.Targets;

namespace Game
{
    public static class NLogConfigurator
    {
        public static void Configure(LogLevel minLevel)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var coloredConsole = new NLog.Targets.ColoredConsoleTarget("console")
            {
                Layout = Layout.FromString("${message}")
            };

            var warningHighlightRule = new ConsoleRowHighlightingRule();
            warningHighlightRule.Condition = ConditionParser.ParseExpression("level == LogLevel.Warn");
            warningHighlightRule.ForegroundColor = ConsoleOutputColor.White;
            coloredConsole.RowHighlightingRules.Add(warningHighlightRule);

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

            var digitHighlightRule = new ConsoleWordHighlightingRule();
            digitHighlightRule.Regex = "\\d";
            digitHighlightRule.ForegroundColor = ConsoleOutputColor.Magenta;
            coloredConsole.WordHighlightingRules.Add(digitHighlightRule);


            config.AddRule(minLevel, LogLevel.Fatal, coloredConsole);

            LogManager.Configuration = config;
        }
    }
}
