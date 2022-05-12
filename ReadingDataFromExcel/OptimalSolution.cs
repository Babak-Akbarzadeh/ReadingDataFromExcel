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

        Instance instance;
        public bool[][] patientAdmission_jk;
        public bool[][] distibutionORs_ak;
        public bool[][] listOfSurgeries_aj;
        public bool[][][] timeLine_taj;
        public bool[][][] ORtimeLine_tkj;
        public double[] closingT_j;
        public double[] startT_j;
        public double[] endPre_OperatingT_j;
        public double[] endPost_OperatingT_j;
        public int[] priority_j;
        public double maxClosingT;
        public int numberTimeBlocks;
        public double[] utilizationOR_k;
        public double[] utilizationOR2_k;
        public int[] nTimeblocksUsed_k;
        public int[] latestTimeblock_k;
        public int[] Cmax_k;
        public int sumCmax;
        public int[] finishTime_a;
        public int[] startTime_a;
        public int[] totalDur_a;
        public double[] efficiency_a;
        public double efficiencyOR;


        public bool[][][] px_jkt;

        string inf_description;
        bool infeasible;


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

            listOfSurgeries_aj = new bool[instance.settings.index_A][];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                listOfSurgeries_aj[a] = new bool[instance.settings.index_J];
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    listOfSurgeries_aj[a][j] = false;
                }
            }

            numberTimeBlocks = ((int)instance.settings.totalRegTimePerRoom) / instance.settings.index_T;  //!!

            timeLine_taj = new bool[numberTimeBlocks][][];
            for (int t = 0; t < numberTimeBlocks; t++)
            {
                timeLine_taj[t] = new bool[instance.settings.index_A][];
                for (int a = 0; a < instance.settings.index_A; a++)
                {
                    timeLine_taj[t][a] = new bool[instance.settings.index_J];
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        timeLine_taj[t][a][j] = false;
                    }
                }
            }

            ORtimeLine_tkj = new bool[numberTimeBlocks][][];
            for (int t = 0; t < numberTimeBlocks; t++)
            {
                ORtimeLine_tkj[t] = new bool[instance.settings.index_K][];
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    ORtimeLine_tkj[t][k] = new bool[instance.settings.index_J];
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        ORtimeLine_tkj[t][k][j] = false;
                    }
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
            utilizationOR2_k = new double[instance.settings.index_K];
            nTimeblocksUsed_k = new int[instance.settings.index_K];
            latestTimeblock_k = new int[instance.settings.index_K];
            Cmax_k = new int[instance.settings.index_K];
            for (int k = 0; k < instance.settings.index_K; k++)
            {
                utilizationOR_k[k] = 0;
                nTimeblocksUsed_k[k] = 0;
                utilizationOR2_k[k] = 0;
                latestTimeblock_k[k] = 0;
                Cmax_k[k] = 0;
            }

            sumCmax = new int();
            sumCmax = 0;

            finishTime_a = new int[instance.settings.index_A];
            startTime_a = new int[instance.settings.index_A];
            totalDur_a = new int[instance.settings.index_A];
            efficiency_a = new double[instance.settings.index_A];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                finishTime_a[a] = 0;
                startTime_a[a] = 0;
                totalDur_a[a] = 0;
                efficiency_a[a] = 0;
            }
            px_jkt = new bool[instance.settings.index_J][][];
			for (int j = 0; j < instance.settings.index_J; j++)
			{
                px_jkt[j] = new bool[instance.settings.index_K][];
				for (int k = 0; k < instance.settings.index_K; k++)
				{
                    px_jkt[j][k] = new bool[instance.settings.totalRegTimePerRoom];
					for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
					{
                        px_jkt[j][k][t] = true;
					}
				}
			}
            efficiencyOR = 0;
        }

        public void printMe(string path, string name)
        {
            StreamWriter writer = new StreamWriter(path + name + "optimalSol.txt");

            for (int k = 0; k < instance.settings.index_K; k++)
            {
                int max = 0;
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    if (patientAdmission_jk[j][k])
                    {
                        if (endPost_OperatingT_j[j] > max)
                        {
                            max = (int)endPost_OperatingT_j[j];
                            Cmax_k[k] = max;
                        }
                    }
                }
                sumCmax += Cmax_k[k];
            }

            writer.Write("Maximal closing time: " + maxClosingT + " ");
            writer.WriteLine();
            writer.Write("Sum closing times: " + sumCmax + " ");
            writer.WriteLine();
            writer.WriteLine();

            for (int k = 0; k < instance.settings.index_K; k++)
            {
                writer.Write("Closing time Room " + k + "= " + Cmax_k[k]);
                writer.WriteLine();
            }

            writer.WriteLine();
            writer.Write("Surgery XX (StartTime - EndTime ; Priority)");
            writer.WriteLine();

            for (int k = 0; k < instance.settings.index_K; k++)
            {
                writer.Write("Room " + k + ": ");
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    if (patientAdmission_jk[j][k])
                    {
                        writer.Write(j.ToString("00") + "(" + (int)startT_j[j] + " - " + (int)closingT_j[j] + "; " + priority_j[j] + ") ");
                    }
                }
                writer.WriteLine();
            }
            writer.WriteLine();

            for (int a = 0; a < instance.settings.index_A; a++)
            {
                writer.Write("Surgeon " + a + ": ");
                for (int k = 0; k < instance.settings.index_K; k++)
                {
                    if (distibutionORs_ak[a][k])
                    {
                        writer.Write("Room " + k + ", ");
                    }
                }
                writer.WriteLine();
            }
            writer.WriteLine();

            for (int a = 0; a < instance.settings.index_A; a++)
            {
                writer.Write("Surgeries for surgeon " + a + ": ");
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    if (listOfSurgeries_aj[a][j])
                    {
                        writer.Write(j.ToString("00") + " ");
                    }
                }
                writer.WriteLine();
            }

            writer.WriteLine();

            writer.Write("Timescale: " + instance.settings.index_T + " min.");
            writer.WriteLine();
            writer.WriteLine();
            int prvcase = -2;
            int currentcase = -1;
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                writer.Write("Surgeon " + a + ": ");
                string clst = "";

                for (int t = 0; t < numberTimeBlocks; t++)
                {
                    bool check = true;

                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (timeLine_taj[t][a][j])
                        {
                            writer.Write(j.ToString("00") + " ");
                            check = false;

                            if (currentcase != j)
                            {
                                prvcase = currentcase;
                                currentcase = j;
                            }

                        }

                    }
                    if (check)
                    {
                        clst = "";
                        //if (t > 0 && !timeLine_taj[t - 1][a][j])
                        //{
                        //    for (int tt = 0; tt < (double)instance.pre_operatingT_j[j] / instance.settings.index_T; tt++)
                        //    {
                        //        clst += "st ";
                        //    }
                        //    for (int tt = 0; prvcase >= 0 && tt < (double)instance.post_operatingT_j[j - 1] / instance.settings.index_T; tt++)
                        //    {
                        //        clst = "cl " + clst;
                        //    }
                        //}
                        //writer.Write(clst);
                        writer.Write(".. ");
                    }

                }
                writer.WriteLine();
            }

            writer.WriteLine();

            for (int k = 0; k < instance.settings.index_K; k++)
            {
                writer.Write("Room " + k + ": ");
                for (int t = 0; t < numberTimeBlocks; t++)
                {
                    bool check = true;
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (ORtimeLine_tkj[t][k][j])
                        {
                            writer.Write(j.ToString("00") + " ");
                            check = false;
                        }
                    }
                    if (check)
                    {
                        writer.Write(".. ");
                    }

                }
                writer.WriteLine();
            }

            writer.WriteLine();

            for (int k = 0; k < instance.settings.index_K; k++)
            {
                for (int t = 0; t < numberTimeBlocks; t++)
                {
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (ORtimeLine_tkj[t][k][j])
                        {

                            nTimeblocksUsed_k[k]++;
                            latestTimeblock_k[k] = t + 1;
                        }
                    }
                }
                utilizationOR_k[k] = (double)nTimeblocksUsed_k[k] / numberTimeBlocks;
                utilizationOR2_k[k] = (double)nTimeblocksUsed_k[k] / latestTimeblock_k[k];
                writer.Write("utilization Room " + k + "= " + utilizationOR_k[k] + "; " + utilizationOR2_k[k]);
                writer.WriteLine();
            }

            writer.WriteLine();

            int[] mint_a = new int[instance.settings.index_A];
            int[] maxt_a = new int[instance.settings.index_A];
            for (int a = 0; a < instance.settings.index_A; a++)
            {
                mint_a[a] = 999999;
                maxt_a[a] = 0;

                finishTime_a[a] = 0;
                startTime_a[a] = 99999;
                for (int t = 0; t < numberTimeBlocks; t++)
                {
                    for (int j = 0; j < instance.settings.index_J; j++)
                    {
                        if (timeLine_taj[t][a][j])
                        {
                            if (t < mint_a[a])
                            {
                                mint_a[a] = t;
                                startTime_a[a] = mint_a[a] * instance.settings.index_T;
                            }
                            if (t > maxt_a[a])
                            {
                                maxt_a[a] = t;
                                finishTime_a[a] = maxt_a[a] * instance.settings.index_T;
                            }
                        }
                    }
                }
                for (int j = 0; j < instance.settings.index_J; j++)
                {
                    if (instance.wl_ja[j][a])
                    {
                        totalDur_a[a] += instance.duration_j[j];
                    }
                }
                efficiency_a[a] = (double)totalDur_a[a] / (finishTime_a[a] - startTime_a[a]);
                writer.Write("efficiency surgeon " + a + "= " + efficiency_a[a] + "");
                writer.WriteLine();
            }

            writer.WriteLine();

            int totalORdur = 0;
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                totalORdur += instance.pre_operatingT_j[j];
                totalORdur += instance.duration_j[j];
                totalORdur += instance.post_operatingT_j[j];
            }
            efficiencyOR = (double)totalORdur / sumCmax;
            writer.Write("total efficiency OR's= " + efficiencyOR);
            writer.Close();
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
                        }
                    }
                }
            }

            for (int j = 0; j < instance.settings.index_J; j++)
            {
                for (int a = 0; a < instance.settings.index_A; a++)
                {
                    if (instance.wl_ja[j][a])  //waitingList_aj[a][j]
                    {
                        for (int t = (int)endPre_OperatingT_j[j]; t < ((int)closingT_j[j]); t++)
                        {
                            timeLine_taj[t][a][j] = true;
                        }
                    }
                }
            }

        }

        public void setdeficiency()
        {
            deficiency_a = new double[instance.settings.index_A];
            totalORassignment_a = new int[instance.settings.index_A];

            for (int a = 0; a < instance.settings.index_A; a++)
            {
                deficiency_a[a] = 0;
                totalORassignment_a[a] = 0;
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
                            if (px_jkt[k][k][t])
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
                    deficiency_a[a] = 1 - min / (end - start);
                }
            }
        }

        public void setInfeasibility()
        {
            infeasible = false;
            inf_description = "";

            //time check
            for (int j = 0; j < instance.settings.index_J; j++)
            {
                for (int i = 0; i < instance.settings.index_J; i++)
                {
                    if (i != j)
                    {
                        for (int a = 0; a < instance.settings.index_A; a++)
                        {
                            if (instance.wl_ja[j][a] && instance.wl_ja[i][a])
                            {
                                for (int t = 0; t < instance.settings.totalRegTimePerRoom; t++)
                                {
                                    if (timeLine_taj[t][a][j] && timeLine_taj[t][a][i])
                                    {
                                        infeasible = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void WriteXML(string Path, string name)
        {

            Path = Path + name + "Solution.xml";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(OptimalSolution));
            System.IO.FileStream file = System.IO.File.Create(Path);
            writer.Serialize(file, this);
            file.Close();
        }

        public OptimalSolution ReadXML(string Path, string name)
        {
            Path = Path + name + "Solution.xml";
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
