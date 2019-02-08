namespace TestCompany.Common
{
    public delegate void EventHandler();

    public delegate void EventHandler<T>(T arg);

    public delegate void EventHandler<T1, T2>(T1 arg, T2 arg2);
}