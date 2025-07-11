namespace ParkSimulatorTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ParkSimulator.Init();

            ParkSpot spotEntrada = new();

            spotEntrada.Name = "Entrada principal";
            spotEntrada.IsVisitable = true;
            spotEntrada.VisitorCapacity = 100;
            spotEntrada.VisitorOccupation = 0;
            spotEntrada.IsVisitorEntry = true;
            spotEntrada.VisitorsEnterPerHour = 10;

            ParkSimulator.AddParkSpot(spotEntrada);

            ParkSpot spotSalida = new();

            spotSalida.Name = "Entrada principal";
            spotSalida.IsVisitable = true;
            spotSalida.VisitorCapacity = 100;
            spotSalida.VisitorOccupation = 0;
            spotSalida.IsVisitorExit = true;
            spotSalida.VisitorsExitPerHour = 4;

            ParkSimulator.AddParkSpot(spotSalida);

            ParkSpot spotAtraccion1 = new();

            spotAtraccion1.Name = "Noria";
            spotAtraccion1.IsVisitable = true;
            spotAtraccion1.VisitorCapacity = 30;
            spotAtraccion1.VisitorOccupation = 0;

            spotAtraccion1.IsVisitorServicer = true;
            spotAtraccion1.VisitorsServicedPerHour = 90;

            spotAtraccion1.IsVisitorServicePaid = true;
            spotAtraccion1.EarningsPerService = 10;
 
            spotAtraccion1.IsMaintenable = true;
            spotAtraccion1.MaintenanceCostPerHour = 100;

            ParkSimulator.AddParkSpot(spotAtraccion1);

            ParkSpot spotAtraccion2 = new();

            spotAtraccion2.Name = "Autochoques";
            spotAtraccion2.IsVisitable = true;
            spotAtraccion2.VisitorCapacity = 50;
            spotAtraccion2.VisitorOccupation = 0;

            spotAtraccion2.IsVisitorServicer = true;
            spotAtraccion2.VisitorsServicedPerHour = 250;

            spotAtraccion2.IsVisitorServicePaid = true;
            spotAtraccion2.EarningsPerService = 3;
 
            spotAtraccion2.IsMaintenable = true;
            spotAtraccion2.MaintenanceCostPerHour = 120;

            ParkSimulator.AddParkSpot(spotAtraccion2);

            spotEntrada.Connections.Add(spotAtraccion1);
            spotEntrada.Connections.Add(spotAtraccion2);
            spotAtraccion1.Connections.Add(spotAtraccion2);
            spotAtraccion1.Connections.Add(spotAtraccion1);

            spotAtraccion1.Connections.Add(spotSalida);
            spotAtraccion2.Connections.Add(spotSalida);

            ParkSimulator.Play();

            List<ParkSpot> list = ParkSimulator.GetParkSpots();

            for(float t = 0; t < 24; t += 0.02f)
            {
                ParkSimulator.Step(0.02f);

                foreach(ParkSpot s in list)
                {
                    Console.WriteLine("Name: " + s.Name);
                    if(s.IsVisitable)
                    {
                        Console.WriteLine("Capacity: " + s.VisitorCapacity);
                        Console.WriteLine("Occupation: " + s.VisitorOccupation);
                        Console.WriteLine("Waiting for connection: " + s.VisitorsWaitingForConnection);

                        if(s.IsVisitorServicer)
                        {
                            Console.WriteLine("Total serviced " + s.TotalVisitorsServiced);

                            if(s.IsVisitorServicePaid)
                            {
                                Console.WriteLine("Total earnings " + s.TotalEarnings);
                            }
                        }
                        
                    }

                    if(s.IsMaintenable)
                    {
                        Console.WriteLine("Cost" + s.TotalMaintenanceCost);
                    }
                    Console.WriteLine("-----------------");
                }

            }

            ParkSimulator.Stop();

            Console.WriteLine("KPI " + KPI.totalCost + " " +  ParkSimulator.GetKPI(KPI.totalCost));
            Console.WriteLine("KPI " + KPI.totalEarnings + " " + ParkSimulator.GetKPI(KPI.totalEarnings));
            Console.WriteLine("KPI " + KPI.totalProfit  + " " + ParkSimulator.GetKPI(KPI.totalProfit));
        }
    }
}
