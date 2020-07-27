using System.Collections;
using System.Collections.Generic;

public interface IDependentObject { }

public interface IDependentObject<T>: IDependentObject where T: IDependentObject
{
    IList<T> getDependencies();
}
