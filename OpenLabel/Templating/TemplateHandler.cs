using OpenLabel.Shared;
using System.Text;

namespace OpenLabel.Templating
{
    /// <summary>
    /// Handles ZPL template rendering by replacing placeholders and parsing conditional statements.
    /// </summary>
    public class TemplateHandler
    {
        /// <summary>
        /// Renders a template by replacing placeholders and evaluating conditions.
        /// </summary>
        /// <param name="template">The template string containing placeholders.</param>
        /// <param name="placeholders">A dictionary of key-value pairs where the key is the placeholder and the value is the replacement string.</param>
        /// <returns>
        /// A <see cref="Result{T, E}"/> containing the rendered ZPL string if successful, 
        /// or an error message if a problem occurs during rendering.
        /// </returns>
        public Result<string, string> RenderTemplate(string template, Dictionary<string, string> placeholders)
        {
            var parsedTemplateResult = ParseConditionals(template, placeholders);
            if (parsedTemplateResult.IsFailure)
            {
                return Result<string, string>.Err(parsedTemplateResult.Error);
            }

            string finalTemplate = ReplacePlaceholders(parsedTemplateResult.Value, placeholders);
            return Result<string, string>.Ok(finalTemplate);
        }

        /// <summary>
        /// Replaces placeholders in the template with their corresponding values from the dictionary.
        /// </summary>
        /// <param name="template">The template string containing placeholders to be replaced.</param>
        /// <param name="placeholders">A dictionary containing the placeholder keys and their respective replacement values.</param>
        /// <returns>The template with all placeholders replaced by their corresponding values.</returns>
        private string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            StringBuilder result = new StringBuilder(template);

            foreach (var placeholder in placeholders)
            {
                string placeholderKey = placeholder.Key;
                string placeholderValue = placeholder.Value;

                result.Replace(placeholderKey, placeholderValue);
            }

            return result.ToString();
        }

        /// <summary>
        /// Parses conditional blocks (e.g., {{IF CONDITION}}...{{ENDIF}}) in the template.
        /// If the condition is present in the placeholders dictionary, the content inside the block is included in the result.
        /// Otherwise, the conditional content is removed.
        /// </summary>
        /// <param name="template">The template string that may contain conditional blocks.</param>
        /// <param name="placeholders">A dictionary of key-value pairs for placeholder replacements. It is used to check if a condition is met.</param>
        /// <returns>
        /// A <see cref="Result{T, E}"/> containing the parsed template with conditionals evaluated, 
        /// or an error message if the template contains invalid conditionals.
        /// </returns>
        private Result<string, string> ParseConditionals(string template, Dictionary<string, string> placeholders)
        {
            int startIndex = template.IndexOf("{{IF ", StringComparison.Ordinal);

            while (startIndex != -1)
            {
                int endIndex = template.IndexOf("{{ENDIF}}", startIndex, StringComparison.Ordinal);
                if (endIndex == -1)
                {
                    return Result<string, string>.Err("Invalid template: missing {{ENDIF}}");
                }

                int conditionEnd = template.IndexOf("}}", startIndex, StringComparison.Ordinal);
                if (conditionEnd == -1 || conditionEnd > endIndex)
                {
                    return Result<string, string>.Err("Invalid template: malformed {{IF}} condition");
                }

                string condition = template.Substring(startIndex + 5, conditionEnd - startIndex - 5).Trim();
                string conditionalContent = template.Substring(conditionEnd + 2, endIndex - conditionEnd - 2);

                string contentToReplace = placeholders.ContainsKey(condition) ? conditionalContent : "";

                template = template.Remove(startIndex, endIndex - startIndex + 8);
                template = template.Insert(startIndex, contentToReplace);

                startIndex = template.IndexOf("{{IF ", startIndex, StringComparison.Ordinal);
            }

            return Result<string, string>.Ok(template);
        }
    }
}
