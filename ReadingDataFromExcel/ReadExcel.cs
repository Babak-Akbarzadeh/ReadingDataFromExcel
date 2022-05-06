using System;
using System.Collections;
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
            //contex = new DataLayer.UZDBDataContext();

            //ReadGeneralData();
            ReadSurgeriesData();
            //MSSUZ();
            //SBB();
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
            for (int index = 2; Flag == true; index++)
            {
                Console.WriteLine("this is row " + index.ToString());
                var MyValues = (System.Array)MySheet.get_Range("A" +
                       index.ToString(), "Z" + index.ToString()).Cells.Value;
                //try
                //{

                //    var MyValues = (System.Array)MySheet.get_Range("A" +
                //       index.ToString(), "Z" + index.ToString()).Cells.Value;
                //    if (MyValues.GetValue(1, 1) == null)
                //    {
                //        Flag = false;
                //        continue;
                //    }
                //    string PatName = MyValues.GetValue(1, 1).ToString();
                //    var PQ = (from t in contex.PatientInfos
                //              where t.PatientName == PatName
                //              select t).FirstOrDefault();
                //    if (PQ == null) { continue; }
                //    string RoomName = "";
                //    if (MyValues.GetValue(1, 4) != null)
                //    {
                //        RoomName = MyValues.GetValue(1, 4).ToString();
                //    }
                //    if (RoomName == "")
                //    {
                //        continue;

                //    }
                //    var RQ = (from t in contex.RoomInfos
                //              where t.RoomName == RoomName
                //              select t).FirstOrDefault();
                //    if (RQ == null)
                //    {
                //        continue;
                //    }
                //    string surgeonName = "";
                //    if (MyValues.GetValue(1, 7) != null)
                //    {
                //        surgeonName = MyValues.GetValue(1, 7).ToString();
                //    }
                //    string SurgeonDep = "";
                //    if (MyValues.GetValue(1, 2) != null)
                //    {
                //        SurgeonDep = MyValues.GetValue(1, 2).ToString();
                //    }
                //    if (SurgeonDep == "")
                //    {
                //        continue;
                //    }
                //    var QS = (from t in contex.SurgeonInfos
                //              where t.SurgeonName == surgeonName && t.SurgeonDepartment == SurgeonDep
                //              select t).FirstOrDefault();
                //    if (QS == null)
                //    {
                //        continue;
                //    }
                //    var redondantData = (from t in contex.SurgeryInfos
                //                         where t.SurgeryPatient == PQ.PatientID
                //                         select t).FirstOrDefault();
                //    if (redondantData == null)
                //    {
                //        contex.SurgeryInfos.InsertOnSubmit(new DataLayer.SurgeryInfo()
                //        {
                //            SurgeryPatient = PQ.PatientID,
                //            SurgeryDate = MyValues.GetValue(1, 3).ToString(),
                //            NeededNurse = 2,
                //            SurgeryRoom = RQ.RoomID,
                //            SurgerySurgeon = QS.SurgeonID,
                //            PlannedComplitionTime = MyValues.GetValue(1, 25).ToString(),
                //            PlannedStartTime = MyValues.GetValue(1, 24).ToString(),
                //            RealStartTime = MyValues.GetValue(1, 17).ToString(),
                //            RealComplitionTime = MyValues.GetValue(1, 19).ToString(),

                //        });
                //        contex.SubmitChanges();
                //    }


                //}
                //catch (Exception ex)
                //{
                //    ex.ToString();
                //}
            }


        }
        //public void ReadGeneralData()
        //{

        //    Excel.Workbook MyBook = null;
        //    Excel.Application MyApp = null;
        //    Excel.Worksheet MySheet = null;

        //    MyApp = new Excel.Application();
        //    MyApp.Visible = false;
        //    MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\UZCase.xlsx");
        //    MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
        //                                                 //var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        //                                                 // int theWeek = setWeekDateInfo() + 1;

        //    Boolean Flag = true;
        //    // for (int index = 2; index <= lastRow; index++)
        //    for (int index = 2; Flag == true; index++)
        //    {
        //        Console.WriteLine("this is row " + index.ToString());
        //        try
        //        {
        //            var MyValues = (System.Array)MySheet.get_Range("A" +
        //               index.ToString(), "Z" + index.ToString()).Cells.Value;
        //            if (MyValues.GetValue(1, 1) == null)
        //            {
        //                Flag = false;
        //                continue;
        //            }
        //            string PatName = MyValues.GetValue(1, 1).ToString();
        //            var PQ = (from t in contex.PatientInfos
        //                      where t.PatientName == PatName
        //                      select t).FirstOrDefault();
        //            if (PQ == null)
        //            {
        //                string Order = "";
        //                if (MyValues.GetValue(1, 5) != null)
        //                {
        //                    Order = MyValues.GetValue(1, 5).ToString();
        //                }
        //                string OrderD = "";
        //                if (MyValues.GetValue(1, 6) != null)
        //                {
        //                    OrderD = MyValues.GetValue(1, 6).ToString();
        //                }
        //                string RStrt = "";
        //                if (MyValues.GetValue(1, 17) != null)
        //                {
        //                    RStrt = MyValues.GetValue(1, 17).ToString();
        //                }
        //                string RFin = "";
        //                if (MyValues.GetValue(1, 19) != null)
        //                {
        //                    RFin = MyValues.GetValue(1, 19).ToString();
        //                }

        //                DateTime RealStart = Convert.ToDateTime(RStrt);
        //                DateTime RealFin = Convert.ToDateTime(RFin);
        //                int RDif = RealFin.Subtract(RealStart.TimeOfDay).Hour * 60 + RealFin.Subtract(RealStart.TimeOfDay).Minute;
        //                string MStrt = "";
        //                if (MyValues.GetValue(1, 24) != null)
        //                {
        //                    MStrt = MyValues.GetValue(1, 24).ToString();
        //                }
        //                string MFin = "";
        //                if (MyValues.GetValue(1, 25) != null)
        //                {
        //                    MFin = MyValues.GetValue(1, 25).ToString();
        //                }
        //                DateTime MeanStart = Convert.ToDateTime(MStrt);
        //                DateTime MeanFin = Convert.ToDateTime(MFin);
        //                int MDif = MeanFin.Subtract(MeanStart.TimeOfDay).Hour * 60 + MeanFin.Subtract(MeanStart.TimeOfDay).Minute;
        //                contex.PatientInfos.InsertOnSubmit(new DataLayer.PatientInfo()
        //                {
        //                    PatientName = PatName,
        //                    OrderDesc = OrderD,
        //                    PatientOrder = Order,
        //                    MeanSurgeryDur = MDif,
        //                    RealSurgeryDur = RDif,
        //                });
        //            }
        //            string RoomName = "";
        //            if (MyValues.GetValue(1, 4) != null)
        //            {
        //                RoomName = MyValues.GetValue(1, 4).ToString();
        //                if (RoomName == "C1ZA")
        //                {
        //                    continue;
        //                }
        //            }
        //            if (RoomName != "")
        //            {
        //                var RQ = (from t in contex.RoomInfos
        //                          where t.RoomName == RoomName
        //                          select t).FirstOrDefault();
        //                if (RQ == null)
        //                {
        //                    contex.RoomInfos.InsertOnSubmit(new DataLayer.RoomInfo()
        //                    {
        //                        RoomName = RoomName,
        //                    });
        //                }
        //            }
        //            string surgeonName = "";
        //            if (MyValues.GetValue(1, 7) != null)
        //            {
        //                surgeonName = MyValues.GetValue(1, 7).ToString();
        //            }
        //            string SurgeonDep = "";
        //            if (MyValues.GetValue(1, 2) != null)
        //            {
        //                SurgeonDep = MyValues.GetValue(1, 2).ToString();
        //            }
        //            if (SurgeonDep != "")
        //            {
        //                var QS = (from t in contex.SurgeonInfos
        //                          where t.SurgeonName == surgeonName && t.SurgeonDepartment == SurgeonDep
        //                          select t).FirstOrDefault();
        //                if (QS == null)
        //                {
        //                    contex.SurgeonInfos.InsertOnSubmit(new DataLayer.SurgeonInfo()
        //                    {
        //                        SurgeonName = surgeonName,
        //                        SurgeonDepartment = SurgeonDep
        //                    });
        //                }
        //            }

        //            contex.SubmitChanges();

        //        }
        //        catch (Exception ex)
        //        {
        //            ex.ToString();
        //        }
        //    }

        //}
        //public void MSSUZ()
        //{
        //    Excel.Workbook MyBook = null;
        //    Excel.Application MyApp = null;
        //    Excel.Worksheet MySheet = null;

        //    MyApp = new Excel.Application();
        //    MyApp.Visible = false;
        //    MyBook = MyApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\MSS.xlsx");
        //    MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
        //                                                 //var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        //                                                 // int theWeek = setWeekDateInfo() + 1;

        //    Boolean Flag = true;
        //    // for (int index = 2; index <= lastRow; index++)
        //    bool xxpair = true;
        //    int cl = 1;
        //    for (int index = 4; index < 55 && Flag == true; index++)
        //    {
        //        Console.WriteLine("this is row " + index.ToString());
        //        try
        //        {
        //            var MyValues = (System.Array)MySheet.get_Range("M" +
        //               index.ToString(), "O" + index.ToString()).Cells.Value;
        //            if (MyValues.GetValue(1, 1) == null)
        //            {
        //                continue;
        //            }
        //            if (MyValues.GetValue(1, 1).ToString().Contains("CLUSTER"))
        //            {
        //                cl++;
        //                continue;
        //            }
        //            if (MyValues.GetValue(1, 1).ToString().Contains("ONPARE"))
        //            {
        //                xxpair = false;
        //                cl = 0;
        //            }
        //            int room = Convert.ToInt32(MyValues.GetValue(1, 2).ToString());
        //            var mssq = (from t in contex.MSSInfos
        //                        where t.DayofWeek == "Friday" && t.IfPare == xxpair && t.RoomID == room
        //                        select t).FirstOrDefault();
        //            string dep = MyValues.GetValue(1, 1).ToString();
        //            string clos = "20:00";
        //            if (MyValues.GetValue(1, 3).ToString().Contains("17.30"))
        //            {
        //                clos = "17:30";
        //            }
        //            if (MyValues.GetValue(1, 3).ToString().Contains("16.00"))
        //            {
        //                clos = "16:00";
        //            }
        //            mssq.DepName = dep;
        //            mssq.ClosingTime = clos;
        //            mssq.ClusterName = ("C" + cl);
        //            contex.SubmitChanges();

        //        }
        //        catch (Exception ex)
        //        {
        //            ex.ToString();
        //        }
        //    }
        //    contex.SubmitChanges();
        //}
        //public void SBB()
        //{
        //    var query = from t in contex.SurgeryInfos
        //                select t;
        //    foreach (DataLayer.SurgeryInfo item in query)
        //    {
        //        var sbbq = (from t in contex.SBBInfos
        //                    where t.DateTime == item.SurgeryDate && t.SurgeonID == item.SurgerySurgeon && t.RoomID == item.SurgeryRoom && t.FinishTime == item.PlannedStartTime
        //                    select t).FirstOrDefault();
        //        if (sbbq != null)
        //        {
        //            DateTime sbbfinish = Convert.ToDateTime(sbbq.FinishTime);
        //            DateTime itemfinish = Convert.ToDateTime(item.PlannedComplitionTime);
        //            if (sbbfinish < itemfinish)
        //            {
        //                sbbq.FinishTime = item.PlannedComplitionTime;
        //            }

        //            DateTime sbbstart = Convert.ToDateTime(sbbq.StartTime);
        //            DateTime itemstart = Convert.ToDateTime(item.PlannedStartTime);
        //            if (sbbstart > itemstart)
        //            {
        //                sbbq.StartTime = item.PlannedStartTime;
        //            }
        //        }
        //        else
        //        {
        //            DateTime itemfinish = Convert.ToDateTime(item.PlannedComplitionTime);
        //            DateTime itemstart = Convert.ToDateTime(item.PlannedStartTime);
        //            int dayofyear = itemfinish.DayOfYear;
        //            string dayofnewyear = new DateTime(itemfinish.Year, 1, 1).DayOfWeek.ToString();
        //            int firstday = 0;
        //            switch (dayofnewyear)
        //            {
        //                case "Monday":
        //                    firstday = 0;
        //                    break;
        //                case "Tuesday":
        //                    firstday = 6;
        //                    break;
        //                case "Wednesday":
        //                    firstday = 5;
        //                    break;
        //                case "Thursday":
        //                    firstday = 4;
        //                    break;
        //                case "Friday":
        //                    firstday = 3;
        //                    break;
        //                case "Saturday":
        //                    firstday = 2;
        //                    break;
        //                case "Sunday":
        //                    firstday = 1;
        //                    break;

        //                default:
        //                    break;
        //            }
        //            dayofyear -= firstday;
        //            int week = (int)Math.Ceiling(((double)dayofyear / 7));
        //            if (firstday != 0)
        //            {
        //                week++;
        //            }

        //            contex.SBBInfos.InsertOnSubmit(new DataLayer.SBBInfo()
        //            {
        //                DateTime = item.SurgeryDate,
        //                SurgeonID = item.SurgerySurgeon,
        //                Day = itemfinish.DayOfWeek.ToString(),
        //                RoomID = item.SurgeryRoom,
        //                FinishTime = item.PlannedComplitionTime,
        //                StartTime = item.PlannedStartTime,
        //                IfPair = week % 2 == 0
        //            });
        //        }

        //        contex.SubmitChanges();

        //    }
        //}

    }
}
