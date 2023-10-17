using System.Reflection.Metadata.Ecma335;
using System.Text;


namespace Expressive
{
	partial class Parser
	{
        private List<Token> Tokens { get; set; }
        
        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
        }


        public void Parse()
        {
            
        }
    }
}