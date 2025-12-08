namespace AdventOfCode._2025._8.Models {
    public class JunctionPair
    {
        public JunctionBox Box1 { get; set; }
        public JunctionBox Box2 { get; set; }
        public double Distance { get; set; }

        public JunctionPair(JunctionBox box1, JunctionBox box2, double distance)
        {
            Box1 = box1;
            Box2 = box2;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"{Box1} <-> {Box2} (distance: {Distance:F2})";
        }
    }
}