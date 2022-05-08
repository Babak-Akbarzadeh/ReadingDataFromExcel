using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReadingDataFromExcel
{
	public class Instance

    {
        string description;
        Random random;
        public InstanceSettings settings;

        public bool[][] wl_ja; //public bool[][] waitingList_aj;
        public bool[][] feasibleOR_ak;
        public bool[][] requiredResources_jr;
        public int[] duration_j;
        public int[] lb_duration_a;
        public int[] ub_duration_a;
        public int[] lb_pre_operatingT_a;
        public int[] lb_post_operatingT_a;
        public int[] ub_pre_operatingT_a;
        public int[] ub_post_operatingT_a;
        public int[] pre_operatingT_j;
        public int[] post_operatingT_j;
        public int[] transferT_j;
        public int[] priority_j;
        public int[] setupResource_r;
        public int[] amountAvailable_r;
        public int M;
        public string name;

        public Instance() { }
        public Instance(InstanceSettings settings, string name)

        {
            this.settings = settings;
            initial(name);
        }

        public void initial(string name)
        {
            random = new Random(0);
            this.name = name;

            duration_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                duration_j[j] = 0;
            }
            //createSets();
            createSequence(name);
        }

        public void initialInputs(InstanceSettings settings)
        {
            
            // px_jk
            bool[][] px_jk = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                px_jk[j] = new bool[settings.index_K];
                for (int k = 0; k < settings.index_K; k++)
                {
                    px_jk[j][k] = false;
                }
            }
            wl_ja = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                wl_ja[j] = new bool[settings.index_A];
                for (int a = 0; a < settings.index_A; a++)
                {
                    wl_ja[j][a] = false;
                }
            }

            
            duration_j = new int[settings.index_J];
            pre_operatingT_j = new int[settings.index_J];
            post_operatingT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                duration_j[j] = 0;
                pre_operatingT_j[j] = 0;
                post_operatingT_j[j] = 0;
            }

            

            // Determine feasible ORs
            feasibleOR_ak = new bool[settings.index_A][];
            for (int a = 0; a < settings.index_A; a++)
            {
                feasibleOR_ak[a] = new bool[settings.index_K];

                for (int k = 0; k < settings.index_K; k++)
                {
                    feasibleOR_ak[a][k] = false;
                }
            }



            // Determine required resources
            requiredResources_jr = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                requiredResources_jr[j] = new bool[settings.index_R];
               
                for (int r = 0; r < settings.index_R; r++)
                {
                    requiredResources_jr[j][r] = false;

                }
            }



            amountAvailable_r = new int[settings.index_R];
            for (int r = 0; r < settings.index_R; r++)
            {
                amountAvailable_r[r] = 0;
            }



            transferT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                transferT_j[j] = 0;
            }

            priority_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                priority_j[j] = 0;
            }

            setupResource_r = new int[settings.index_R];
            for (int r = 0; r < settings.index_R; r++)
            {
                setupResource_r[r] = 0;
            }

            M = 1000;

        }

        /*
        public void createSets() 
        {
            waitingList_aj = new bool[settings.index_A] [];
            for (int a = 0; a < settings.index_A; a++)
            {
                waitingList_aj[a] = new bool[settings.index_J];
                for (int j = 0; j < settings.index_J; j++)
                {
                    waitingList_aj[a][j] = false;
                }
            }

            for (int j = 0; j < settings.index_J; j++) 
            {
                int indexS = random.Next(0, settings.index_A);
                waitingList_aj[indexS][j] = true;
            } //assign a surgery to a surgeon at random

            lb_duration_a = new int[settings.index_A];
            for (int a = 0; a < settings.index_A; a++)
            {
                lb_duration_a[a] = random.Next(settings.lb_duration_ave, settings.ub_duration_ave/2);
                int amount = random.Next(0, settings.lb_duration_ave);
                if (random.NextDouble() < 0.5)
                {
                    lb_duration_a[a] -= amount;
                }
            }

            ub_duration_a = new int[settings.index_A];
            for (int a = 0; a < settings.index_A; a++)
            {
                ub_duration_a[a] = random.Next(settings.ub_duration_ave / 2, settings.ub_duration_ave);
                int amont = random.Next(settings.lb_duration_ave, settings.ub_duration_ave);
                if (random.NextDouble() < 0.5)
                {
                    ub_duration_a[a] += amont;
                }
            }

            duration_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                duration_j[j] = 0;
                for (int a = 0; a < settings.index_A; a++)
                {
                    if (waitingList_aj[a][j])
                    {
                        duration_j[j] = random.Next(lb_duration_a[a], ub_duration_a[a]);
                    }
                }
            }

            pre_operatingT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                pre_operatingT_j[j] = 0;
                for (int a = 0; a < settings.index_A; a++)
                {
                    if (waitingList_aj[a][j])
                    {
                        pre_operatingT_j[j] = random.Next(settings.lb_pre_operatingT_ave, settings.ub_pre_operatingT_ave);
                    }
                }
            }

            post_operatingT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                post_operatingT_j[j] = 0;
                for (int a = 0; a < settings.index_A; a++)
                {
                    if (waitingList_aj[a][j])
                    {
                        post_operatingT_j[j] = random.Next(settings.lb_post_operatingT_ave, settings.ub_post_operatingT_ave);
                    }
                }
            }

            feasibleOR_ak = new bool[settings.index_A][];  //!!
            for (int a = 0; a < settings.index_A; a++)
            {
                feasibleOR_ak[a] = new bool[settings.index_K];
                double totalReq = 0;
                double totalAve = (settings.totalRegTimePerRoom) * settings.index_K;
                for (int i = 0; i < settings.index_J; i++)
                {
                    if (waitingList_aj[a][i])
                    {
                        totalReq += duration_j[i]+pre_operatingT_j[i]+post_operatingT_j[i];
                    }
                }

                for (int k = 0; k < settings.index_K; k++)
                {
                    feasibleOR_ak[a][k] = false;
                }

                if (a < settings.index_K)
                {
                    feasibleOR_ak[a][a] = true;
                }
                else
                {//!!
                    int kk = 0;
                    double totalReqMin = 999;
                    for(int k=0; k < settings.index_K; k++)
                    {
                        double totalReq2 = 0;
                        for (int aa=0; aa < a+1; aa++)
                        {
                            if (feasibleOR_ak[aa][k])
                            {
                                
                                for (int i = 0; i < settings.index_J; i++)
                                {                                   
                                    if (waitingList_aj[aa][i])
                                    {
                                        totalReq2 += duration_j[i] + pre_operatingT_j[i] + post_operatingT_j[i];
                                    }
                                }
                            }
                        }
                        if (totalReq2 < totalReqMin)
                        {
                            kk = k;
                            totalReqMin = totalReq2;
                        }
                    }//!!
                    //int kk = random.Next(0, settings.index_K - 1);
                    feasibleOR_ak[a][kk] = true;
                }

                for (int k=0; k < settings.index_K; k++)
                {
                    if (random.NextDouble() < (double)totalReq/totalAve )  
                    {
                        feasibleOR_ak[a][k] = true;
                    }                 
                }   
            }  


            requiredResources_jr = new bool[settings.index_J][];  
            for (int j=0; j < settings.index_J; j++)
            {
                requiredResources_jr[j] = new bool[settings.index_R];
                bool[] reqRes = new bool[settings.index_R];
                for (int rr = 0; rr < settings.index_R; rr++)
                {
                    if (rr == 0)
                    {
                        reqRes[rr] = true;
                    }
                    else
                    {
                        if (random.NextDouble() < settings.reqResourceRatio)              //!! Some resources are only required by 1 surgeon
                        {
                            reqRes[rr] = true;
                        }
                        else
                        {
                            reqRes[rr] = false;
                        }
                    }
                }
                for (int r=0; r<settings.index_R; r++)
                {
                    requiredResources_jr[j][r] = reqRes[r];
                    
                }
            }
          
            transferT_j = new int[settings.index_J];
            for (int j = 0; j< settings.index_J; j++)
            {
                transferT_j[j] = random.Next(settings.lb_Transfer, settings.ub_Transfer) ;
            }

            priority_j = new int[settings.index_J];      //!! Sometimes 7.5; 7.6
            for (int j=0; j<settings.index_J; j++)
            {
                priority_j[j] = random.Next(settings.lb_Priority, settings.ub_Priority);
            }

            setupResource_r = new int[settings.index_R];
            for(int r=0; r<settings.index_R; r++)
            {
                setupResource_r[r] = random.Next(settings.lb_ResourceSetupTime, settings.ub_ResourceSetupTime); 
            }

            amountAvailable_r = new int[settings.index_R];
            amountAvailable_r[0] = settings.index_K;
            //4,6, 2, 1, 1, 1, 1, 1, 3, 2
            amountAvailable_r[1] = 6;
            amountAvailable_r[2] = 2;
            amountAvailable_r[3] = 1;
            amountAvailable_r[4] = 1;
            amountAvailable_r[5] = 1;
            amountAvailable_r[6] = 1;
            amountAvailable_r[7] = 1;
            amountAvailable_r[8] = 3;
            amountAvailable_r[9] = 2;

            
        }*/

        public void createSequence(string name)
        {
            StreamWriter writer = new StreamWriter(name + "inst.txt");
            description = "";
            // px_jk
            bool[][] px_jk = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                px_jk[j] = new bool[settings.index_K];
                for (int k = 0; k < settings.index_K; k++)
                {
                    px_jk[j][k] = false;
                }
            }
            wl_ja = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                wl_ja[j] = new bool[settings.index_A];
                for (int a = 0; a < settings.index_A; a++)
                {
                    wl_ja[j][a] = false;
                }
            }

            // # total patient 
            int[] totalPatientInRoom_k = new int[settings.index_K];
            for (int i = 0; i < settings.index_K; i++)
            {
                totalPatientInRoom_k[i] = 0;
            }
            for (int p = 0; p < settings.index_J; p++)
            {
                int max = 0;
                int min = settings.index_J;
                for (int k = 0; k < settings.index_K; k++)
                {
                    if (totalPatientInRoom_k[k] < min)
                    {
                        min = totalPatientInRoom_k[k];
                    }
                    if (totalPatientInRoom_k[k] > max)
                    {
                        max = totalPatientInRoom_k[k];
                    }
                }
                // random room ass
                int theRoom = -1;
                while (theRoom < 0)
                {
                    theRoom = random.Next(0, settings.index_K);
                    if (totalPatientInRoom_k[theRoom] < max)
                    {
                        px_jk[p][theRoom] = true;
                        totalPatientInRoom_k[theRoom]++;
                    }
                    else if (totalPatientInRoom_k[theRoom] == min)
                    {
                        px_jk[p][theRoom] = true;
                        totalPatientInRoom_k[theRoom]++;
                    }
                    else
                    {
                        theRoom = -1;
                    }

                }
            }
            for (int k = 0; k < settings.index_K; k++)
            {
                description += "Room " + k + ":";
                for (int j = 0; j < settings.index_J; j++)
                {
                    if (px_jk[j][k])
                    {
                        description += j.ToString("00");
                        description += ", ";
                    }
                }
                description += "\n";
            }
            int[] startTime_j = new int[settings.index_J];
            int[] endTime_j = new int[settings.index_J];
            duration_j = new int[settings.index_J];
            pre_operatingT_j = new int[settings.index_J];
            post_operatingT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                startTime_j[j] = 0;
                endTime_j[j] = 0;
                duration_j[j] = 0;
                pre_operatingT_j[j] = 0;
                post_operatingT_j[j] = 0;
            }

            int[] timeRoom_k = new int[settings.index_K];
            for (int k = 0; k < settings.index_K; k++)
            {
                timeRoom_k[k] = 0;
            }
            // duration_i = pre + peri + post
            for (int r = 0; r < settings.index_K; r++)
            {
                // howmany patient = 50
                int totalTimeslots = settings.totalRegTimePerRoom / settings.index_T;
                bool[] timeLine = new bool[totalTimeslots];
                for (int t = 0; t < totalTimeslots; t++)
                {
                    timeLine[t] = false;
                }
                int[] points = new int[totalPatientInRoom_k[r]];
                for (int k = 0; k < totalPatientInRoom_k[r]; k++)
                {
                    points[k] = -1;
                }
                int counter = 0;
                ///
                while (counter < totalPatientInRoom_k[r] && points[counter] < 0)
                {
                    int tmpPoint = random.Next(1, totalTimeslots);
                    if (!timeLine[tmpPoint])
                    {
                        timeLine[tmpPoint] = true;
                        points[counter] = tmpPoint;
                        counter++;

                    }

                }

                /////

                //for (int j = 0; j < totalPatientInRoom_k[r]; j++)
                //{
                //    int tmpPoint = random.Next(1, settings.totalRegTimePerRoom);
                //    if (!timeLine[tmpPoint])
                //    {
                //        timeLine[tmpPoint] = true;
                //        points[j] = tmpPoint;

                //    }
                //    else { j--; }
                //}
                // duration for each patient 
                // sort
                for (int p = 0; p < points.Length; p++)
                {
                    for (int pp = p + 1; pp < points.Length; pp++)
                    {
                        if (points[pp] < points[p])
                        {
                            int tmp = points[pp];
                            points[pp] = points[p];
                            points[p] = tmp;
                        }
                    }
                }
                int pointCounter = 0;
                int curPointer = points[pointCounter];
                int prvPointer = 0;
                for (int j = 0; j < settings.index_J; j++)
                {
                    int compeleteTime = 0;
                    if (px_jk[j][r])
                    {
                        compeleteTime = curPointer - prvPointer;
                        startTime_j[j] = timeRoom_k[r] * settings.index_T;
                        endTime_j[j] = (timeRoom_k[r] + compeleteTime) * settings.index_T;
                        timeRoom_k[r] += compeleteTime;
                        pointCounter++;
                        prvPointer = curPointer;
                        if (pointCounter < totalPatientInRoom_k[r])
                        {
                            curPointer = points[pointCounter];
                        }

                        if (compeleteTime < 5)
                        {
                            duration_j[j] = compeleteTime * settings.index_T;
                            pre_operatingT_j[j] = 0;
                            post_operatingT_j[j] = 0;
                        }
                        else
                        {
                            int dur = random.Next((int)Math.Round(compeleteTime * settings.ratioDurationToTotal), compeleteTime);
                            dur -= dur % settings.index_T;
                            duration_j[j] = dur * settings.index_T;
                            int post = (int)Math.Ceiling((compeleteTime - dur) * random.NextDouble());
                            post -= post % settings.index_T;
                            if (post + dur > compeleteTime)
                            {
                                post_operatingT_j[j] = (compeleteTime - dur) * settings.index_T;
                                pre_operatingT_j[j] = 0;
                            }
                            else
                            {
                                post_operatingT_j[j] = post;
                                pre_operatingT_j[j] = (compeleteTime - dur - post) * settings.index_T;
                            }

                        }
                    }

                }

            }

            for (int j = 0; j < settings.index_J; j++)
            {
                description += "Surgery ";
                description += j.ToString("00");
                description += ": ";
                description += pre_operatingT_j[j].ToString("000");
                description += ", ";
                description += duration_j[j].ToString("000");
                description += ", ";

                description += post_operatingT_j[j].ToString("000");
                description += "\n";
            }


            /*
           //wl_ja
           bool[][] wl_ja = new bool[settings.index_J][];
           for (int j = 0; j < settings.index_J; j++)
           {
               px_jk[j] = new bool[settings.index_A];
               for (int a = 0; a < settings.index_A; a++)
               {
                   wl_ja[j][a] = false;
               }
           }
           // # total patient 
           int[] totalDuration_a = new int[settings.index_A];
           for (int a = 0; a < settings.index_A; a++)
           {
               totalDuration_a[a] = 0;
           }
           for (int j = 0; j < settings.index_J; j++)
           {
               int max = 0;
               int min = int.MaxValue;
               for (int a = 0; a < settings.index_A; a++)
               {
                   if (totalDuration_a[a] < min)
                   {
                       min = totalDuration_a[a];
                   }
                   if (totalDuration_a[a] > max)
                   {
                       max = totalDuration_a[a];
                   }
               }
               // random patient ass
               int theSurgeon = -1;
               int tmpDuration = duration_j[j] + pre_operatingT_j[j] + post_operatingT_j[j];
               while (theSurgeon <= 0)
               {
                   theSurgeon = random.Next(0, settings.index_A);                   
                   if (totalDuration_a[theSurgeon] + tmpDuration > settings.totalRegTimePerRoom - 1)
                   {
                       theSurgeon = -1;
                       bool noSurgeon = true;
                       for (int s = 0; s < settings.index_A; s++)
                       {
                           if (totalDuration_a[s] + tmpDuration < settings.totalRegTimePerRoom - 1)
                           {
                               noSurgeon = false;
                               break;
                           }
                       }

                       if (noSurgeon)
                       {
                           if (pre_operatingT_j[j] > 0)
                           {
                               tmpDuration = duration_j[j]  + post_operatingT_j[j];
                               pre_operatingT_j[j] = 0;
                           }
                           else if (post_operatingT_j[j]>0)
                           {
                               tmpDuration = duration_j[j];
                               post_operatingT_j[j] = 0;
                           }
                           else
                           {
                               duration_j[j]--;
                               tmpDuration = duration_j[j];
                           }
                       }

                   }
                   else if (totalDuration_a[theSurgeon] < max)
                   {
                       wl_ja[j][theSurgeon] = true;
                       totalDuration_a[theSurgeon] +=  tmpDuration;
                   }
                   else if (totalDuration_a[theSurgeon] == min) //!!
                   {
                       wl_ja[j][theSurgeon] = true;
                       totalDuration_a[theSurgeon]+=tmpDuration;
                   }
                   else
                   {
                       theSurgeon = -1;
                   }
               }
           }*/

            // Determine feasible ORs
            feasibleOR_ak = new bool[settings.index_A][];
            for (int a = 0; a < settings.index_A; a++)
            {
                feasibleOR_ak[a] = new bool[settings.index_K];

                for (int k = 0; k < settings.index_K; k++)
                {
                    feasibleOR_ak[a][k] = false;
                }
            }

            for (int s = 0; s < settings.index_A; s++)
            {
                int k = s % settings.index_K;
                feasibleOR_ak[s][k] = true;
            }


            for (int a = 0; a < settings.index_A; a++)
            {
                int total = 0;
                for (int k = 0; k < settings.index_K; k++)
                {
                    if (feasibleOR_ak[a][k])
                    {
                        total++;
                    }
                }
                for (int k = 0; k < settings.index_K && total < settings.TotalExtraRoomAssignment; k++)
                {
                    int sur = 0;
                    for (int aa = 0; aa < settings.index_A; aa++)
                    {
                        if (feasibleOR_ak[aa][k])
                        {
                            sur++;
                        }
                    }

                    int totalSur = 0;
                    for (int aa = 0; aa < settings.index_A; aa++)
                    {
                        for (int kk = 0; kk < settings.index_K; kk++)
                        {
                            if (feasibleOR_ak[aa][kk])
                            {
                                totalSur++;
                            }
                        }

                    }

                    double chance = 1;
                    if (totalSur != 0)
                    {
                        chance = (double)(totalSur - sur) / totalSur;
                    }



                    if (!feasibleOR_ak[a][k] && random.NextDouble() < chance)
                    {
                        feasibleOR_ak[a][k] = true;
                        total++;
                    }
                }

            }

            for (int a = 0; a < settings.index_A; a++)
            {
                description += "Surgeon ";
                description += a;
                description += ": ";
                for (int k = 0; k < settings.index_K; k++)
                {
                    if (feasibleOR_ak[a][k])
                    {
                        description += "room ";
                        description += k;
                        description += ", ";
                    }
                }
                description += "\n";
            }

            //assign surgeries to surgeons


            int[] totalDuration_a = new int[settings.index_A];
            for (int a = 0; a < settings.index_A; a++)
            {
                totalDuration_a[a] = 0;
            }

            for (int j = 0; j < settings.index_J; j++)
            {
                int assignedRoom = -1;
                for (int k = 0; k < settings.index_K; k++)
                {
                    if (px_jk[j][k])
                    {
                        assignedRoom = k;
                        break;
                    }
                }

                int surgeon = -1;
                int tmpDuration = duration_j[j] + pre_operatingT_j[j] + post_operatingT_j[j];
                while (surgeon < 0)
                {
                    int randomSurgeon = random.Next(0, settings.index_A);
                    if (feasibleOR_ak[randomSurgeon][assignedRoom])
                    {

                        if (totalDuration_a[randomSurgeon] + tmpDuration > settings.totalRegTimePerRoom - 1)
                        {
                            surgeon = -1;
                            bool noSurgeon = true;
                            for (int a = 0; a < settings.index_A && feasibleOR_ak[a][assignedRoom]; a++)
                            {
                                if (totalDuration_a[a] + tmpDuration < settings.totalRegTimePerRoom - 1)
                                {
                                    noSurgeon = false;
                                    break;
                                }
                            }

                            if (noSurgeon)
                            {
                                if (pre_operatingT_j[j] > 0)
                                {
                                    tmpDuration = duration_j[j] + post_operatingT_j[j];
                                    pre_operatingT_j[j] = 0;
                                }
                                else if (post_operatingT_j[j] > 0)
                                {
                                    tmpDuration = duration_j[j];
                                    post_operatingT_j[j] = 0;
                                }
                                else
                                {
                                    duration_j[j]--;
                                    tmpDuration = duration_j[j];
                                }
                            }
                        }
                        else
                        {
                            surgeon = randomSurgeon;
                            totalDuration_a[randomSurgeon] += tmpDuration;
                        }
                    }
                }
                wl_ja[j][surgeon] = true;
            }

            for (int a = 0; a < settings.index_A; a++)
            {
                description += "Surgeon ";
                description += a;
                description += ": ";
                for (int j = 0; j < settings.index_J; j++)
                {
                    if (wl_ja[j][a])
                    {
                        description += j.ToString("00");
                        description += ", ";
                    }
                }
                description += "\n";
            }

            // Determine required resources
            requiredResources_jr = new bool[settings.index_J][];
            for (int j = 0; j < settings.index_J; j++)
            {
                requiredResources_jr[j] = new bool[settings.index_R];
                bool[] reqRes = new bool[settings.index_R];
                for (int rr = 0; rr < settings.index_R; rr++)
                {
                    if (rr == 0)
                    {
                        reqRes[rr] = true;
                    }
                    else
                    {
                        if (random.NextDouble() < settings.reqResourceRatio)
                        {
                            reqRes[rr] = true;
                        }
                        else
                        {
                            reqRes[rr] = false;
                        }
                    }
                }
                for (int r = 0; r < settings.index_R; r++)
                {
                    requiredResources_jr[j][r] = reqRes[r];

                }
            }


            int[][] amountNeeded_rt = new int[settings.index_R][];
            for (int r = 0; r < settings.index_R; r++)
            {
                amountNeeded_rt[r] = new int[settings.totalRegTimePerRoom];
                for (int t = 0; t < settings.totalRegTimePerRoom; t++)
                {
                    amountNeeded_rt[r][t] = 0;
                }
            }


            // create amountAvailable_r based on max number used
            for (int t = 0; t < settings.totalRegTimePerRoom; t++)
            {
                for (int j = 0; j < settings.index_J; j++)
                {
                    if (startTime_j[j] <= t && endTime_j[j] >= t)
                    {
                        for (int r = 0; r < settings.index_R; r++)
                        {
                            if (requiredResources_jr[j][r])
                            {
                                amountNeeded_rt[r][t]++;
                            }
                        }
                    }
                }
            }

            int[] maxAmountNeeded_r = new int[settings.index_R];
            for (int r = 0; r < settings.index_R; r++)
            {
                maxAmountNeeded_r[r] = 0;
            }

            for (int r = 0; r < settings.index_R; r++)
            {
                for (int t = 0; t < settings.totalRegTimePerRoom; t++)
                {
                    if (amountNeeded_rt[r][t] > maxAmountNeeded_r[r])
                    {
                        maxAmountNeeded_r[r] = amountNeeded_rt[r][t];
                    }
                }
            }

            amountAvailable_r = new int[settings.index_R];
            for (int r = 0; r < settings.index_R; r++)
            {
                amountAvailable_r[r] = 0;
            }


            // create amountAvailable_r based on max number used
            for (int r = 0; r < settings.index_R; r++)
            {
                amountAvailable_r[r] = maxAmountNeeded_r[r];
            }



            transferT_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                transferT_j[j] = random.Next(settings.lb_Transfer, settings.ub_Transfer);
            }

            priority_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                priority_j[j] = random.Next(settings.lb_Priority, settings.ub_Priority);
            }

            setupResource_r = new int[settings.index_R];
            for (int r = 0; r < settings.index_R; r++)
            {
                setupResource_r[r] = random.Next(settings.lb_ResourceSetupTime, settings.ub_ResourceSetupTime);
            }

            M = 1000;


            writer.WriteLine(description);
            writer.Close();
        }


        public void WriteXML(string Path, string name)
        {

            Path = Path + name + "Instance.xml";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Instance));
            System.IO.FileStream file = System.IO.File.Create(Path);
            writer.Serialize(file, this);
            file.Close();
        }

        public Instance ReadXML(string Path, string name)
        {
            Path = Path + name + "Instance.xml";
            // Now we can read the serialized book ...  
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Instance));

            System.IO.StreamReader file = new System.IO.StreamReader(Path);
            Instance tmp = (Instance)reader.Deserialize(file);
            file.Close();
            return tmp;
        }

    }
}
