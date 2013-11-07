namespace BiOWheelsVisualizer
{
    public class VisualizerFactory
    {
        public static IVisualizer CreateVisualizer()
        {
            return new Visualizer();
        }
    }
}
