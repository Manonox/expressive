using Antlr4.Runtime;
using Expressive;
using Expressive.AST;
using StringContent = Expressive.StringContent;


while (true) {
    // Console.Write("> "); Console.Out.Flush();
    // _ = Console.ReadLine();

    // var content = new StringContent(input ?? "shit eater");
    // var content = new FileContent(input ?? "main.exr");

    var content = new FileContent("tests/basic.exr");
    var lexer = new ExpressiveLexer(CharStreams.fromString(content.Data));
    var parser = new ExpressiveParser(new CommonTokenStream(lexer));
    var ast = parser.chunk();
    
    if (ast is null)
    {
        Console.WriteLine("Parse error..?");
        continue;
    }

    var vm = new VM();
    vm.variables.Add("print", new VM.Value((args) => {
        var strs = args.Select(x => x.ToString());
        Console.WriteLine(string.Join(", ", strs));
        return new();
    }));

    var visitor = new StatementVisitor() { Vm = vm, Parser = parser };
    visitor.Visit(ast);
    

    Console.WriteLine("All good!");
    
    foreach (var name in vm.variables.Keys) {
        Console.WriteLine($"{name} = {vm.variables.GetValueOrDefault(name)}");
    }

    _ = Console.ReadLine();
}