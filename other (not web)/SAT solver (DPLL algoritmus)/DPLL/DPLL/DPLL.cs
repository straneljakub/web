using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DPLL
{
    public class DPLL
    {
        public int level = 0;

        //Algorithm ---------------------------------------------------------------------------------------------------------------------------------------------
        public int AlgDPLL(Formula F, int heuristics)
        {
            level++;
            if (F.IsEmptyClause())
                return 0;

            if (F.IsEmptyFormula())
                return 1;


            Clause unitClause = F.UnitPropagation();
            if (unitClause != null)
            {
                int var = unitClause.VariablesList[0];
                if (var > 0)
                {
                    F = Evaluation(F, var, 1);
                    return AlgDPLL(F, heuristics);
                }
                else
                {
                    F = Evaluation(F, var, 0);
                    return AlgDPLL(F, heuristics);
                }
            }

            int pureLiteral = IsPureLiteral(F);
            if (pureLiteral != 0)
            {
                F = Evaluation(F, pureLiteral, 1);
                return AlgDPLL(F, heuristics);
            }

            Formula oldF;
            switch (heuristics)
            {
                case 1: //DLCS------------------------------------------------------------------------------------------------------
                    oldF = CopyFormula(F);
                    int chosenLiteral = DLCS(F);
                    if (Occurences(F, chosenLiteral) >= Occurences(F, -chosenLiteral))
                    {
                        F = Evaluation(F, chosenLiteral, 1);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                        {
                            F = Evaluation(oldF, chosenLiteral, 0);
                            if (AlgDPLL(F, heuristics) == 1)
                                return 1;
                            else
                                return 0;
                        }
                    }
                    else
                    {
                        F = Evaluation(F, chosenLiteral, 0);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                        {
                            F = Evaluation(oldF, chosenLiteral, 1);
                            if (AlgDPLL(F, heuristics) == 1)
                                return 1;
                            else
                                return 0;
                        }

                    }

                case 2: //MOM ------------------------------------------------------------------------------------------------------------------
                    oldF = CopyFormula(F);
                    chosenLiteral = MOM(F);
                    F = Evaluation(F, chosenLiteral, 1);
                    if (AlgDPLL(F, heuristics) == 1)
                        return 1;
                    else
                    {
                        F = Evaluation(oldF, chosenLiteral, 0);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                            return 0;
                    }

                case 3: //BÖHM ------------------------------------------------------------------------------------------------------------------
                    oldF = CopyFormula(F);
                    chosenLiteral = BOHM(F);
                    F = Evaluation(F, chosenLiteral, 1);
                    if (AlgDPLL(F, heuristics) == 1)
                        return 1;
                    else
                    {
                        F = Evaluation(oldF, chosenLiteral, 0);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                            return 0;
                    }
                case 4: //My Heuristic ------------------------------------------------------------------------------------------------------
                    oldF = CopyFormula(F);
                    chosenLiteral = MyHeuristic(F);
                    F = Evaluation(F, chosenLiteral, 1);
                    if (AlgDPLL(F, heuristics) == 1)
                        return 1;
                    else
                    {
                        F = Evaluation(oldF, chosenLiteral, 0);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                            return 0;
                    }
                default: //DLIS -----------------------------------------------------------------------------------------------------------------
                    oldF = CopyFormula(F);
                    chosenLiteral = DLIS(F);
                    F = Evaluation(F, chosenLiteral, 1);
                    if (AlgDPLL(F, heuristics) == 1)
                        return 1;
                    else
                    {
                        F = Evaluation(oldF, chosenLiteral, 0);
                        if (AlgDPLL(F, heuristics) == 1)
                            return 1;
                        else
                            return 0;
                    }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------       
        public Formula Evaluation(Formula F, int variable, int eval)
        {
            if (variable > 0)
            {
                int i = 0;
                for (i = 0; i < F.ClauseList.Count; i++)
                {
                    if (F.ClauseList[i].VariablesList.Contains(variable))
                    {
                        if (eval == 1)
                        {
                            F.ClauseList.Remove(F.ClauseList[i]);
                            i--;
                            continue;
                        }
                        else
                            F.ClauseList[i].VariablesList.Remove(variable);

                    }
                    if (F.ClauseList[i].VariablesList.Contains(-variable))
                    {
                        if (eval == 1)
                            F.ClauseList[i].VariablesList.Remove(-variable);
                        else
                        {
                            F.ClauseList.Remove(F.ClauseList[i]);
                            i--;
                            continue;
                        }
                    }
                }
            }
            else
            {
                int i = 0;
                for (i = 0; i < F.ClauseList.Count; i++)
                {
                    if (F.ClauseList[i].VariablesList.Contains(variable))
                    {
                        if (eval == 1)
                            F.ClauseList[i].VariablesList.Remove(variable);
                        else
                        {
                            F.ClauseList.Remove(F.ClauseList[i]);
                            i--;
                            continue;
                        }
                    }
                    if (F.ClauseList[i].VariablesList.Contains(-variable))
                    {
                        if (eval == 1)
                        {
                            F.ClauseList.Remove(F.ClauseList[i]);
                            i--;
                            continue;
                        }
                        else
                            F.ClauseList[i].VariablesList.Remove(-variable);

                    }
                }
            }
            return F;
        }

        public int IsPureLiteral(Formula F)
        {
            List<int> literals = PresentLiterals(F);
            foreach (int i in literals)
            {
                if (i > 0 && !literals.Contains(-i))
                    return i;
            }
            return 0;
        }

        public int Occurences(Formula F, int u)
        {
            int count = 0;
            foreach (Clause clause in F.ClauseList)
            {
                if (clause.VariablesList.Contains(u))
                    count++;
            }
            return count;
        }

        public int OccurencesK(Formula F, int u)
        {
            int minClause = int.MaxValue;
            foreach (Clause clause in F.ClauseList)
            {
                if (clause.VariablesList.Count < minClause)
                    minClause = clause.VariablesList.Count;
            }
            int count = 0;
            foreach (Clause clause in F.ClauseList)
            {
                if (clause.VariablesList.Count == minClause)
                {
                    if (clause.VariablesList.Contains(u))
                        count++;
                }
            }
            return count;
        }

        public int OccurencesK(Formula F, int u, int k)
        {
            int count = 0;
            foreach (Clause clause in F.ClauseList)
            {
                if (clause.VariablesList.Count == k)
                {
                    if (clause.VariablesList.Contains(u))
                        count++;
                }
            }
            return count;
        }

        public List<int> PresentLiterals(Formula F)
        {
            List<int> literals = new List<int>();
            foreach (Clause clause in F.ClauseList)
                foreach (int i in clause.VariablesList)
                {
                    if (!literals.Contains(i))
                        literals.Add(i);
                }
            return literals;
        }

        public Formula CopyFormula(Formula F)
        {
            Formula copy = new Formula();
            List<Clause> ClausesList = new List<Clause>();
            foreach (Clause clause in F.ClauseList)
            {
                List<int> VariablesList = new List<int>();
                foreach (int var in clause.VariablesList)
                {
                    VariablesList.Add(var);
                }
                Clause c = new Clause();
                c.VariablesList = VariablesList;
                ClausesList.Add(c);
            }
            copy.ClauseList = ClausesList;
            return copy;
        }

        // Heuristics -----------------------------------------------------------------------------------------------------------------------------------------
        public int DLIS(Formula F)
        {
            int numberOfOccurences;
            int var = 0;
            int max = 0;
            List<int> literals = PresentLiterals(F);
            foreach (int i in literals)
            {
                numberOfOccurences = Occurences(F, i);
                if (numberOfOccurences > max)
                {
                    max = numberOfOccurences;
                    var = i;
                }
            }
            return var;
        }

        public int DLCS(Formula F)
        {
            List<int> literals = PresentLiterals(F);
            int max = 0;
            int var = 0;
            foreach (int i in literals)
            {
                if (i > 0 && literals.Contains(-i))
                {
                    if (Occurences(F, i) + Occurences(F, -i) > max)
                    {
                        max = Occurences(F, i) + Occurences(F, -i);
                        var = i;
                    }
                }
            }
            return var;
        }

        public int MOM(Formula F)
        {
            List<int> literals = PresentLiterals(F);
            int max = 0;
            int var = 0;
            int potentialMax;
            int pVar, nVar, p;
            foreach (int i in literals)
            {
                if (i > 0 && literals.Contains(-i) && (OccurencesK(F, i) + OccurencesK(F, -i)) > 0)
                {
                    pVar = i;
                    nVar = -i;
                    p = ((OccurencesK(F, pVar) * OccurencesK(F, nVar)) / (OccurencesK(F, pVar) + OccurencesK(F, nVar))) + 1;
                    potentialMax = ((OccurencesK(F, pVar) + OccurencesK(F, nVar)) * p) + (OccurencesK(F, pVar) * OccurencesK(F, nVar));
                    if (potentialMax > max)
                    {
                        max = potentialMax;
                        var = i;
                    }
                }
            }
            return var;
        }

        public int BOHM(Formula F)
        {
            int maxClause = 0;
            foreach (Clause clause in F.ClauseList)
            {
                if (clause.VariablesList.Count > maxClause)
                {
                    maxClause = clause.VariablesList.Count;
                }
            }

            List<int> literals = PresentLiterals(F);

            int var = 0;
            string maxString = "0";
            string newString;

            foreach (int i in literals)
            {
                if (i < 0)
                    continue;
                newString = "";
                for (int j = 0; j < maxClause; j++)
                {
                    int p1 = 1;
                    int p2 = 2;
                    int pVar = OccurencesK(F, i, j + 1);
                    int nVar = OccurencesK(F, -i, j + 1);
                    newString += $"{(p1 * (Math.Max(pVar, nVar))) + (p2 * (Math.Min(pVar, nVar)))}";
                }
                if(string.Compare(maxString, newString) == -1)
                {
                    maxString = newString;
                    var = i;
                }
                    
            }
            return var;
        }

        // Maximalizace výskytu literálu a jeho negace v klausulích s průměrnou délkou
        public int MyHeuristic(Formula F)
        {
            List<int> literals = PresentLiterals(F);
            int count = 0;

            foreach (Clause clause in F.ClauseList)
            {
               count += clause.VariablesList.Count;
            }

            int k = count / F.ClauseList.Count;

            int max = 0;
            int var = 0;
            foreach(int i in literals)
            {
                if (i < 0)
                    continue;

                int occurence = OccurencesK(F, i, k) + OccurencesK(F, -i, k);
                if (occurence > max)
                {
                    max = occurence;
                    var = i;
                } 
            }
            return var;
        }


    }
}


