using Expressive;
using StringContent = Expressive.StringContent;


while (true) {
    Console.Write("> "); Console.Out.Flush();
    var input = Console.ReadLine();

    var content = new StringContent(input ?? "shit eater");
    var lexer = new Lexer(content);

    var result = lexer.Parse();

    if (result.IsError) {
        Console.WriteLine(result.Error);
    }
    else {
        Console.WriteLine(result.Tokens.Count + " tokens:");
        foreach (var item in result.Tokens)
        {
            Console.WriteLine("\t" + item);
        }
    }
}