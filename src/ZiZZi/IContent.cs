using System;
namespace ZiZZi
{
    public interface IContent<T>
    {
        T Value();
        bool CanTake();
    }
}

