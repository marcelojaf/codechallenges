// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.RegularExpressions;

var templateParser = new TemplateParser()
    .Register<User>("{{UserFirstName}}", (user) => user.FirstName)
    .Register<User>("{{UserLastName}}", (user) => user.LastName)
    .Register<Company>("{{CompanyName}}", (company) => company.Name)
    .Register<Company>("{{CompanyCity}}", (company) => company.City);

var objects = new object[] {
                new User() { FirstName = "John", LastName = "Doe" },
                new Company() { Name = "ABC Company", City = "Dallas" }
            };


string line;
var inputBuf = new StringBuilder();
while ((line = Console.ReadLine()) != null)
{
    inputBuf.AppendLine(line);
}

// Take Standard Input -> TemplateParser
var parsedTemplateString = templateParser.Parse(inputBuf.ToString(), objects);

Console.Write(parsedTemplateString);

// END WRITE TO TEST EVALUATOR

public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Company
{
    public string Name { get; set; }
    public string City { get; set; }
}

public class TemplateParser
{
    private Dictionary<string, Func<object, string>> tokenMappings;

    public TemplateParser()
    {
        tokenMappings = new Dictionary<string, Func<object, string>>();
    }

    public TemplateParser Register<T>(string token, Func<T, string> func)
    {
        tokenMappings[token] = obj =>
        {
            if (obj is T typedObj)
            {
                return func(typedObj);
            }
            return "";
        };
        return this;
    }

    public string Parse(string template, object[] objects)
    {
        StringBuilder result = new StringBuilder(template);

        foreach (var obj in objects)
        {
            foreach (var tokenMapping in tokenMappings)
            {
                string token = tokenMapping.Key;
                Func<object, string> func = tokenMapping.Value;
                string replacement = func(obj);

                result = result.Replace(token, replacement);
            }
        }

        return result.ToString();
    }
}