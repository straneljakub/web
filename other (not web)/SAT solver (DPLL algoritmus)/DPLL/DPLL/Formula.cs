using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DPLL
{
    public class Formula
    {
        public List<Clause> ClauseList = new List<Clause>();

        public int ClausesCount { get; set; }
        public int VariablesCount { get; set; }

        public Formula() { }
        public Formula LoadFormula()
        {
            Formula F = new Formula();
            using (var reader = new StreamReader("ZDE DOPLNTE CESTU K DIMACS SOUBORU "))
            {
                string line = reader.ReadLine();
                string[] firstLine = line.Split(' ');
                F.VariablesCount = int.Parse(firstLine[2]);
                F.ClausesCount = int.Parse(firstLine[3]);

                while ((line = reader.ReadLine()) != null)
                    F.ClauseList.Add(new Clause(line));
            }
            return F;
        }

        public bool IsEmptyClause()
        {
            foreach(var clause in this.ClauseList)
            {
                if (clause.VariablesList.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEmptyFormula()
        {
            return this.ClauseList.Count == 0;
        }

        public Clause UnitPropagation()
        {
            foreach (var clause in this.ClauseList)
            {
                if (clause.VariablesList.Count == 1)
                {
                    return clause;
                }
            }
            return null;
        }

        
        
    }
}
