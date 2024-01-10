namespace Expressive.IL
{
    interface IInstruction
    {
        public bool Execute(VM vm);
    }

    class SetInstruction : IInstruction
    {
        public string Name { get; set; }
        public VM.Value Value { get; set; }

        public SetInstruction(string name, VM.Value value)
        {
            Name = name; Value = value;
        }

        public bool Execute(VM vm)
        {
            vm.variables.Add(Name, Value);
            return true;
        }
    }
}