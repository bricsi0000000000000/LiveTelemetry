namespace DataModel.Live
{
    public class SensorValue
    {
        public int SensorValueId { get; set; }

        public double Value { get; set; }

        public int SensorId { get; set; }

        public int SessionId { get; set; }

        public int PackageId { get; set; }
    }
}
