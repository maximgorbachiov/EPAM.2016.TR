namespace WebInterfaces.Interfaces
{
    public interface IIdGenerator
    {
        int GetId();
        void SetCurentPosition(int position);
        int GetCurrentPosition();
    }
}
