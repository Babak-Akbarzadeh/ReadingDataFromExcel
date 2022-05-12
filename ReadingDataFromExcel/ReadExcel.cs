using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
namespace ReadingDataFromExcel
{
	public class ReadExcel

    {
        //public DataLayer.UZDBDataContext contex;

        public ReadExcel()
        {
            initial();
            readSurgeonORAssignment();
            ReadSurgeriesData();
            writeAllInstanceAndSolution();
            //MSSUZ();
            //SBB();
        }
        List<int> rooms;
        List<int> resources;
        List<int> patients;
        List<int> surgeons;
        List<List<int>> surgeonRoomAssignemnt;
        int totalTimeslots;
        int lengthOfTimeslot;

        int[] resourceAve;

        public List<Instance> instances;

        public List<OptimalSolution> manualSolution;

        public void writeAllInstanceAndSolution() 
        {
            string path = Directory.GetCurrentDirectory() + "\\" + "RealLife\\";
			if (!Directory.Exists(path))
			{
                Directory.CreateDirectory(path);
			}
			for (int i = 0; i < instances.Count; i++)
			{
                instances[i].WriteXML(path , "ins_" + i.ToString("00")); 
                manualSolution[i].WriteXML(path, "sol_" + i.ToString("00"));
            }
        }
        public void initial() 
        {
            instances = new List<Instance>();
            manualSolution = new List<OptimalSolution>();
            surgeonRoomAssignemnt = new List<List<int>>();
            
        }
        public void initialInstanceData() 
        {
            rooms = new List<int>();
            resources = new List<int>() ;
            patients = new List<int>();
            surgeons = new List<int>();
            totalTimeslots = 0;
        }

