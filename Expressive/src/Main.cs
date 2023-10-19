using Expressive;
using StringContent = Expressive.StringContent;


while (true) {
    Console.Write("> "); Console.Out.Flush();
    var input = Console.ReadLine();

    var content = new StringContent(input ?? "shit eater");
    var lexer = new Lexer(content);
    var lexerResult = lexer.Parse();

    if (lexerResult.IsError)
    {
        Console.WriteLine(lexerResult.Error);
        continue;
    }

    var parser = new Parser(lexerResult.Tokens);
    var parserResult = parser.Parse();

    if (parserResult.IsError)
    {
        Console.WriteLine(parserResult.Error);
        continue;
    }

    Console.WriteLine("All good!");
}