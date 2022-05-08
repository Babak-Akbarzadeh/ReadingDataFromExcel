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
        }

        public void printMe(string path, string name)
        {
            StreamWriter writer = new StreamWriter(path + name + "optimalSol.txt");

            writer.Write("Maximal closing time: " + maxClosingT + " ");
            writer.WriteLine();
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

            writer.Close();
        }
    }
}
