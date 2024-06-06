using System.Text;

namespace OpenLabel.Templating
{
    internal class TemplateHandler
    {
        public string RenderTemplate(string template, Dictionary<string, string> placeholders)
        {
            template = ParseConditionals(template, placeholders);
            return ReplacePlaceholders(template, placeholders);
        }

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

        private string ParseConditionals(string template, Dictionary<string, string> placeholders)
        {
            int startIndex = template.IndexOf("{{IF ", StringComparison.Ordinal);

            while (startIndex != -1)
            {
                int endIndex = template.IndexOf("{{ENDIF}}", startIndex, StringComparison.Ordinal);
                if (endIndex == -1)
                {
                    throw new ArgumentException("Invalid template: missing {{ENDIF}}");
                }

                int conditionEnd = template.IndexOf("}}", startIndex, StringComparison.Ordinal);
                if (conditionEnd == -1 || conditionEnd > endIndex)
                {
                    throw new ArgumentException("Invalid template: malformed {{IF}} condition");
                }

                string condition = template.Substring(startIndex + 5, conditionEnd - startIndex - 5).Trim();
                string conditionalContent = template.Substring(conditionEnd + 2, endIndex - conditionEnd - 2);

                string contentToReplace = placeholders.ContainsKey(condition) ? conditionalContent : "";

                template = template.Remove(startIndex, endIndex - startIndex + 8);
                template = template.Insert(startIndex, contentToReplace);

                startIndex = template.IndexOf("{{IF ", startIndex, StringComparison.Ordinal);
            }

            return template;
        }
    }
}
