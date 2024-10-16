using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Client.Entities
{
    public class Ship
    {
        public char Model { get; set; }
        public List<CoordinatesDto> Coordinates { get; set; }
        public int Size { get; set; }
        public string Orientation { get; set; }
        public CoordinatesDto StartCoordinate { get; set; }

        public bool IsSinked { get; set; }

        public List<CoordinatesDto> HitCoordinates { get; set; }

        public Ship(char model, List<CoordinatesDto> coordinates)
        {
            Model = model;
            Coordinates = coordinates;
            Size = coordinates.Count;
            HitCoordinates = new List<CoordinatesDto>();
            IsSinked = false;
            Orientation = GetOrientation();
            StartCoordinate = GetStartCoordinate();
            Console.WriteLine(Model);
            foreach(var coord in coordinates)
            {
                Console.WriteLine($"  - {coord.Row}, {coord.Column}");
            }
        }

        private string GetOrientation()
        {
            if (Coordinates.Count == 1)
            {
                return "single";
            }
            if (Coordinates[0].Row == Coordinates[1].Row)
            {
                return "horizontal";
            }
            return "vertical";
        }

        private CoordinatesDto GetStartCoordinate()
        {
            if (Orientation == "horizontal")
            {
                Coordinates.Sort((a, b) => a.Column.CompareTo(b.Column));
            }
            else
            {
                Coordinates.Sort((a, b) => a.Row.CompareTo(b.Row));
            }

            return Coordinates[0];
        }
    }
}
