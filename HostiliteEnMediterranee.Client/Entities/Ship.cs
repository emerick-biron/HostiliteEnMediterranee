using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Client.Entities
{
    public class Ship
    {
        public char Model { get; set; }
        public List<CoordinatesDto> Coordinates { get; set; }
        public List<CoordinatesDto> HitCoordinates { get; set; }
        public int Size { get; set; }
        public string Orientation { get; set; }
        public bool IsSinked { get; set; }

        public Ship(char model, List<CoordinatesDto> coordinates)
        {
            Model = model;
            Coordinates = coordinates;
            Size = coordinates.Count;
            HitCoordinates = new List<CoordinatesDto>();
            IsSinked = false;
            Orientation = GetOrientation();
            SortCoordinates(Orientation);
        }

        public Ship(char model, int size)
        {
            Model = model;
            Size = size;
            HitCoordinates = new List<CoordinatesDto>();
            IsSinked = false;
        }

        public void UpdtateOpponentCoordinates(List<CoordinatesDto> coords)
        {
            HitCoordinates = coords;
            Coordinates = coords;
            IsSinked = true;
            Orientation = GetOrientation();
            SortCoordinates(Orientation);
        }

        private string GetOrientation()
        {
            if (Coordinates[0].Row == Coordinates[1].Row)  return "horizontal";
        
            return "vertical";
        }

        private void SortCoordinates(string orientation)
        {
            if (orientation == "horizontal")  Coordinates.Sort((a, b) => a.Column.CompareTo(b.Column));
            else                              Coordinates.Sort((a, b) => a.Row.CompareTo(b.Row));
        }
    }
}
