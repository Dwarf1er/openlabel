using System.Text;

namespace OpenLabel.Scaling
{
    /// <summary>
    /// Handles scaling of ZPL commands for different printer resolutions.
    /// </summary>
    public class LabelScaler
    {
        /// <summary>
        /// Defines ZPL commands that require scaling, along with the number of parameters that should be scaled.
        /// If the value is <c>null</c>, all parameters of the command will be scaled.
        /// </summary>
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

        /// <summary>
        /// Scales a given ZPL string from a source DPI to a target DPI.
        /// </summary>
        /// <param name="zpl">The original ZPL string.</param>
        /// <param name="sourceResolution">The source resolution of the original ZPL.</param>
        /// <param name="targetResolution">The target resolution of the final ZPL.</param>
        /// <returns>
        /// A scaled ZPL string where applicable commands have been adjusted based on the resolution change.
        /// </returns>
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

        /// <summary>
        /// Splits a ZPL string into individual command tokens.
        /// </summary>
        /// <param name="zpl">The ZPL string to tokenize.</param>
        /// <returns>A list of ZPL command tokens.</returns>
        private static List<string> ZPLTokenizer(string zpl)
        {
            return new List<string>(zpl.Split('^'));
        }

        /// <summary>
        /// Parses and scales a single ZPL command based on the given resolution scale factor.
        /// </summary>
        /// <param name="zplToken">The ZPL command token.</param>
        /// <param name="scaleFactor">The scaling factor to apply to numeric parameters.</param>
        /// <returns>
        /// The scaled ZPL command string, or an empty string if the token is invalid.
        /// </returns>
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

        /// <summary>
        /// Calculates the scale factor for converting from one resolution to another.
        /// </summary>
        /// <param name="sourceResolution">The source resolution of the ZPL.</param>
        /// <param name="targetResolution">The target resolution of the ZPL.</param>
        /// <returns>The scale factor to apply when adjusting ZPL command values.</returns>
        private static double CalculateScaleFactor(int sourceResolution, int targetResolution)
        {
            return Math.Round((double)targetResolution / sourceResolution, 2);
        }
    }
}
