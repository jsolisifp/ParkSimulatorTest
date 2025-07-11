using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParkSimulatorTest
{
    public class ParkSpot
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Vector2 Location { get; set; } = new Vector2(0, 0);

        public List<ParkSpot> Connections { get { return connections; } }

        public bool IsVisitable { get; set; }
        public int VisitorCapacity { get; set; }
        public float VisitorOccupation { get; set; }

        public bool IsVisitorEntry { get; set; }
        public float VisitorsEnterPerHour { get; set; }

        public bool IsVisitorExit { get; set; }
        public float VisitorsExitPerHour { get; set; }

        public bool IsVisitorServicer { get; set; }
        public float TotalVisitorsServiced { get; set; }
        public float VisitorsServicedPerHour { get; set; }

        public bool IsVisitorServicePaid { get; set; }
        public float EarningsPerService { get; set; }
        public float TotalEarnings { get; set; }

        public bool IsMaintenable { get; set; }
        public float MaintenanceCostPerHour { get; set; }
        public float TotalMaintenanceCost { get; set; }

        List<ParkSpot> connections;

        public float VisitorsWaitingForConnection  {  get; set;  }



        public ParkSpot()
        {
            Id = Guid.NewGuid().ToString();
            Name = "My object";
            connections = new List<ParkSpot>();
        }

        internal void Start()
        {
            TotalVisitorsServiced = 0;
            TotalMaintenanceCost = 0;

            VisitorsWaitingForConnection = 0;
        }

        internal void Step(float deltaHours)
        {
            if(IsVisitable)
            {
                if(IsVisitorEntry)
                {
                    VisitorOccupation += VisitorsEnterPerHour * deltaHours;
                    if(VisitorOccupation >= VisitorCapacity) { VisitorOccupation = VisitorCapacity; }

                    VisitorsWaitingForConnection += VisitorOccupation;
                    VisitorOccupation = 0;
                }

                if(IsVisitorExit)
                {
                    VisitorOccupation -= VisitorsExitPerHour * deltaHours;
                    if(VisitorOccupation <= 0) { VisitorOccupation = 0; }
                }

                if(IsVisitorServicer)
                {
                    float serviced = MathF.Min(VisitorsServicedPerHour * deltaHours, VisitorOccupation);
                    TotalVisitorsServiced += serviced;
                    if(IsVisitorServicePaid) { TotalEarnings += serviced * EarningsPerService; }
                    VisitorOccupation -= serviced;
                    VisitorsWaitingForConnection += serviced;
                }


                while((int)VisitorsWaitingForConnection > 0 && GetConnectionsFreeCapacity() > 0)
                {
                    int randIndex = ParkSimulator.GetRandom().Next() % connections.Count;

                    if(GetConnectionFreeCapacity(connections[randIndex]) > 0)
                    {
                        connections[randIndex].VisitorOccupation ++;
                        VisitorsWaitingForConnection --;
                    }
                }

            }

            if(IsMaintenable)
            {
                TotalMaintenanceCost += MaintenanceCostPerHour * deltaHours;
            }

        }

        internal int GetConnectionFreeCapacity(ParkSpot spot)
        {
            if(spot.IsVisitable)
            {
                return (int)(spot.VisitorCapacity - spot.VisitorOccupation);
            }
            else
            {
                return 0;
            }
        }

        internal int GetConnectionsFreeCapacity()
        {
            int result = 0;
            foreach(ParkSpot c in connections)
            {
                result += GetConnectionFreeCapacity(c);
            }

            return result;
        }
    }

}
