using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Template
{
    public static class EmailBodyBuilder
    {
        public static string GenerateBody(string templateName, Dictionary<string, string> templateModel)
        {
            var templatePath = $"D:/Eyad/Survey App/SurveyApplication/Survey.Infrastructure/Template/{templateName}.html";
            var streamReader = new StreamReader(templatePath);
            var body = streamReader.ReadToEnd();

            foreach (var item in templateModel)
            {
                body = body.Replace(item.Key, item.Value);
            }
            return body;
        }
    }
}
