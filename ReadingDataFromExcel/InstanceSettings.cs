using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingDataFromExcel
{
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
        public int index_T;

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

        public double TotalExtraRoomAssignment;

        public string path;

        public List<InstanceSettings> instancegroups;
        public InstanceSettings() { }

    }
}
