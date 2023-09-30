Lets say you want to build a templating engine based on delegates.

For each token you want to parse in your templates, you register a delegate that is responsible for returning a string that will replace that token. The template parser will take a string template parameter and an array of objects to be used during template token replacement.

Goal of this task is to implement the Template Parser.

Given the following code:

```
public class User {
    public string FirstName {get;set;}
    public string LastName {get;set;}
}

public class Company{
    public string Name {get; set;}
    public string City {get; set;}
}

var templateParser = new TemplateParser()
    .Register<User>("{{UserFirstName}}",(user) => user.FirstName)
    .Register<User>("{{UserLastName}}",(user) => user.LastName)
    .Register<Company>("{{CompanyName}}",(company) =>
    company.Name)
    .Register<Company>("{{CompanyCity}}",(city) => company.City)
var objects = new object[] {
    new User() { FirstName = "John", LastName = "Doe"},
    new Company() { Name = "ABC Company", City = "Dallas"}
}

var template = @"
    Hi {{UserFirstName}} {{UserLastName}},

    Thank you for signing up with our company {{CompanyName}} located in {{CompanyCity}}. We will be processing your request shortly.

    Thank You,
    {{CompanyName}}
";
templateParser.Parse(template, objects);
```

Implement the TemplateParser so that it will yield the following output when ran:

```
Hi John Doe,

Thank you for signing up with our company ABC Company located in Dallas. We will be processing your request shortly.

Thank You,
ABC Company
```
