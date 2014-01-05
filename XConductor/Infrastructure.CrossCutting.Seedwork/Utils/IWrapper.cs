namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface IWrapper<out T>
    {
        T Unwrapp();
    }
}
