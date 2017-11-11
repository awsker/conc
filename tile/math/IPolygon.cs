using System.Collections.Generic;
using System.Net.Mail;

namespace tile.math
{
    public interface IPolygon
    {
        Line[] Lines { get; }
    }
}
