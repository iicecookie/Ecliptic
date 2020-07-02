using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic
{
    public interface IPath
    {
        // получить путь на устройстве
        string GetDatabasePath(string filename);
    }
}
