namespace MathParsing
{
    interface IEvaluatable
    {
        int ParameterCount { get; }
        
        double Invoke(double[] Parameters);
    }
}
