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
    private Dictionary<string, Delegate> tokenMap;

    public TemplateParser()
    {
        tokenMap = new Dictionary<string, Delegate>();
    }

    public TemplateParser Register<T>(string token, Func<T, string> replacementFunction)
    {
        if (!tokenMap.ContainsKey(token))
        {
            // Register the token and its replacement function.
            tokenMap[token] = replacementFunction;
        }
        else
        {
            // Handle the case where a token is already registered.
            // You can choose to handle this differently if needed.
            throw new InvalidOperationException($"Token '{token}' is already registered.");
        }
        return this;
    }

    public string Parse(string template, object[] objects)
    {
        // Use regular expressions to find all tokens in the template.
        var regex = new Regex(@"{{(.*?)}}");
        var matches = regex.Matches(template);

        // Iterate through each token match and perform substitution.
        foreach (Match match in matches)
        {
            var token = match.Value;
            if (tokenMap.ContainsKey(token))
            {
                // Extract the type name from within the double curly braces of the token.
                var typeName = match.Groups[1].Value;

                // Find the corresponding object in the array of objects.
                var obj = Array.Find(objects, o => o.GetType().Name == typeName);

                if (obj != null)
                {
                    // Get the replacement function from the dictionary and execute it.
                    var replacementFunction = (Func<object, string>)tokenMap[token];
                    var replacement = replacementFunction(obj);

                    // Replace the token with the result of the replacement function.
                    template = template.Replace(token, replacement);
                }
                else
                {
                    // Handle the case where a matching object is not found.
                    // You can choose to handle this differently if needed.
                    throw new InvalidOperationException($"Matching object for '{token}' not found.");
                }
            }
        }

        return template;
    }
}