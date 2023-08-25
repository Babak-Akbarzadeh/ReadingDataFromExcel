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
        public string description;
        Random random;
        public InstanceSettings settings;

        public bool[][] wl_ja; //public bool[][] waitingList_aj;
        public bool[][] feasibleOR_ak;
        public bool[][] feasibleOR_jk;
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
        public Pathes pathes;

        public AlgorithmicSettings algorithmicSettings;
        public Instance() { }
        public Instance(InstanceSettings settings, string expriment, string groupName, string name) 
        
        {
            this.settings = settings;
            this.name = name;
            pathes = new Pathes(expriment, "", groupName);
            algorithmicSettings = new AlgorithmicSettings(settings);
            initial();
        }
        public void initialInputs(InstanceSettings settings)
        {

            // px_jk
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

            feasibleOR_jk = new bool[settings.index_J][];
            for (int a = 0; a < settings.index_J; a++)
            {
                feasibleOR_jk[a] = new bool[settings.index_K];

                for (int k = 0; k < settings.index_K; k++)
                {
                    feasibleOR_jk[a][k] = false;
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
        public void initial()
        {
            random = new Random();

            duration_j = new int[settings.index_J];
            for (int j = 0; j < settings.index_J; j++)
            {
                duration_j[j] = 0;
            }
            //createSets();
            createSequence() ;
            setDescription() ;
        }
        

        public void createSequence()
        {
            description = "\n\n";
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
                int totalTimeslots = settings.totalRegTimePerRoom / settings.lengthOfTimeSlot;  
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
                        startTime_j[j] = timeRoom_k[r] * settings.lengthOfTimeSlot;
                        endTime_j[j] = (timeRoom_k[r] + compeleteTime) * settings.lengthOfTimeSlot;
                        timeRoom_k[r] += compeleteTime;
                        pointCounter++;
                        prvPointer = curPointer;
                        if (pointCounter < totalPatientInRoom_k[r])
                        {
                            curPointer = points[pointCounter];
                        }

                        if (compeleteTime <= 1)
                        {
                            duration_j[j] = compeleteTime * settings.lengthOfTimeSlot;
                            pre_operatingT_j[j] = 0;
                            post_operatingT_j[j] = 0;
                        }
                        else
                        {
                            int dur = random.Next((int)Math.Round(compeleteTime * settings.ratioDurationToTotal), compeleteTime);
                            
                            duration_j[j] = dur * settings.lengthOfTimeSlot;
							
                            int post = (int)Math.Round((double)(compeleteTime - dur) * random.NextDouble());
                            if (post + dur > compeleteTime)
                            {
                                post_operatingT_j[j] = (compeleteTime - dur) * settings.lengthOfTimeSlot;
                                pre_operatingT_j[j] = 0;
                            }
                            else
                            {
                                post_operatingT_j[j] = post * settings.lengthOfTimeSlot;
                                pre_operatingT_j[j] = (compeleteTime - dur - post) * settings.lengthOfTimeSlot;
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
                description += pre_operatingT_j[j].ToString("000") ;
                description += ", ";
                description += duration_j[j].ToString("000");
                description += ", ";
                
                description += post_operatingT_j[j].ToString("000");
                description += "\n";
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
                for (int k = 0; k < settings.index_K && total < settings.totalExtraRoomAssignment; k++)
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
                        chance = (double)(totalSur - sur)/ totalSur;
                    }



                    if (!feasibleOR_ak[a][k] && random.NextDouble() < chance)
                    {
                        feasibleOR_ak[a][k] = true;
                        total++;
                    }
                }

            }

            for(int a = 0; a < settings.index_A; a++)
            {
                description += "Surgeon ";
                description += a;
                description += ": ";
                for (int k=0; k < settings.index_K; k++)
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

            int[] sortedPat_j = new int[settings.index_J];
			for (int j = 0; j < settings.index_J; j++)
			{
                sortedPat_j[j] = j;
			}
			for (int i = 0; i < settings.index_J; i++)
			{
				for (int j = i + 1; j < settings.index_J; j++)
				{
                    int tmpi = duration_j[sortedPat_j[i]] + pre_operatingT_j[sortedPat_j[i]] + post_operatingT_j[sortedPat_j[i]];
                    int tmpj = duration_j[sortedPat_j[j]] + pre_operatingT_j[sortedPat_j[j]] + post_operatingT_j[sortedPat_j[j]];
					if (tmpi > tmpj)
					{
                        int tmp = sortedPat_j[i];
                        sortedPat_j[i] = sortedPat_j[j];
                        sortedPat_j[j] = tmp;

                    }
                }
			}


            for (int jj = 0; jj < settings.index_J; jj++)
            {
                int j = sortedPat_j[jj];
                int assignedRoom = -1;
                for( int k=0; k < settings.index_K; k++)
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

                        if (totalDuration_a[randomSurgeon] + tmpDuration > settings.totalRegTimePerRoom )
                        {
                            surgeon = -1;
                            bool noSurgeon = true;
                            for (int a = 0; a < settings.index_A && feasibleOR_ak[a][assignedRoom]; a++)
                            {
                                if (totalDuration_a[a] + tmpDuration < settings.totalRegTimePerRoom )
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
                                    duration_j[j] -= settings.lengthOfTimeSlot;
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

            for (int a=0; a < settings.index_A; a++)
            {
                description += "Surgeon ";
                description += a;
                description += ": ";
                for(int j=0; j < settings.index_J; j++)
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
            for (int t=0; t<settings.totalRegTimePerRoom; t++)
            {
                for( int j=0; j<settings.index_J; j++)
                {
                    if (startTime_j[j] <= t && endTime_j[j] >= t)
                    {
                        for (int r=0; r < settings.index_R; r++)
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
            for( int r=0; r < settings.index_R; r++)
            {
                maxAmountNeeded_r[r] = 0;
            }

            for(int r = 0; r < settings.index_R; r++)
            {
                for( int t=0; t < settings.totalRegTimePerRoom; t++)
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
            for (int r=0; r < settings.index_R; r++)
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

            ArrayInitializer.CreateArray(ref feasibleOR_jk, settings.index_J, settings.index_K, true);

			for (int j = 0; j < settings.index_J; j++)
			{
				for (int k = 0; k < settings.index_K; k++)
				{
					if (random.NextDouble() < 0.1)
					{
                        feasibleOR_jk[j][k] = false;
					}
				}
			}

            M = settings.index_J * settings.totalRegTimePerRoom;


        }

        public void setDescription()
        {
            string tmp = "\nWaiting list \n";
            for (int a = 0; a < settings.index_A; a++)
            {

                tmp += "Surgeon " + a.ToString("00") + ":";
                for (int j = 0; j < settings.index_J; j++)
                {
                    if (wl_ja[j][a])
                    {
                        tmp += " " + j.ToString("00");
                    }
                }
                tmp += "\n";
            }


            tmp += "\n \nSurgical cases:\n";
            for (int j = 0; j < settings.index_J; j++)
            {
                tmp += j.ToString("00") + "( pr: " + priority_j[j].ToString("00") + " )" + ": " + pre_operatingT_j[j].ToString("000") + " - " + duration_j[j].ToString("000") + " - " + post_operatingT_j[j].ToString("000") + "\n";
            }


            tmp += "\nRoom Assignemnt:\n";
            for (int k = 0; k < settings.index_K; k++)
            {
                tmp += "Room " + k + ":";
                string list = "";
                for (int a = 0; a < settings.index_A; a++)
                {
                    if (feasibleOR_ak[a][k])
                    {
                        if (list == "")
                        {
                            list += " " + a.ToString("00");
                        }
                        else
                        {
                            list += ", " + a.ToString("00");
                        }
                    }
                }
                tmp += list + "\n";
            }
            description = tmp;
        }
        public void WriteXML()
        {
            setDescription();
            string path = pathes.InsGroupLocation + name + "Instance.xml";
			if (File.Exists(path))
			{
				Console.WriteLine("The instance is already there, nothing has been rewritten");
			}
			else
			{
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Instance));
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, this);
                file.Close();
            }
            
        }

        public Instance ReadXML(string Path, string name)
        {
            Path = Path + name + "Instance.xml";
            if (!File.Exists(Path))
            {
                Console.WriteLine("The instance is not there this is an empty instances ");
                return new Instance();
			}
			else
			{
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
}
