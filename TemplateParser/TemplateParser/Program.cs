// See https://aka.ms/new-console-template for more information
using System.Text;

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

    /// <summary>
    /// Constructor. Creates a new instance of TemplateParser.
    /// </summary>
    public TemplateParser()
    {
        tokenMappings = new Dictionary<string, Func<object, string>>();
    }

    /// <summary>
    /// Registers a delegate function to replace a token in a template.
    /// </summary>
    /// <typeparam name="T">The type of object associated with the token.</typeparam>
    /// <param name="token">The token to be replaced in the template.</param>
    /// <param name="func">The delegate function that returns the value to be substituted.</param>
    /// <returns>The current instance of TemplateParser.</returns>
    public TemplateParser Register<T>(string token, Func<T, string> func)
    {
        tokenMappings[token] = obj =>
        {
            if (obj is T typedObj)
            {
                return func(typedObj);
            }
            return token;
        };
        return this;
    }

    /// <summary>
    /// Parses a template string and replaces tokens with corresponding values.
    /// </summary>
    /// <param name="template">The template string with tokens to be replaced.</param>
    /// <param name="objects">An array of objects used to replace the tokens.</param>
    /// <returns>The template string with tokens replaced.</returns>
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