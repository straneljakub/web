using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLL
{
    class Program
    {
        static void Main()
        {
            Formula F = new Formula();
            F = F.LoadFormula();
            var dpll = new DPLL();

            Console.WriteLine("Literals: " + F.VariablesCount + " Clauses: " + F.ClausesCount);
            Console.WriteLine();

            //DLIS
            int result = dpll.AlgDPLL(F, -1);
            Console.WriteLine("DLIS");
            Console.WriteLine("Is satisfiable: " + result);
            Console.WriteLine("Level: " + dpll.level);
            Console.WriteLine();

            F = F.LoadFormula();
            dpll.level = 0;
            //DLCS
            result = dpll.AlgDPLL(F, 1);
            Console.WriteLine("DLCS");
            Console.WriteLine("Is satisfiable: " + result);
            Console.WriteLine("Level: " + dpll.level);
            Console.WriteLine();

            F = F.LoadFormula();
            dpll.level = 0;
            //MOM
            result = dpll.AlgDPLL(F, 2);
            Console.WriteLine("MOM");
            Console.WriteLine("Is satisfiable: " + result);
            Console.WriteLine("Level: " + dpll.level);
            Console.WriteLine();

            F = F.LoadFormula();
            dpll.level = 0;
            //BÖHM
            result = dpll.AlgDPLL(F, 3);
            Console.WriteLine("BÖHM");
            Console.WriteLine("Is satisfiable: " + result);
            Console.WriteLine("Level: " + dpll.level);
            Console.WriteLine();

            F = F.LoadFormula();
            dpll.level = 0;
            //My Heuristic
            result = dpll.AlgDPLL(F, 4);
            Console.WriteLine("My Heuristic");
            Console.WriteLine("Is satisfiable: " + result);
            Console.WriteLine("Level: " + dpll.level);
            Console.WriteLine();
        }
    }
}
