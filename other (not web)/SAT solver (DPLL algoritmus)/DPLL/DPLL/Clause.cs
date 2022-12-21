using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLL
{
    public class Clause
    {
        public List<int> VariablesList = new List<int>();

        public Clause() { }
        public Clause(string s)
        {
            int i = 0;
            int v;
            foreach (var variable in s.Split(' '))
            {
                v = int.Parse(variable);
                if (v == 0)
                    break;

                this.VariablesList.Add(v);
                i++;
            }

        }
    }
}
