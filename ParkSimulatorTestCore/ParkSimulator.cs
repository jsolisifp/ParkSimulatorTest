using System.Numerics;

namespace ParkSimulatorTest
{
    public enum KPI
    {
        totalCost,
        totalEarnings,
        totalProfit
    }

    public enum SimulatorState
    {
        uninitialized,
        stopped,
        playing,
    }

    public class ParkSimulator
    {
        static SimulatorState state = SimulatorState.uninitialized;
        static float time;

        static List<ParkSpot> parkSpots;
        static Dictionary<string, ParkSpot> parkSpotsById;

        static Random random;

        public static void Init()
        {
            parkSpots = new List<ParkSpot>();
            parkSpotsById = new Dictionary<string, ParkSpot>();
            state = SimulatorState.stopped;
            time = 0;
            random = new Random();
        }

        public static void Play()
        {
            if(state != SimulatorState.stopped) { Console.WriteLine("Error: No se puede reproducir si la simulación no está parada"); return; }

            parkSpots.ForEach(e => e.Start());

            state = SimulatorState.playing;
            time = 0;
        }

        public static void Step(float deltaHours)
        {
            if(state != SimulatorState.playing) { Console.WriteLine("Error: No se puede avanzar un paso si la simulación no está reproduciéndose"); return; }

            parkSpots.ForEach(e => e.Step(deltaHours));

            time += deltaHours;
        }

        public static void Stop()
        {
            if(state != SimulatorState.playing) { Console.WriteLine("Error: No se puede parar si la simulación no está reproduciéndose"); return; }
        }

        public static SimulatorState GetState()
        {
            return state;
        }

        public static float GetTime()
        {
            return time;
        }

        public static List<ParkSpot> GetParkSpots()
        {
            List<ParkSpot> list = new(parkSpots);

            return list;

        }

        public static SimulatorState GetSimulatorState()
        {
            return state;
        }

        public static ParkSpot GetParkSpot(string id)
        {
            if(!parkSpotsById.ContainsKey(id)) { Console.WriteLine("El punto con id " + id + " no existe"); return null; }

            return parkSpotsById[id];
        }

        public static void AddParkSpot(ParkSpot spot)
        {
            if(state != SimulatorState.stopped) { Console.WriteLine("No se puede añadir un punto del parque si la simulación no está parada"); }

            parkSpots.Add(spot);
            parkSpotsById.Add(spot.Id, spot);
        }

        public static void RemoveParkSpot(ParkSpot spot)
        {
            if(state != SimulatorState.stopped) { Console.WriteLine("No se puede quitar un punto del parque si la simulación no está parada"); }

            parkSpots.Remove(spot);
            parkSpotsById.Remove(spot.Id);
        }

        public static float GetKPI(KPI kpi)
        {
            float result;

            if(kpi == KPI.totalCost)
            {
                result = 0;
                parkSpots.ForEach(e => { if(e.IsMaintenable) {  result += e.TotalMaintenanceCost; } });
            }
            else if(kpi == KPI.totalEarnings)
            {
                result = 0;
                parkSpots.ForEach(e => { if(e.IsVisitorServicePaid) { result += e.TotalEarnings; } } );
            }
            else // kpi == KPI.totalProfit
            {
                result = 0;
                parkSpots.ForEach(e => { if(e.IsVisitorServicePaid) { result += e.TotalEarnings; } if(e.IsMaintenable) { result -= e.TotalMaintenanceCost; } } );
            }

            return result;
        }

        internal static Random GetRandom()
        {
            return random;
        }









    }
}
