using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingDataFromExcel
{
    /// <summary>
    /// this is main constructor 
    /// </summary>
    public class InstanceSettings
    {
        Random random = new Random();
        public int lb_duration_ave;
        public int ub_duration_ave;
        public int lb_pre_operatingT_ave;
        public int lb_post_operatingT_ave;
        public int ub_pre_operatingT_ave;
        public int ub_post_operatingT_ave;

        public int index_J;
        public int index_A;
        public int index_K;
        public int index_R;
        public int lengthOfTimeSlot;
        public int lengthOfBlock;

        public int totalRegTimePerRoom;
        public int totalOveTimePerRoom;
        public double reqResourceRatio;

        public int lb_Priority;
        public int ub_Priority;

        public int lb_Transfer;
        public int ub_Transfer;

        public int lb_ResourceSetupTime;
        public int ub_ResourceSetupTime;

        public double ratioDurationToTotal;

        public double totalExtraRoomAssignment;

        public List<InstanceSettings> instancegroups;
        public InstanceSettings() { }

        public InstanceSettings(string grName, int lb_j, int ub_j, int lb_k, int ub_k, int lb_a, int ub_a, double ratioDurTotal)
        {
            random = new Random();
            initial(grName, lb_j, ub_j, lb_k, ub_k, lb_a, ub_a, ratioDurTotal);
        }
        public void initial(string grName, int lb_j, int ub_j, int lb_k, int ub_k, int lb_a, int ub_a, double ratioDurTotal)
        {
            index_J = random.Next(lb_j, ub_j);
            index_K = random.Next(lb_k, ub_k);
            index_A = random.Next(lb_a, ub_a);
            index_R = 10;


            totalRegTimePerRoom = 600;
            lengthOfTimeSlot = totalRegTimePerRoom / 30;
            lengthOfBlock = totalRegTimePerRoom / 10;
            totalOveTimePerRoom = 180;
            reqResourceRatio = 0.005;

            lb_duration_ave = 20;
            ub_duration_ave = 50;

            lb_pre_operatingT_ave = 8;
            ub_pre_operatingT_ave = 21;

            lb_post_operatingT_ave = 5;
            ub_post_operatingT_ave = 10;

            lb_Priority = 1;
            ub_Priority = 10;

            lb_Transfer = 0;
            ub_Transfer = 10;

            lb_ResourceSetupTime = 0;
            ub_ResourceSetupTime = 0;
            ratioDurationToTotal = ratioDurTotal;
            totalExtraRoomAssignment = 2;
        }

        public void createGroups()
        {
            instancegroups = new List<InstanceSettings>();
            int[][] indexJLBUB = new int[][] { new int[] { 10, 20 }, new int[] { 21, 40 }, new int[] { 41, 60 }, new int[] { 61, 80 } };
            int[][] indexKLBUB = new int[][] { new int[] { 3, 4 }, new int[] { 5, 6 }, new int[] { 7, 8 }, new int[] { 9, 10 } };
            int[][] indexALBUB = new int[][] { new int[] { 4, 5 }, new int[] { 6, 8 }, new int[] { 8, 11 }, new int[] { 12, 14 } };
            double[] arrayRatioDurationToTotal = new double[] { 0.3, 0.5, 0.7 };

            int grName = 0;
            for (int j = 0; j < indexJLBUB.Count(); j++)
            {
                for (int ra = 0; ra < arrayRatioDurationToTotal.Length; ra++)
                {
                    grName++;
                    instancegroups.Add(new InstanceSettings("G" + grName, indexJLBUB[j][0], indexJLBUB[j][1], indexKLBUB[j][0], indexKLBUB[j][1], indexALBUB[j][0], indexALBUB[j][1], arrayRatioDurationToTotal[ra]));
                }

            }
        }
    }
}