        public void readSurgeonORAssignment() 
        {
            Excel.Workbook MyBook = null;
            Excel.Application MyApp = null;
            Excel.Worksheet MySheet = null;

            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\data.xlsx");
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                                                         //var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                                                         // int theWeek = setWeekDateInfo() + 1;

            Boolean Flag = true;
            DateTime dayOfSurgery = Convert.ToDateTime("10/5/2021 8:00", new System.Globalization.CultureInfo("en-GB"));
            initialInstanceData();
            int beginingOfDay = 3;
            for (int index = 3; Flag == true; index++)
            {
                Console.WriteLine("this is row " + index.ToString());
                var MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "AB" + index.ToString()).Cells.Value;
                if (MyValues.GetValue(1, 1) == null)
                {
                    Flag = false;

                    continue;
                }

                int theR = -1;
                int theS = -1;

                if (MyValues.GetValue(1, 10) != null)
                {
                    theR = Convert.ToInt32(MyValues.GetValue(1, 10).ToString());
                }
                if (MyValues.GetValue(1, 1) != null)
                {
                    theS = Convert.ToInt32(MyValues.GetValue(1, 1).ToString());
                }

				if (theS >=0 && theR >=0)
				{
                    setSurgeonORAssignemnt(theS, theR);
				}
            }
        }
        public void ReadSurgeriesData()
        {
            Excel.Workbook MyBook = null;
            Excel.Application MyApp = null;
            Excel.Worksheet MySheet = null;

            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\data.xlsx");
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                                                         //var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                                                         // int theWeek = setWeekDateInfo() + 1;

            Boolean Flag = true;
            DateTime dayOfSurgery = Convert.ToDateTime("10/5/2021 8:00", new System.Globalization.CultureInfo("en-US"));
            initialInstanceData();
            int beginingOfDay = 3;
            for (int index = 3; Flag == true; index++)
            {
                Console.WriteLine("this is row " + index.ToString());
                var MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "AB" + index.ToString()).Cells.Value;
				if (MyValues.GetValue(1, 1) == null)
				{
                    Flag = false;

                    continue;
				}
                DateTime tmpDate = Convert.ToDateTime(MyValues.GetValue(1, 26), new System.Globalization.CultureInfo("en-US"));
                if (tmpDate.Day != dayOfSurgery.Day)
                {
                    resourceAve = new int[10] { rooms.Count, 6, 2, 1, 1, 1, 1, 1, 3, 2 };
                    // create settings 
                    InstanceSettings settings = new InstanceSettings();
                    settings.index_A = surgeons.Count;
                    settings.index_J = patients.Count;
                    settings.index_K = rooms.Count;
                    settings.index_R = resources.Count;
                    settings.index_T = lengthOfTimeslot;
                    settings.totalRegTimePerRoom = totalTimeslots;
                    // create instances

                    string name = tmpDate.Year.ToString() + "-" + tmpDate.Month.ToString() + "-" + tmpDate.Day.ToString();
                    Instance tmpInstance = new Instance(settings, name);
                    OptimalSolution tmpSolution = new OptimalSolution(tmpInstance);
					tmpInstance.initialInputs(settings);
                    int counter = -1;
					for (int i = beginingOfDay; i < index; i++)
					{
                        MyValues = (System.Array)MySheet.get_Range("A" +
                       i.ToString(), "AB" + i.ToString()).Cells.Value;
                        counter++;
                        tmpDate = Convert.ToDateTime(MyValues.GetValue(1, 26), new System.Globalization.CultureInfo("en-GB"));

                        // waiting list
                        int theS = returnIndex(Convert.ToInt32(MyValues.GetValue(1, 1).ToString()), surgeons);
                        int theP = returnIndex(Convert.ToInt32(MyValues.GetValue(1, 27).ToString()), patients);

                        tmpInstance.wl_ja[theP][theS] = true;

                        // time
                        int startTime = (tmpDate.Hour-8) * 60 + tmpDate.Minute;
                        int duration = 0;
                        int preDuration = 0;
                        int postDuration = 0;
                        if (MyValues.GetValue(1, 5) != null)
                        {
                            duration = Convert.ToInt32(MyValues.GetValue(1, 5).ToString());
                        }
                        if (MyValues.GetValue(1, 6) != null)
                        {
                            preDuration = Convert.ToInt32(MyValues.GetValue(1, 6).ToString());
                        }
                        if (MyValues.GetValue(1, 7) != null)
                        {
                            postDuration = Convert.ToInt32(MyValues.GetValue(1, 7).ToString());
                        }
                        tmpInstance.duration_j[theP] = duration;
                        tmpInstance.pre_operatingT_j[theP] = preDuration;
                        tmpInstance.post_operatingT_j[theP] = postDuration;
                        int completionTime = startTime + duration + preDuration + postDuration;

                        // resources 
                        int theRsc1 = -1;
                        int theRsc2 = -1;
                        int theRsc3 = -1;

                        if (MyValues.GetValue(1, 12) != null)
                        {
                            theRsc1 = Convert.ToInt32(MyValues.GetValue(1, 12).ToString());
                        }
                        if (MyValues.GetValue(1, 13) != null)
                        {
                            theRsc2 = Convert.ToInt32(MyValues.GetValue(1, 13).ToString());
                        }
                        if (MyValues.GetValue(1, 14) != null)
                        {
                            theRsc3 = Convert.ToInt32(MyValues.GetValue(1, 14).ToString());
                        }

						if (theRsc1 >= 0)
						{
                            tmpInstance.requiredResources_jr[theP][returnIndex(theRsc1, resources)] = true;
                            tmpInstance.amountAvailable_r[returnIndex(theRsc1, resources)] = resourceAve[returnIndex(theRsc1, resources)];
						}
                        if (theRsc2 >= 0)
                        {
                            tmpInstance.requiredResources_jr[theP][returnIndex(theRsc2, resources)] = true;
                            tmpInstance.amountAvailable_r[returnIndex(theRsc2, resources)] = resourceAve[returnIndex(theRsc2, resources)];
                        }
                        if (theRsc3 >= 0)
                        {
                            tmpInstance.requiredResources_jr[theP][returnIndex(theRsc3, resources)] = true;
                            tmpInstance.amountAvailable_r[returnIndex(theRsc3, resources)] = resourceAve[returnIndex(theRsc3, resources)];
                        }

                        int priority = 0;
						if (MyValues.GetValue(1, 9) != null)
						{
                            priority = (int)Math.Ceiling( Convert.ToDouble(MyValues.GetValue(1, 9).ToString()));
                        }
                        tmpInstance.priority_j[theP] = priority;


						// room assignment 
						for (int s = 0; s < surgeonRoomAssignemnt.Count; s++)
						{
							if (surgeonRoomAssignemnt[s][0] == Convert.ToInt32(MyValues.GetValue(1, 1).ToString()))
							{
								for (int r = 1; r < surgeonRoomAssignemnt[s].Count; r++)
								{
                                    int theV = surgeonRoomAssignemnt[s][r];
                                    int theRIndex = returnIndex(theV, rooms);
									if (theRIndex >= 0)
									{
                                        tmpInstance.feasibleOR_ak[theS][theRIndex] = true;
									}
								}
							}
						}
                        if (MyValues.GetValue(1, 15) != null)
                        {
                            int fix = Convert.ToInt32(MyValues.GetValue(1, 15).ToString());
							if (fix == 0)
							{
								for (int r = 0; r < rooms.Count; r++)
								{
                                    tmpInstance.feasibleOR_jk[theP][r] = true;
								}
							}
							else
							{
                                int theR = -1;
                                if (MyValues.GetValue(1, 10) != null)
                                {
                                    theR = Convert.ToInt32(MyValues.GetValue(1, 10).ToString());
                                }
                                for (int r = 0; r < rooms.Count; r++)
                                {
                                    tmpInstance.feasibleOR_jk[theP][r] = false;
                                }
                                tmpInstance.feasibleOR_jk[theP][returnIndex(theR, rooms)] = true;
                            }
                        }

                        // it is done maually? 
                        tmpInstance.transferT_j[theP] = 05;
                        for (int r = 0; r < tmpInstance.settings.index_R; r++)
                        {
                            tmpInstance.setupResource_r[r] = 0;
                        }

                        tmpInstance.M = patients.Count * totalTimeslots;



                        int theRoom = -1;
                        if (MyValues.GetValue(1, 10) != null)
                        {
                            theRoom = Convert.ToInt32(MyValues.GetValue(1, 10).ToString());
                        }




                        // manual solution 
                        tmpSolution.px_jkt[theP][returnIndex(theRoom, rooms)][startTime] = true;
                    }

                    instances.Add(tmpInstance);
                    manualSolution.Add(tmpSolution);

                    initialInstanceData();
                    dayOfSurgery = dayOfSurgery.AddDays(1);
                    beginingOfDay = index;
                    index--;
                    continue;
                }
                else 
                {
                    int theP = -1;
					if (MyValues.GetValue(1, 27) != null)
					{
                        theP = Convert.ToInt32(MyValues.GetValue(1, 27).ToString());
					}

                    int theS = -1;
                    if (MyValues.GetValue(1, 1) != null)
                    {
                        theS = Convert.ToInt32(MyValues.GetValue(1, 1).ToString());
                    }

                    int theR = -1;
                    if (MyValues.GetValue(1, 10) != null)
                    {
                        theR = Convert.ToInt32(MyValues.GetValue(1, 10).ToString());
                    }

                    int theRsc1 = -1;
                    int theRsc2 = -1;
                    int theRsc3 = -1;

                    if (MyValues.GetValue(1, 12) != null)
                    {
                        theRsc1 = Convert.ToInt32(MyValues.GetValue(1, 12).ToString());
                    }
                    if (MyValues.GetValue(1, 13) != null)
                    {
                        theRsc2 = Convert.ToInt32(MyValues.GetValue(1, 13).ToString());
                    }
                    if (MyValues.GetValue(1, 14) != null)
                    {
                        theRsc3 = Convert.ToInt32(MyValues.GetValue(1, 14).ToString());
                    }


                    if (theP >= 0 && addPatient(theP))
					{
                        patients.Add(theP);
					}

                    if (theR >= 0 && addRoom(theR))
                    {
                        rooms.Add(theR);
                    }
                    if (theS >= 0 && addSurgeon(theS))
                    {
                        surgeons.Add(theS);
                    }

					if (theRsc1 >= 0 && addResource(theRsc1))
					{
                        resources.Add(theRsc1);
					}
                    if (theRsc2 >= 0 && addResource(theRsc2))
                    {
                        resources.Add(theRsc2);
                    }
                    if (theRsc3 >= 0 && addResource(theRsc3))
                    {
                        resources.Add(theRsc3);
                    }

                    if (tmpDate.Hour < 8)
					{
						Console.WriteLine();
					}
                    int startTime = (tmpDate.Hour - 8) * 60 + tmpDate.Minute;
                    int duration = 0;
                    int preDuration = 0;
                    int postDuration = 0;
                    if (MyValues.GetValue(1, 5) != null)
                    {
                        duration = Convert.ToInt32(MyValues.GetValue(1, 5).ToString());
                    }
                    if (MyValues.GetValue(1, 6) != null)
                    {
                        preDuration = Convert.ToInt32(MyValues.GetValue(1, 6).ToString());
                    }
                    if (MyValues.GetValue(1, 7) != null)
                    {
                        postDuration = Convert.ToInt32(MyValues.GetValue(1, 7).ToString());
                    }
                    int completionTime = startTime + duration + preDuration + postDuration;
                    if (totalTimeslots < completionTime )
					{
                        totalTimeslots = completionTime;
                    }
                    

                    lengthOfTimeslot = 5;
                    
                }

            }
        }


        public bool addRoom(int theR) 
        {
			foreach (int r in rooms)
			{
				if (r == theR)
				{
                    return false;
				}
			}

            return true;
        }

        public bool addSurgeon(int theS)
        {
            foreach (int s in surgeons)
            {
                if (s == theS)
                {
                    return false;
                }
            }

            return true;
        }

        public bool addPatient(int theP)
        {
            foreach (int p in patients)
            {
                if (p == theP)
                {
                    return false;
                }
            }

            return true;
        }

        public bool addResource(int theR)
        {
            foreach (int p in resources)
            {
                if (p == theR)
                {
                    return false;
                }
            }

            return true;
        }

        public int returnIndex(int theVal, List<int> theList)
        {
			for (int v = 0; v < theList.Count; v++)
			{
				if (theList[v] == theVal)
				{
                    return v;
				}
			}

            return -1;
        }


        public void setSurgeonORAssignemnt(int theS, int theR) 
        {
            bool addSurgeon = true;
			for (int s = 0; s < surgeonRoomAssignemnt.Count; s++)
			{
				if (surgeonRoomAssignemnt[s][0] == theS)
				{
                    addSurgeon = false;
                    bool addRoom = true;
					for (int r = 1; r < surgeonRoomAssignemnt[s].Count; r++)
					{
						if (surgeonRoomAssignemnt[s][r] == theR)
						{
                            addRoom = false;
                            break;
						}
					}

					if (addRoom)
					{
                        surgeonRoomAssignemnt[s].Add(theR);
					}
				}
			}

			if (addSurgeon)
			{
                surgeonRoomAssignemnt.Add(new List<int>());
                surgeonRoomAssignemnt[surgeonRoomAssignemnt.Count - 1].Add(theS);
                surgeonRoomAssignemnt[surgeonRoomAssignemnt.Count - 1].Add(theR);
			}
        }

    }
}
