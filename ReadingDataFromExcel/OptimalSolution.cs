using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ReadingDataFromExcel
{
    public class OptimalSolution
    {
        public string description;
        public string inf_description;
        public bool infeasible;
        public bool[][] patientAdmission_jk;
        public bool[][] distibutionORs_ak;
        public double[] closingT_j;
        public double[] startT_j;
        public double[] endPre_OperatingT_j;
        public double[] endPost_OperatingT_j;
        public int[] priority_j;
        public double maxClosingT;
        public double[] utilizationOR_k;
        public int[] nTimeblocksUsed_k;
        public int[] latestTimeblock_k;
        public int[] Cmax_k;
        public int sumCmax;
        public int[] finishTime_a;
        public int[] startTime_a;
        public double[] efficiency_a;
        public double efficiencyOR;

        public int totalCompletionTime;
        public bool[][][] px_jkt;
        public Instance instance;


        public double[] deficiency_a;
        public int[] totalORassignment_a;

        public OptimalSolution() { }
        public OptimalSolution(Instance instance)
        {
            this.instance = instance;
            initial();
        }
        public void initial()
        {
            totalCompletionTime = 0;
            patientAdmission_jk = new bool[instance.settings.index_J][];
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                patientAdmission_jk[j] = new bool[instance.settings.index_K];
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    patientAdmission_jk[j][k] = false;
                }
            }

            distibutionORs_ak = new bool[instance.settings.index_A][];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                distibutionORs_ak[a] = new bool[instance.settings.index_K];
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    distibutionORs_ak[a][k] = false;
                }
            }


            closingT_j = new double[instance.settings.index_J];
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                closingT_j[j] = 0;
            }

            startT_j = new double[instance.settings.index_J];
            endPre_OperatingT_j = new double[instance.settings.index_J];
            endPost_OperatingT_j = new double[instance.settings.index_J];
            priority_j = new int[instance.settings.index_J];
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                startT_j[j] = 0;
                endPre_OperatingT_j[j] = 0;
                endPost_OperatingT_j[j] = 0;
                priority_j[j] = 0;
            }

            maxClosingT = new double();
            maxClosingT = 0;

            utilizationOR_k = new double[instance.settings.index_K];
            nTimeblocksUsed_k = new int[instance.settings.index_K];
            latestTimeblock_k = new int[instance.settings.index_K];
            Cmax_k = new int[instance.settings.index_K];
            for (int k = 0; k < instance.settings.index_K; k++)
            {
                utilizationOR_k[k] = 0;
                nTimeblocksUsed_k[k] = 0;
                latestTimeblock_k[k] = 0;
                Cmax_k[k] = 0;
            }

            sumCmax = new int();
            sumCmax = 0;

            finishTime_a = new int[instance.settings.index_A];
            startTime_a = new int[instance.settings.index_A];
            efficiency_a = new double[instance.settings.index_A];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                finishTime_a[a] = 0;
                startTime_a[a] = instance.settings.totalRegTimePerRoom;
                efficiency_a[a] = 0;
            }

            efficiencyOR = 0;
            ArrayInitializer.CreateArray(ref px_jkt, instance.settings.index_J, instance.settings.index_K, instance.settings.totalRegTimePerRoom, false);
        }

        public string printMe(string[] extraInfo)
        {
            string result = "";
            for (int x = 0; x < extraInfo.Length; x++)
            {
                result += extraInfo[x] + "\t";
            }
            result += efficiencyOR + "\t" + maxClosingT + "\t";
            for (int k = 0; k < instance.settings.index_K; k++)
            {
                result += Cmax_k[k] + "\t";

            }
            result += totalCompletionTime;
            return result;
        }

        public void calculateAllParameters()
        {
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                    {
                        if (px_jkt[j][k][t])
                        {
                            patientAdmission_jk[j][k] = true;
                            startT_j[j] = t;
                            closingT_j[j] = startT_j[j] + instance.pre_operatingT_j[j] + instance.duration_j[j];
                            endPre_OperatingT_j[j] = startT_j[j] + instance.pre_operatingT_j[j];
                            endPost_OperatingT_j[j] = closingT_j[j] + instance.post_operatingT_j[j];
                            nTimeblocksUsed_k[k] += instance.pre_operatingT_j[j] + instance.duration_j[j] + instance.post_operatingT_j[j];
                            totalCompletionTime += (int)endPost_OperatingT_j[j];
                            for (int a = 0; a < instance.settings.index_A; a++)
                            {
                                if (instance.wl_ja[j][a])
                                {
                                    distibutionORs_ak[a][k] = true;
                                    if (startTime_a[a] > t + instance.pre_operatingT_j[j])
                                    {
                                        startTime_a[a] = t + instance.pre_operatingT_j[j];

                                    }
                                    if (finishTime_a[a] < t + instance.pre_operatingT_j[j] + instance.duration_j[j])
                                    {
                                        finishTime_a[a] = t + instance.pre_operatingT_j[j] + instance.duration_j[j];

                                    }
                                }


                            }
                            if (Cmax_k[k] < endPost_OperatingT_j[j])
                            {
                                Cmax_k[k] = (int)endPost_OperatingT_j[j];

                            }
                            if (maxClosingT < closingT_j[j])
                            {
                                maxClosingT = closingT_j[j];

                            }
                        }
                    }
                }
            }


            for (int k = 0; k < instance.settings.index_K; k++)
            {
                sumCmax += Cmax_k[k];
                utilizationOR_k[k] = (double)nTimeblocksUsed_k[k] / Cmax_k[k];
            }



            setdeficiency();
            setDescription();
            setInfeasibility();

        }
        public void setDescription()
        {
            description = "\n\n";

            description += "Total completion time: " + totalCompletionTime + "\n";
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                description += "   Surgeon deficiency[" + a.ToString("00") + "]: " + Math.Round(deficiency_a[a] * 100) + "%\n";
            }
            description += "================================================================\n";
            for (int k = 0; k < instance.settings.index_K; k++)
            {
                for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                {
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (px_jkt[j][k][t])
                        {
                            for (int a = 0; a < instance.settings.index_A; a++)
                            {
                                if (instance.wl_ja[j][a])
                                {
                                    description += "px_jkt[" + j.ToString("00") + "][" + k + "][" + t.ToString("000") + "]" + " (" + t.ToString("000")
                                + "-" + (t + instance.pre_operatingT_j[j]).ToString("000") + ", " + (t + instance.pre_operatingT_j[j]).ToString("000") + "-" + (t + instance.pre_operatingT_j[j] + instance.duration_j[j]).ToString("000")
                                + ", " + (t + instance.pre_operatingT_j[j] + instance.duration_j[j]).ToString("000") + "-" + (t + instance.pre_operatingT_j[j] + instance.duration_j[j] + instance.post_operatingT_j[j]).ToString("000") + ")"
                                + " Pr = " + instance.priority_j[j] + " Surgeon: " + a.ToString("00") + "\n";
                                }
                            }


                        }
                    }
                }
            }
            description += "\n\n";
            description += "TimeLine: ";
            for (int t = 0; t < instance.settings.totalRegTimePerRoom; t += instance.settings.lengthOfTimeSlot)
            {
                if (t % instance.settings.lengthOfBlock == 0 && t > 0)
                {
                    description += " || ";
                }
                description += t.ToString("000") + " ";

            }

            description += "|| Clz || Eff\n";
            for (int k = 0; k < instance.settings.index_K; k++)
            {
                string patientTimeLine = "";
                string surgeonTimeLine = "";
                bool[] blockStatus = new bool[instance.settings.totalRegTimePerRoom / instance.settings.lengthOfBlock];
                for (int b = 0; b < instance.settings.totalRegTimePerRoom / instance.settings.lengthOfBlock; b++)
                {
                    blockStatus[b] = false;
                }
                for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                {
                    bool idle = true;
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (px_jkt[j][k][t])
                        {
                            int tt = t;
                            while (tt % instance.settings.lengthOfTimeSlot != 0)
                            {
                                tt--;
                            }
                            idle = false;
                            double totalDur = instance.pre_operatingT_j[j] + instance.duration_j[j] + instance.post_operatingT_j[j];
                            double totalTimeSlot = totalDur / instance.settings.lengthOfTimeSlot;
                            for (int ts = 0; ts < totalTimeSlot; ts++)
                            {
                                if (tt % instance.settings.lengthOfBlock == 0 && tt > 0 && !blockStatus[tt / instance.settings.lengthOfBlock])
                                {
                                    patientTimeLine += " || ";
                                    surgeonTimeLine += " || ";
                                    blockStatus[tt / instance.settings.lengthOfBlock] = true;
                                }
                                if (ts * instance.settings.lengthOfTimeSlot < instance.pre_operatingT_j[j])
                                {

                                    patientTimeLine += "+" + j.ToString("00") + " ";

                                    surgeonTimeLine += "---" + " ";
                                }
                                else if (ts * instance.settings.lengthOfTimeSlot < instance.pre_operatingT_j[j] + instance.duration_j[j])
                                {
                                    patientTimeLine += "~" + j.ToString("00") + " ";

                                    int theA = -1;
                                    for (int a = 0; a < instance.settings.index_A; a++)
                                    {
                                        if (instance.wl_ja[j][a])
                                        {
                                            theA = a;
                                            break;
                                        }
                                    }
                                    surgeonTimeLine += " " + theA.ToString("00") + " ";
                                }
                                else
                                {
                                    patientTimeLine += j.ToString("00") + "+ ";

                                    surgeonTimeLine += "---" + " ";
                                }
                                tt += instance.settings.lengthOfTimeSlot;

                            }
                            if (totalDur == 0)
                            {
                                break;
                            }
                            t += (int)totalDur - 1;
                            break;
                        }



                    }
                    if (idle)
                    {
                        int tt = t;
                        while (tt % instance.settings.lengthOfTimeSlot != 0)
                        {
                            tt--;
                        }
                        if (tt % instance.settings.lengthOfBlock == 0 && tt > 0 && !blockStatus[tt / instance.settings.lengthOfBlock])
                        {
                            patientTimeLine += " || ";
                            surgeonTimeLine += " || ";
                            blockStatus[tt / instance.settings.lengthOfBlock] = true;
                        }
                        if (t % instance.settings.lengthOfTimeSlot == 0 && t > 0)
                        {
                            patientTimeLine += "---" + " ";
                            surgeonTimeLine += "---" + " ";
                        }
                    }
                }

                description += " Patient: " + patientTimeLine + "|| " + Cmax_k[k].ToString("000") + " || " + Math.Round(utilizationOR_k[k] * 100) + "%" + "\n";
                description += " Surgeon: " + surgeonTimeLine + "|| " + Cmax_k[k].ToString("000") + " || " + Math.Round(utilizationOR_k[k] * 100) + "%" + "\n \n";
            }
        }

        public void setdeficiency()
        {
            ArrayInitializer.CreateArray(ref deficiency_a, instance.settings.index_A, 0);
            ArrayInitializer.CreateArray(ref totalORassignment_a, instance.settings.index_A, 0);
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                int start = instance.settings.totalRegTimePerRoom;
                int end = 0;
                int min = 0;
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    bool yesThereIs = false;
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        for (int t = 0; t < instance.settings.totalRegTimePerRoom && instance.wl_ja[j][a]; t++)
                        {
                            if (px_jkt[j][k][t])
                            {
                                yesThereIs = true;
                                if (t < start)
                                {
                                    start = t;
                                }
                                if (t + instance.duration_j[j] + instance.pre_operatingT_j[j] > end)
                                {
                                    end = t + instance.duration_j[j] + instance.pre_operatingT_j[j];
                                }
                                min += instance.duration_j[j];

                            }
                        }
                    }
                    if (yesThereIs)
                    {
                        totalORassignment_a[a]++;

                    }
                }
                if (min > 0)
                {
                    deficiency_a[a] = 1 - (double)min / (end - start);
                }
            }
        }

        public void setInfeasibility()
        {
            infeasible = false;
            inf_description = "\n\n";

            //time check
            List<int>[][] caseStacks_at = new List<int>[instance.settings.index_A][];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                caseStacks_at[a] = new List<int>[instance.settings.totalRegTimePerRoom];
                for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                {
                    caseStacks_at[a][t] = new List<int>();
                }
            }
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                    {
                        if (px_jkt[j][k][t])
                        {
                            for (int a = 0; a < instance.settings.index_A; a++)
                            {
                                if (instance.wl_ja[j][a])
                                {
                                    for (int tt = t + instance.pre_operatingT_j[j]; tt < t + instance.pre_operatingT_j[j] + instance.duration_j[j]; tt++)
                                    {
                                        caseStacks_at[a][tt].Add(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                {
                    if (caseStacks_at[a][t].Count > 1)
                    {
                        infeasible = true;
                        inf_description += "Surgeon " + a + " has overlapping issue at time " + t.ToString("00") + ", here are the overlapping surgical cases:\n";
                        foreach (int item in caseStacks_at[a][t])
                        {
                            inf_description += item.ToString("00") + " ";

                        }
                    }
                }
            }


            for (int a = 0; a < instance.settings.index_A; a++)
            {
                string tmp = "";
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    for (int i = j + 1; i < instance.settings.index_J && instance.wl_ja[j][a]; i++)
                    {
                        if (instance.wl_ja[i][a])
                        {
                            if (instance.priority_j[j] < instance.priority_j[i] && startT_j[j] < startT_j[i])
                            {
                                tmp += "pr_i[" + i + "] = " + instance.priority_j[i] + " pr_j[" + j + "] = " + instance.priority_j[j] + " but start_i[" + i + "] = " + startT_j[i] + "  start_j[" + j + "] = " + startT_j[j] + "\n";
                            }
                            if (instance.priority_j[j] > instance.priority_j[i] && startT_j[j] > startT_j[i])
                            {
                                tmp += "pr_i[" + i + "] = " + instance.priority_j[i] + " pr_j[" + j + "] = " + instance.priority_j[j] + " but start_i[" + i + "] = " + startT_j[i] + "  start_j[" + j + "] = " + startT_j[j] + "\n";
                            }
                        }

                    }
                }
                if (tmp != "")
                {
                    infeasible = true;
                    tmp = "Surgeon " + a + " has sequencing issue: \n" + tmp;
                    inf_description += tmp;

                }
            }


            if (!infeasible)
            {
                inf_description = "\n\n The solution is infeasible";
            }
        }

        public void WriteXML()
        {
            calculateAllParameters();
            string Path = instance.pathes.OutPutGr + instance.name + "Solution.xml";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(OptimalSolution));
            System.IO.FileStream file = System.IO.File.Create(Path);
            writer.Serialize(file, this);
            file.Close();
        }

        public OptimalSolution ReadXML()
        {
            string Path = instance.pathes.OutPutLocation + instance.name + "Solution.xml";
            // Now we can read the serialized book ...  
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Instance));

            System.IO.StreamReader file = new System.IO.StreamReader(Path);
            OptimalSolution tmp = (OptimalSolution)reader.Deserialize(file);
            file.Close();
            return tmp;
        }
    }
}
