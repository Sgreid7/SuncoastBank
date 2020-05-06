# First Bank of Suncoast

A console app that will let you track a savings and a checking account total by performing "transactions"

This will save your information in a txt file in JSON so you can track your account totals over time, automatically.

# Includes

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
- [JSON](https://www.json.org/json-en.html)
- [PERSISTENT DATA](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/walkthrough-persisting-an-object-in-visual-studio)
- [MVC](https://dotnet.microsoft.com/apps/aspnet/mvc)

# Featured Code

## Method Used to Save Data to bankData.txt

```JSX
static void SaveData(Bank bank)
    {
      string json = JsonConvert.SerializeObject(bank);
      System.IO.File.WriteAllText(@"bankData.txt", json);
    }
```
