namespace flyingApp
{
    public interface ISubject
    {
        void attachListener(IObserver obs);
        void detachListener(IObserver obs);
    }
}
