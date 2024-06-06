using System.Text;

namespace OpenLabel.Scaling
{
    internal class LabelScaler
    {
        private static readonly Dictionary<string, int?> zplCommandsScalingParametersMap = new()
        {
            {"FO", 2},
            {"PW", null},
            {"FT", 2},
            {"A0", null},
            {"A1", null},
            {"A2", null},
            {"A3", null},
            {"A4", null},
            {"A5", null},
            {"A6", null},
            {"A7", null},
            {"A8", null},
            {"A9", null},
            {"A@", null},
            {"LL", null},
            {"LH", null},
            {"GB", null},
            {"FB", null},
            {"BY", null},
            {"BQ", 3},
            {"B3", null},
            {"BC", null},
            {"B7", 2}
        };

        public static string ScaleZPL(string zpl, int sourceResolution, int targetResolution)
        {
            double scaleFactor = CalculateScaleFactor(sourceResolution, targetResolution);
            List<string> zplTokens = ZPLTokenizer(zpl);
            StringBuilder scaledZPL = new();

            foreach (string zplToken in zplTokens)
            {
                string parsedToken = ZPLParser(zplToken, scaleFactor);
                if (!string.IsNullOrEmpty(parsedToken))
                {
                    scaledZPL.Append("^").Append(parsedToken);
                }
            }

            return scaledZPL.ToString();
        }

        private static List<string> ZPLTokenizer(string zpl)
        {
            return new List<string>(zpl.Split('^'));
        }

        private static string ZPLParser(string zplToken, double scaleFactor)
        {
            if (string.IsNullOrEmpty(zplToken))
            {
                return string.Empty;
            }

            string command = zplToken.Substring(0, 2);
            string[] parameters = zplToken.Substring(2).Split(',');

            if (zplCommandsScalingParametersMap.ContainsKey(command))
            {
                int? numberOfParametersToScale = zplCommandsScalingParametersMap[command];
                if (numberOfParametersToScale == null)
                {
                    numberOfParametersToScale = parameters.Length;
                }

                for (int i = 0; i < numberOfParametersToScale; i++)
                {
                    if (double.TryParse(parameters[i], out double value))
                    {
                        parameters[i] = Math.Round(value * scaleFactor, MidpointRounding.AwayFromZero).ToString();
                    }
                }
            }

            return command + string.Join(",", parameters);
        }

        private static double CalculateScaleFactor(int sourceResolution, int targetResolution)
        {
            return Math.Round((double)targetResolution / sourceResolution, 2);
        }
    }
}
