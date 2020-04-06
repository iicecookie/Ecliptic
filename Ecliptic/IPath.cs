using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
