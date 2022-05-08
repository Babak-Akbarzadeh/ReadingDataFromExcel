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
            instances = new List<Instance>();
            ReadSurgeriesData();
            //MSSUZ();
            //SBB();
        }
        List<int> rooms;
        int resources;
        List<int> patients;
        List<int> surgeons;
        int totalTimeslots;

        public List<Instance> instances;

        public void initialInstanceData() 
        {
            rooms = new List<int>();
            resources = 10;
            patients = new List<int>();
            surgeons = new List<int>();
            totalTimeslots = 0;
        }
         public void ReadSurgeriesData()
        {
            Excel.Workbook MyBook = null;
            Excel.Application MyApp = null;
            Excel.Worksheet MySheet = null;

            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\OR_data.xlsx");
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                                                         //var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                                                         // int theWeek = setWeekDateInfo() + 1;

            Boolean Flag = true;
            DateTime dayOfSurgery = Convert.ToDateTime("10/5/2021 8:00", new System.Globalization.CultureInfo("en-GB"));
            initialInstanceData();
            for (int index = 3; Flag == true; index++)
            {
                Console.WriteLine("this is row " + index.ToString());
                var MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "AA" + index.ToString()).Cells.Value;

                DateTime tmpDate = Convert.ToDateTime(MyValues.GetValue(1, 26), new System.Globalization.CultureInfo("en-GB"));
                if (tmpDate.Day != dayOfSurgery.Day)
                {
                    // create settings 
                    InstanceSettings settings = new InstanceSettings();
                    settings.index_A = surgeons.Count;
                    settings.index_J = patients.Count;
                    settings.index_K = rooms.Count;
                    settings.index_R = resources;
                    settings.index_T = totalTimeslots;
                    // create instances
                    Instance tmpInstance = new Instance();
                    tmpInstance.initialInputs(settings);




                    initialInstanceData();
                    dayOfSurgery.AddDays(1);
                }
                else 
                {
                    int theP = -1;
					if (MyValues.GetValue(1, 8) != null)
					{
                        theP = Convert.ToInt32(MyValues.GetValue(1, 8).ToString());
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
					if (tmpDate.Hour < 8)
					{
						Console.WriteLine();
					}

                    totalTimeslots = (tmpDate.Hour - 8) * 60 + tmpDate.Minute;
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
            foreach (int r in resources)
            {
                if (p == theP)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
