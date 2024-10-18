namespace HostiliteEnMediterranee.Client.Entities
{
    public class Move
    {
        public string Player { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsHit { get; set; }
        public string AdditionalInfo { get; set; }

        public override string ToString()
        {
            return $"{Player} - ({Row},{Column}) - {(IsHit ? "Hit" : "Miss")}{AdditionalInfo}";
        }
    }
}
